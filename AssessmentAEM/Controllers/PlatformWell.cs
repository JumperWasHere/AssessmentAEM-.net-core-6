﻿using AssessmentAEM.Data;
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
using System.Net.Http;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Azure;
using AssessmentAEM.Services;

namespace AssessmentAEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformWell : ControllerBase
    {
        private readonly PlatfromDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authenticationService;

        public PlatformWell(PlatfromDbContext context, IHttpClientFactory httpClientFactory, HttpClient httpClient, AuthenticationService authenticationService)
            {
                _context = context;
                _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
            _authenticationService = authenticationService;

        }

        [HttpGet("Async-Trucate-table-for-testing")]
        public async Task<IActionResult> getWell()
        {
            string sqlScript = ReadSqlScriptFromFileOrResource();
            _context.Database.ExecuteSqlRaw(sqlScript);
            return Ok("Done truncate table");
            try
            {
                
                var token = await _authenticationService.GetAuthTokenAsync();

               
         
                var client = _httpClientFactory.CreateClient();

               
                //Console.WriteLine("token: "+token);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
                if (client.DefaultRequestHeaders.Authorization != null)
                {
                    // It's set as expected
                    Console.WriteLine("Bearer token is set in the Authorization header: " + client.DefaultRequestHeaders.Authorization);
                }
                else
                {
                    // Authorization header is not set, there might be an issue
                    Console.WriteLine("Bearer token is not set in the Authorization header.");
                }
                var httpResponseMessage = await client.GetAsync("http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual");
                Console.WriteLine("------------ actual");
                if (httpResponseMessage.IsSuccessStatusCode)
                {

                    var dataResult = await httpResponseMessage.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        NumberHandling = JsonNumberHandling.AllowReadingFromString,
                        PropertyNameCaseInsensitive = true,
                    };
                    var jsonData = System.Text.Json.JsonSerializer.Deserialize<List<Platform>>(dataResult, options);
                    //string sqlScript = ReadSqlScriptFromFileOrResource();
                    //_context.Database.ExecuteSqlRaw(sqlScript);
            
                    foreach (var data in jsonData)
                    {
                        // Check if the Platform exists
                        var existingPlatform = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == data.Id);

                        if (existingPlatform == null)
                        {
                            // If the Platform doesn't exist, insert a new one
                            

                            var platform = new Platform
                            {
                                Id = data.Id,
                                UniqueName = data.UniqueName,
                                Latitude = data.Latitude,
                                Longitude = data.Longitude,
                                CreatedAt = data.CreatedAt, // You can set this as needed
                                UpdatedAt = data.UpdatedAt, // You can set this as needed
                                Well = data.Well.Select(w => new Well
                                {
                                    UniqueName = w.UniqueName,
                                    //PlatformId - w.PlatformId,
                                    Latitude = w.Latitude,
                                    Longitude = w.Longitude,
                                    CreatedAt = w.CreatedAt, // You can set this as needed
                                    UpdatedAt = w.UpdatedAt,
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
                            existingPlatform.CreatedAt = data.CreatedAt;
                            existingPlatform.UpdatedAt = data.UpdatedAt;
                            existingPlatform.Well = data.Well.Select(w => new Well
                            {
                                Id = w.Id,
                                UniqueName = w.UniqueName,
                                Latitude = w.Latitude,
                                Longitude = w.Longitude,
                                CreatedAt = w.CreatedAt,
                                UpdatedAt = w.UpdatedAt,
                                // Map properties from WellDummy to Well entity
                            }).ToList();
                        }

                        
                    }
                    //// Save changes to the database
                    await _context.SaveChangesAsync();
                    return Ok("Success store data");
                    

                }
                else
                {
                    // Handle error response here, e.g., log the error or return a specific error message.
                    // You might want to inspect the httpResponseMessage content to see the error details.
                    return BadRequest("Request failed with status code: " + httpResponseMessage.StatusCode);
                }

            }
            catch (HttpRequestException ex)
            {
                // Handle request exceptions, e.g., network issues
                return BadRequest("Request failed with exception: " + ex.Message);
            }

        }

       

        [HttpGet("Async-Data")]
        public async Task<IActionResult> getWellDummy()
        {
            //1. check if need to filter datetime
            //3.test another posibilty change data
            //4.repair filter createAt datetime,, it should do not change when update
            try
            {
                var token = await _authenticationService.GetAuthTokenAsync();

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

                var httpResponseMessage = await client.GetAsync("http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonFilePath = "./Json/wellDummy.json";
                    //string dataResult = System.IO.File.ReadAllText(jsonFilePath);
                    var dataResult = await httpResponseMessage.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        NumberHandling = JsonNumberHandling.AllowReadingFromString,
                        PropertyNameCaseInsensitive = true,
                        Converters = { new PlatformDummyConverter() },
                    };
                    var jsonData = System.Text.Json.JsonSerializer.Deserialize<List<PlatformDummy>>(dataResult, options);
                    //string sqlScript = ReadSqlScriptFromFileOrResource();
                    //_context.Database.ExecuteSqlRaw(sqlScript);

                    foreach (var data in jsonData)
                    {
                        // Check if the Platform exists
                        var existingPlatform = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == data.Id);

                        if (existingPlatform == null)
                        {
                            // If the Platform doesn't exist, insert a new one
                            var platform = new Platform
                            {
                                Id = data.Id,
                            };
                            if (data.UniqueName != null)
                            {
                                platform.UniqueName = data.UniqueName;
                            }
                            if (data.Latitude != 0.0)
                            {
                                platform.Latitude = data.Latitude;
                            }
                            if (data.Longitude != 0.0)
                            {
                                platform.Longitude = data.Longitude;
                            }
                            if (data.CreatedAt != new DateTime(0001, 1, 1))
                            {
                                platform.CreatedAt = data.CreatedAt;
                            }
                            if (data.UpdatedAt != new DateTime(0001, 1, 1))
                            {
                                platform.UpdatedAt = data.UpdatedAt;
                            }
                            //myTestData.Add(platform);
                            //return Ok(platform);
                           
                            platform.Well = data.Well.Select(w => new Well
                            {
                                UniqueName = w.UniqueName,
                                Latitude = w.Latitude,
                                Longitude = w.Longitude,
                                CreatedAt = w.CreatedAt, // Set this as needed
                                UpdatedAt = w.UpdatedAt, // Set this as needed
                                // Map other properties from WellDummy to Well entity
                            }).ToList();
                            
                        _context.Platforms.Add(platform);
                        }
                        else
                        {
                            if (data.UniqueName != null)
                            {
                                existingPlatform.UniqueName = data.UniqueName;
                            }
                            if (data.Latitude != 0.0)
                            {
                                existingPlatform.Latitude = data.Latitude;
                            }
                            if (data.Longitude != 0.0)
                            {
                                existingPlatform.Longitude = data.Longitude;
                            }
                            if (data.UpdatedAt != new DateTime(0001, 1, 1))
                            {
                                existingPlatform.UpdatedAt = data.UpdatedAt;
                            }

                            existingPlatform.Well = data.Well.Select(w => new Well
                            {
                                Id = w.Id,
                                UniqueName = w.UniqueName,
                                Latitude = w.Latitude,
                                Longitude = w.Longitude,
                                //CreatedAt = w.CreatedAt,
                                UpdatedAt = w.UpdatedAt,
                                // Map properties from WellDummy to Well entity
                            }).ToList();
                        }


                    }
                    //// Save changes to the database
                    await _context.SaveChangesAsync();
                    return Ok("ok");


                }
                else
                {
                    // Handle error response here, e.g., log the error or return a specific error message.
                    // You might want to inspect the httpResponseMessage content to see the error details.
                    return BadRequest("Request failed with status code: " + httpResponseMessage.StatusCode);
                }

            }
            catch (HttpRequestException ex)
            {
                // Handle request exceptions, e.g., network issues
                return BadRequest("Request failed with exception: " + ex.Message);
            }

        }

        //[HttpGet("GetPlatformWellActual")]
        //public async Task<IActionResult> GetPlatformWellActual()
        //{
        //try
        //{
        //    string sqlScript = ReadSqlScriptFromFileOrResource();
        //    _context.Database.ExecuteSqlRaw(sqlScript);

        //    string jsonFilePath = "./Json/wellactualRequest.json";
        //    string jsonString = System.IO.File.ReadAllText(jsonFilePath);
        //    var options = new JsonSerializerOptions
        //    {
        //        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        //        PropertyNameCaseInsensitive = true,
        //    };
        //    var jsonData = System.Text.Json.JsonSerializer.Deserialize<List<Platform>>(jsonString, options);

        //    foreach (var data in jsonData)
        //    {


        //        // Check if the Platform exists
        //        var existingPlatform = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == data.Id);

        //        if (existingPlatform == null)
        //        {
        //            // If the Platform doesn't exist, insert a new one
        //            _context.Platforms.Add(data);
        //        }
        //        else
        //        {
        //            // If the Platform exists, update it
        //            existingPlatform.UniqueName = data.UniqueName;
        //            existingPlatform.Latitude = data.Latitude;
        //            existingPlatform.Longitude = data.Longitude;
        //            existingPlatform.CreatedAt = data.CreatedAt;
        //            existingPlatform.UpdatedAt = data.UpdatedAt;
        //        }

        //        foreach (var well in data.Wells)
        //        {

        //            // Check if the Well exists
        //            var existingWell = await _context.Wells.FirstOrDefaultAsync(w => w.Id == well.Id);

        //            if (well.Id == 0)
        //            {
        //                // If the Well doesn't exist, insert a new one
        //                _context.Wells.Add(well);
        //            }
        //            else
        //            {
        //                // If the Well exists, update it
        //                existingWell.UniqueName = well.UniqueName;
        //                existingWell.Latitude = well.Latitude;
        //                existingWell.Longitude = well.Longitude;
        //                existingWell.CreatedAt = well.CreatedAt;
        //                existingWell.UpdatedAt = well.UpdatedAt;
        //            }
        //        }
        //    }
        //    //// Save changes to the database
        //    await _context.SaveChangesAsync();



        //    return Ok(await _context.Platforms.ToListAsync());
        //}

        //catch (Exception ex)
        //{
        //    // Log the exception for debugging purposes
        //    // You can use a logging framework like Serilog, NLog, or the built-in ILogger
        //    // logger.LogError(ex, "An error occurred while processing the data.");

        //    return BadRequest(ex);
        //}
        //}
        //[HttpGet("GetPlatformWellDummy")]
        //public async Task<IActionResult> GetPlatformWellDummy()
        //{
        //    //try
        //    {
        //        //string sqlScript = ReadSqlScriptFromFileOrResource();
        //           //_context.Database.ExecuteSqlRaw(sqlScript);

        //        string jsonFilePath = "./Json/wellDummy.json";
        //        string jsonString = System.IO.File.ReadAllText(jsonFilePath);
        //        var options = new JsonSerializerOptions
        //        {
        //            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        //            PropertyNameCaseInsensitive = true,
        //        };
        //        var jsonData = System.Text.Json.JsonSerializer.Deserialize<List<PlatformDummy>>(jsonString, options);
        //        foreach (var data in jsonData)
        //        {


        //            // Check if the Platform exists
        //            var existingPlatform = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == data.Id);
        //            if (existingPlatform == null)
        //            {
        //                    var platform = new Platform
        //                {
        //                    UniqueName = data.UniqueName,
        //                    Latitude = data.Latitude,
        //                    Longitude = data.Longitude,
        //                    //CreatedAt = DateTime.Now, // You can set this as needed
        //                    //UpdatedAt = DateTime.Now, // You can set this as needed
        //                    Wells = data.Wells.Select(w => new Well
        //                    {
        //                        UniqueName = w.UniqueName,
        //                        Latitude = w.Latitude,
        //                        Longitude = w.Longitude,
        //                        // Map properties from WellDummy to Well entity
        //                    }).ToList()
        //                };

        //                _context.Platforms.Add(platform);

        //            }
        //            else
        //            {
        //                // If the Platform exists, update it
        //                existingPlatform.UniqueName = data.UniqueName;
        //                existingPlatform.Latitude = data.Latitude;
        //                existingPlatform.Longitude = data.Longitude;
        //                //existingPlatform.CreatedAt = data.CreatedAt;
        //                //existingPlatform.UpdatedAt = data.UpdatedAt;
        //                existingPlatform.Wells = data.Wells.Select(w => new Well
        //                {
        //                    Id= w.Id,
        //                    UniqueName = w.UniqueName,
        //                    Latitude = w.Latitude,
        //                    Longitude = w.Longitude,
        //                    // Map properties from WellDummy to Well entity
        //                }).ToList();
        //            }


        //        }
        //        //// Save changes to the database
        //        await _context.SaveChangesAsync();



        //        return Ok(jsonData);
        //    }

        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex);
        //    }
        //}
        private string ReadSqlScriptFromFileOrResource()
        {
            return System.IO.File.ReadAllText("./Sql/truncateTable.sql");
        }
        private string ReadSqlScriptIdentityOff()
        {
            return System.IO.File.ReadAllText("./Sql/identityOFF.sql");
        }


    }
}
