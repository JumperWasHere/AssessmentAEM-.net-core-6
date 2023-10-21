using AssessmentAEM.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AssessmentAEM.Services
{
    public class AuthenticationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthenticationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetAuthTokenAsync()
        {
            var tokenResponse = "";
            // Use the _httpClientFactory to create an HttpClient
            //var client = _httpClientFactory.CreateClient("MyApi");

            // Send a login request and get the token
            //var response = await client.PostAsync("login", new StringContent("Your login data"));
            //response.EnsureSuccessStatusCode();

            //var token = await response.Content.ReadAsStringAsync();
            //return token;


            var client = _httpClientFactory.CreateClient();
            var loginData = new
            {
                Username = "user@aemenersol.com",
                Password = "Test@123"
            };
            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://test-demo.aemenersol.com/api/Account/Login", content);

            if (response.IsSuccessStatusCode)
            {
                //var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(
                //    await response.Content.ReadAsStringAsync(),
                //    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                //);
                var token = await response.Content.ReadAsStringAsync();
                Console.WriteLine("------------"+token);
                // Save the token for subsequent requests
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                tokenResponse =  token;
            }
                Console.WriteLine("---------no data---");

            return tokenResponse;   
        }
    }

}
