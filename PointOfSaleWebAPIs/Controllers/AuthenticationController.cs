using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointOfSale;
using PointOfSale.Services;
using PointOfSale.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
namespace PointOfSaleWebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

      
        private readonly POSDbContext context;
        private readonly TokenService _tokenService;
        public AuthenticationController(POSDbContext context, TokenService tokenService, ILogger<AuthenticationController> logger)
        {
            this.context = context;
            this._tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("token")]
        public IActionResult GetToken([FromBody] string username, string password)
        {
           
            var token = _tokenService.GenerateToken(username);
            return Ok(new { token });
          
        }
       

        [HttpPost("SeedUsers")]
        public IActionResult SeedUsers()
        {
            try
            {
                EFUserData.SeedData(context);
             
                _logger.LogInformation("User added successfully!");
                return Ok("Users seeded");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while seeding users : { ex.Message}");
                return BadRequest();
            }
           
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                bool loggedIn = Authentication.LoginAPI(context, username, password);
                if (loggedIn)
                {
                    _logger.LogInformation("User logged in!");
                    return Ok(GetToken(username, password));
                }
                else
                {
                    _logger.LogError("Incorrect Password!");
                    return BadRequest("Incorrect password");
                }
              
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while logging in : {ex.Message}");
                return BadRequest($"Error while logging in : {ex.Message}");
            }
        }

        [HttpPost]
        [Route ("Register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.name) || string.Equals(user.name, "string"))
                {
                    return BadRequest("Invalid Name");
                }


                if (string.IsNullOrEmpty(user.email) || string.Equals(user.email, "string") || !EmailValidation.IsValidEmail(user.email))
                {
                    return BadRequest("Invalid Email");
                }

                if (string.Equals(user.role, "string"))
                {
                    return BadRequest("Invalid Role");
                }



                Regex vaildate_password = Password.ValidatePassword();
                if (string.IsNullOrEmpty(user.password) || vaildate_password.IsMatch(user.password) != true)
                {
                    return BadRequest("Invalid Password(must contain numbers, at least one capital alphabet");
                }

                if(Authentication.RegisterAPI(context, user))
                {
                    _logger.LogInformation("User added successfully!");
                    return Ok(user);
                }
                else
                {
                    _logger.LogWarning("Invalid data");
                    return BadRequest("User already exists!");
                }
              
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while registeration : {ex.Message}");
                return BadRequest();
            }
        }
    }
}
