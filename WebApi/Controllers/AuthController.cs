using DatabaseLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DatabaseLibrary.Service;
using DatabaseLibrary.Contexts;
using System.Text;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<TokenResponse?> LoginUser([FromBody] LoginRequest request)
        {
            return await _authService.AuthUserAsync(request);
        }
    }
}
