using DatabaseLibrary.Contexts;
using DatabaseLibrary.Models;
using DatabaseLibrary.Service;
using DatabaseLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly AuthService _authService = new(new CinemaUserDbContext());
        private readonly TicketService _ticketService = new(new CinemaUserDbContext());
        // GET api/<TicketsController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTickets(int id)
        {
            var claim = User.Claims.FirstOrDefault(u => u.Type == "role");
            var privilegeName = "";

            if (await _authService.IsPrivilegeExists(privilegeName, claim.Value))
                return await _ticketService.GetTicketByIdAsync(id);
            else
                return Forbid();
        }
    }
}
