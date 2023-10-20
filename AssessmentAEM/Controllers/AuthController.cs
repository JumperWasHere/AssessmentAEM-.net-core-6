using AssessmentAEM.Data;
using AssessmentAEM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AssessmentAEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly PlatfromDbContext _context;
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthController(PlatfromDbContext context,IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            this.configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }


        [HttpPost("login-api")]
        public async Task<IActionResult> loginApi(User user)
        {
            var client = _httpClientFactory.CreateClient();
            var loginData = new
            {
                Username = user.Username,
                Password = user.Password
            };
            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://test-demo.aemenersol.com/api/Account/Login", content);
            //var responseContent = await response.Content.ReadAsStringAsync();
            //Console.WriteLine("Response Content: " + responseContent);
            //return Ok(responseContent);
            if (response.IsSuccessStatusCode)
            {
                //var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(
                //    await response.Content.ReadAsStringAsync(),
                //    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                //);
                var token =  await response.Content.ReadAsStringAsync();
                // Save the token for subsequent requests
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return Ok(token);
            }
            else
            {
                return Unauthorized("Invalid username or password");
            }

            //var apiResponse = await client.GetAsync("http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual");
            //if (apiResponse.IsSuccessStatusCode)
            //{
            //    using var contentStream =
            //        await apiResponse.Content.ReadAsStreamAsync();

            //    GitHubBranches = await JsonSerializer.DeserializeAsync
            //        <IEnumerable<GitHubBranch>>(contentStream);
            //    return Ok(contentStream);
            //}
            //return Ok(apiResponse.IsSuccessStatusCode);

        }
        


            [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Login(User user)
        {
            

            if (validateUser(user) == false)
            {
                return BadRequest("Invalid credential");
            }
            string token = CreateToken(user);
            return Ok(token);



        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        private bool validateUser(User user)
        {
            if(user.Username != "user@aemenersol.com") return false;
            if(user.Password != "Test@123") return false;

            return true;

        }
    }
}
