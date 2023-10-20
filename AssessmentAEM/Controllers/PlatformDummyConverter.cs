using AssessmentAEM.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AssessmentAEM.Controllers
{
    public class PlatformDummyConverter : JsonConverter<PlatformDummy>
    {
        public override PlatformDummy Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;

                int id = root.TryGetProperty("id", out var idElement) ? idElement.GetInt32() : 0;

                string uniqueName = root.TryGetProperty("uniqueName", out var uniqueNameElement) ? uniqueNameElement.GetString() : null;
                double latitude = root.TryGetProperty("latitude", out var latitudeElement) ? latitudeElement.GetDouble() : 0.0;
                double longitude = root.TryGetProperty("longitude", out var longitudeElement) ? longitudeElement.GetDouble() : 0.0;
                DateTime createdAt = root.TryGetProperty("createdAt", out var createdAtElement) ? createdAtElement.GetDateTime() : new DateTime(0001, 1, 1);
                DateTime updatedAt = root.TryGetProperty("updatedAt", out var updatedAtteElement) ? updatedAtteElement.GetDateTime() : new DateTime(0001, 1, 1);


                List<WellDummy> wellList = new List<WellDummy>();
                if (root.TryGetProperty("well", out var wellArrayElement) && wellArrayElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var wellElement in wellArrayElement.EnumerateArray())
                    {
                        WellDummy well = new WellDummy
                        {
                            Id = wellElement.TryGetProperty("id", out var wellIdElement) ? wellIdElement.GetInt32() : 0,
                            PlatformId = wellElement.TryGetProperty("platformId", out var platformIdElement) ? platformIdElement.GetInt32() : 0,
                            UniqueName = wellElement.TryGetProperty("uniqueName", out var wellUniqueNameElement) ? wellUniqueNameElement.GetString() : null,
                            Latitude = wellElement.TryGetProperty("latitude", out var wellLatitudeElement) ? wellLatitudeElement.GetDouble() : 0.0,
                            Longitude = wellElement.TryGetProperty("longitude", out var wellLongitudeElement) ? wellLongitudeElement.GetDouble() : 0.0,
                            CreatedAt = wellElement.TryGetProperty("createdAt", out var wellCreatedAtElement) ? wellCreatedAtElement.GetDateTime() : DateTime.Now,
                            UpdatedAt = wellElement.TryGetProperty("updatedAt", out var wellUpdatedAtElement) ? wellUpdatedAtElement.GetDateTime() : DateTime.Now
                        };

                        wellList.Add(well);
                    }
                }

                return new PlatformDummy
                {
                    Id = id,
                    UniqueName = uniqueName,
                    Latitude = latitude,
                    Longitude = longitude,
                    CreatedAt = createdAt,
                    UpdatedAt = updatedAt,
                    Well = wellList
                };

            }
        }

        public override void Write(Utf8JsonWriter writer, PlatformDummy value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}