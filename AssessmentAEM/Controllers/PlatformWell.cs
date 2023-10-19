using AssessmentAEM.Data;
using AssessmentAEM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AssessmentAEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformWell : ControllerBase
    {
        private readonly PlatfromDbContext _context;
        //string json = File.ReadAllText("yourdata.json");
        ////string json = File.ReadAllText("yourdata.json");
        //List<MyDataModel> data = JsonSerializer.Deserialize<List<MyDataModel>>(json);
        public PlatformWell(PlatfromDbContext context) => _context = context;
        //[HttpPost("GetPlatformWellActual")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult> GetPlatformWellActual(Platform platform)
        //{
        //    //string json = File.ReadAllText("./Json/weelactualRequest.json");
        //    //List<MyDataModel> data = JsonSerializer.Deserialize<List<MyDataModel>>(json);

        //    var existingPlatform = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == platform.Id);
           
        //    if (existingPlatform == null)
        //    {
        //        // If the Platform doesn't exist, insert a new one
        //        _context.Platforms.Add(data);
        //    }
        //    else
        //    {
        //        // If the Platform exists, update it
        //        existingPlatform.uniqueName = data.uniqueName;
        //        existingPlatform.latitude = data.latitude;
        //        existingPlatform.longitude = data.longitude;
        //        existingPlatform.createdAt = data.createdAt;
        //        existingPlatform.updatedAt = data.updatedAt;
        //    }
        //    //var platforms = _context.Platforms.Include(p => p.Wells).ToList();
        //    return Ok(platform);
        //    //return await _context.Platform.ToListAsync();
        //}
        //  [HttpGet("GetPlatformWellDummy")]
        //  public async Task<IEnumerable<PlatformDummy>> GetPlatformWellDummy()
        //  {
        //      //var platforms = _context.Platform.Include(p => p.Well).ToList();
        //      var platforms = _context.Platform
        //.Include(p => p.Well)
        //.Select(p => new PlatformDummy
        //{
        //    id = p.id,
        //    uniqueName = p.uniqueName,
        //    latitude = p.latitude,
        //    longitude = p.longitude,
        //    lastUpdate = p.updatedAt,
        //    Well = p.Well.Select(w => new WellDummy
        //    {
        //        id = w.id,
        //        platformId = w.platformId,
        //        uniqueName = w.uniqueName,
        //        latitude = w.latitude,
        //        longitude = w.longitude,
        //        lastUpdate = w.updatedAt,
        //        // Map properties of WellDummy here
        //    }).ToList()
        //    // Map other properties as needed
        //})
        //.ToList();
        //      return platforms;

        //  }
        [HttpGet("GetPlatformWellActual")]
        public async Task<IActionResult> GetPlatformWellActual()
        {
            try
            {
                string sqlScript = ReadSqlScriptFromFileOrResource();
                _context.Database.ExecuteSqlRaw(sqlScript);

                string jsonFilePath = "./Json/wellactualRequest.json";
                string jsonString = System.IO.File.ReadAllText(jsonFilePath);
                var options = new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    PropertyNameCaseInsensitive = true,
                };
                var jsonData = System.Text.Json.JsonSerializer.Deserialize<List<Platform>>(jsonString, options);
                
                foreach (var data in jsonData)
                {


                    // Check if the Platform exists
                    var existingPlatform = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == data.Id);

                    if (existingPlatform == null)
                    {
                        // If the Platform doesn't exist, insert a new one
                        _context.Platforms.Add(data);
                    }
                    else
                    {
                        // If the Platform exists, update it
                        existingPlatform.UniqueName = data.UniqueName;
                        existingPlatform.Latitude = data.Latitude;
                        existingPlatform.Longitude = data.Longitude;
                        existingPlatform.CreatedAt = data.CreatedAt;
                        existingPlatform.UpdatedAt = data.UpdatedAt;
                    }

                    foreach (var well in data.Wells)
                    {

                        // Check if the Well exists
                        var existingWell = await _context.Wells.FirstOrDefaultAsync(w => w.Id == well.Id);

                        if (well.Id == 0)
                        {
                            // If the Well doesn't exist, insert a new one
                            _context.Wells.Add(well);
                        }
                        else
                        {
                            // If the Well exists, update it
                            existingWell.UniqueName = well.UniqueName;
                            existingWell.Latitude = well.Latitude;
                            existingWell.Longitude = well.Longitude;
                            existingWell.CreatedAt = well.CreatedAt;
                            existingWell.UpdatedAt = well.UpdatedAt;
                        }
                    }
                }
                //// Save changes to the database
                await _context.SaveChangesAsync();



                return Ok(await _context.Platforms.ToListAsync());
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // You can use a logging framework like Serilog, NLog, or the built-in ILogger
                // logger.LogError(ex, "An error occurred while processing the data.");

                return BadRequest(ex);
            }
        }

        private string ReadSqlScriptFromFileOrResource()
        {
            //var sql = File.ReadAllText("./Sql/truncateTable.sql");
            //migrationBuilder.Sql(sql);
            return System.IO.File.ReadAllText("./Sql/truncateTable.sql");
        }
    }
}
