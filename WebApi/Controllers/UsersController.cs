using DatabaseLibrary.Contexts;
using DatabaseLibrary.Models;
using DatabaseLibrary.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthService _service = new(new CinemaUserDbContext());
        // GET: api/<UsersController>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddUser(CinemaUser user)
        {
            var claim = User.Claims.FirstOrDefault(u => u.Type == "role");
            var privilegeName = "";

            if (await _service.IsPrivilegeExists(privilegeName, claim.Value))
            {
                try
                {
                    await _service.AddUserAsync(user);
                    return Ok();
                }
                catch
                {
                    return BadRequest();
                }
            }
            else
                return Forbid();
        }

    }
}
