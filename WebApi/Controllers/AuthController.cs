using DatabaseLibrary.Contexts;
using DatabaseLibrary.DTOs;
using DatabaseLibrary.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DatabaseLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService = new(new CinemaUserDbContext());

        // POST api/<UsersController>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<bool> RegisterUser([FromBody] LoginRequest request)
        {
            return await _authService.RegistrateUserAsync(request);
        }

        // POST api/<UsersController>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse?>> LoginUser([FromBody] LoginRequest request)
        {
            var token = await _authService.AuthUserAsync(request);
            if (token is null)
                return Forbid();
            return token;
        }
    }
}
