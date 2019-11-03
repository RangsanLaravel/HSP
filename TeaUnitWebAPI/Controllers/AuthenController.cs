using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TeaUnitWebAPI.Models;
using CryptoHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TeaUnitWebAPI.BussinessLogin;

namespace TeaUnitWebAPI.Controller
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class AuthenController : ControllerBase
    {

        ILogger<AuthenController> _logger;
        public IConfiguration Configuration { get; }

        public AuthenController(ILogger<AuthenController> logger, IConfiguration Configuration)
        {
            _logger = logger;
            this.Configuration = Configuration;
        }

        //FromForm = data
        //FromBody = json
        [HttpPost("register")]
        public IActionResult Register([FromBody] Users model)
        {
            Users result = new Users();
            try
            {
                ServiceAction action = new ServiceAction(Configuration);
                action.CheckUser(out result, model.Username);
                if (result != null)
                    return BadRequest();
                model.Password = Crypto.HashPassword(model.Password);
                action.Register(model);
                return Ok(new { result = "ok", message = "register successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to execute POST");
                return StatusCode(500, new { result = "", message = ex.Message });
                
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Users model)
        {
            string token = string.Empty;
            Users result = new Users();
            try
            {
                ServiceAction action = new ServiceAction(Configuration);
                action.CheckUser(out result, model.Username);
                if (result == null)
                    return NotFound();
                if (!Crypto.VerifyHashedPassword(result.Password, model.Password))
                    return Unauthorized();

                //set jwt 
                token = action.BuildToken(result);
                return Ok(new { token = token, message = "login successfully" });

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to execute POST");
                return StatusCode(500, new { result = "", message = ex.Message });
            }
        }


    }
}