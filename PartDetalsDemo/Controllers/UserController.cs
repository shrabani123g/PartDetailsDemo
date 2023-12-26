using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PartDetailsDemo.CQRS.Queries;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace PartDetailsDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IMediator mediator;
        private IConfiguration configuration;
        private readonly ILogger<UserController> seriLogger;
        public UserController(IMediator mediator, IConfiguration configuration, ILogger<UserController> seriLogger)
        {
            this.mediator = mediator;
            this.configuration = configuration;
            this.seriLogger = seriLogger;
        }

        [HttpGet]
        public async Task<IActionResult> UserLogin(GetUserDetailsQuery user)
        {

            seriLogger.Log(LogLevel.Information, "Begning of User login.");
            try
            {
                var userDetail = await mediator.Send(user);

                if (userDetail != null)
                {
                    var token = Generate(userDetail);
                    return Ok(token);
                }
                return NotFound("User not found.");
            }
            catch
            {
                return StatusCode(500, "Time Stamp: " + DateTime.Now.ToString("G") + " An error occured, please contact admin.");
            }
        }

        private string Generate(Models.User userDetail)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(ClaimTypes.Role, userDetail.Role)
            };

                var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                  configuration["Jwt:Audience"],
                  claims,
                  expires: DateTime.Now.AddMinutes(15),
                  signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch(Exception ex)
            {
                seriLogger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }
    }
}
