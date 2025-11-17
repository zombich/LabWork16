using DatabaseLibrary.Contexts;
using DatabaseLibrary.Models;
using DatabaseLibrary.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly AuthService _authService = new(new CinemaUserDbContext());
        // GET: api/<ProfileController>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<CinemaUser>> GetUserData()
        {
            var claim = User.Claims.FirstOrDefault(u => u.Type == "login");
            return await _authService.FindUserByLoginAsync(claim.Value);
        }
    }
}
