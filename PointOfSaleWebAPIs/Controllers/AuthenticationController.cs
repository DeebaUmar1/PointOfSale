using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointOfSale;
using PointOfSale.Services;
using PointOfSale.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
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
                var searchResults = context.Users.FirstOrDefault(user => user.name == username);
                if (searchResults == null)
                {
                    _logger.LogError("User does not exists");
                    return BadRequest("User does not exists!");
                }
                else
                { 
                    if (password == searchResults.password)
                    {
                        _logger.LogInformation("User logged in!");
                        return Ok(GetToken(username, password));
                    }
                    else
                    {
                        _logger.LogError("Incorrect password");
                        return BadRequest("Incorrect password");
                    }
                } 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while logging in : {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route ("Register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                _logger.LogInformation("User added successfully!");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while registeration : {ex.Message}");
                return BadRequest();
            }
        }
    }
}
