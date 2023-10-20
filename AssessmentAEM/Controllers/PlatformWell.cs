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
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using System;

namespace AssessmentAEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlatformWell : ControllerBase
    {
        private readonly PlatfromDbContext _context;
  
        public PlatformWell(PlatfromDbContext context) => _context = context;
      
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
        [HttpGet("GetPlatformWellDummy")]
        public async Task<IActionResult> GetPlatformWellDummy()
        {
            try
            {
                //string sqlScript = ReadSqlScriptFromFileOrResource();
                   //_context.Database.ExecuteSqlRaw(sqlScript);

                string jsonFilePath = "./Json/wellDummy.json";
                string jsonString = System.IO.File.ReadAllText(jsonFilePath);
                var options = new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    PropertyNameCaseInsensitive = true,
                };
                var jsonData = System.Text.Json.JsonSerializer.Deserialize<List<PlatformDummy>>(jsonString, options);
                foreach (var data in jsonData)
                {


                    // Check if the Platform exists
                    var existingPlatform = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == data.Id);
                    if (existingPlatform == null)
                    {
                            var platform = new Platform
                        {
                            UniqueName = data.UniqueName,
                            Latitude = data.Latitude,
                            Longitude = data.Longitude,
                            //CreatedAt = DateTime.Now, // You can set this as needed
                            //UpdatedAt = DateTime.Now, // You can set this as needed
                            Wells = data.Wells.Select(w => new Well
                            {
                                UniqueName = w.UniqueName,
                                Latitude = w.Latitude,
                                Longitude = w.Longitude,
                                // Map properties from WellDummy to Well entity
                            }).ToList()
                        };

                        _context.Platforms.Add(platform);

                    }
                    else
                    {
                        // If the Platform exists, update it
                        existingPlatform.UniqueName = data.UniqueName;
                        existingPlatform.Latitude = data.Latitude;
                        existingPlatform.Longitude = data.Longitude;
                        //existingPlatform.CreatedAt = data.CreatedAt;
                        //existingPlatform.UpdatedAt = data.UpdatedAt;
                        existingPlatform.Wells = data.Wells.Select(w => new Well
                        {
                            Id= w.Id,
                            UniqueName = w.UniqueName,
                            Latitude = w.Latitude,
                            Longitude = w.Longitude,
                            // Map properties from WellDummy to Well entity
                        }).ToList();
                    }

                  
                }
                //// Save changes to the database
                await _context.SaveChangesAsync();



                return Ok(jsonData);
            }

            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }
        private string ReadSqlScriptFromFileOrResource()
        {
            return System.IO.File.ReadAllText("./Sql/truncateTable.sql");
        }
    }
}
