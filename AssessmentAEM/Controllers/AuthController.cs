using AssessmentAEM.Data;
using AssessmentAEM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AssessmentAEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly PlatfromDbContext _context;
        private readonly IConfiguration configuration;

        public AuthController(PlatfromDbContext context,IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Login(User user)
        {
            //if (user.Username != request.Username)
            //{

            //    return BadRequest("user not exist");
            //}
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
    }
}
