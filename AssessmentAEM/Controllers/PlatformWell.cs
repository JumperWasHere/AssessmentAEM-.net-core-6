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
        public async Task<IActionResult> getTruncate()
        {
            string sqlScript = ReadSqlScriptFromFileOrResource();
            _context.Database.ExecuteSqlRaw(sqlScript);
            return Ok("Done truncate table");
            

        }

       

        [HttpPost("Async-Data")]
        public async Task<IActionResult> getAsync([FromForm] EndpointRequest endpointRequest)
        {

            try
            {
                if (endpointRequest == null || string.IsNullOrEmpty(endpointRequest.Endpoint))
                {
                    // The endpointRequest is null, handle the validation here
                    return BadRequest("Please Insert End Point to be tested");
                }
                var endPoint = endpointRequest.Endpoint;
                //return Ok(endpointRequest.Endpoint);
                var token = await _authenticationService.GetAuthTokenAsync();

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

                var httpResponseMessage = await client.GetAsync(endPoint);

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
                    return Ok("SUCCESS ASYNC TO DB");


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
            catch (InvalidOperationException ex)
            {
                // Handle request exceptions, e.g., network issues
                return BadRequest("You Must Enter Valid Endpoint");
            }


        }

        private string ReadSqlScriptFromFileOrResource()
        {
            return System.IO.File.ReadAllText("./Sql/truncateTable.sql");
        }
    


    }
}
