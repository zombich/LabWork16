using DatabaseLibrary.Contexts;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLibrary.Services
{
    public class TicketService(CinemaUserDbContext context)
    {
        private readonly CinemaUserDbContext _context = context;
        public async Task<Ticket?> GetTicketByIdAsync(int id) 
            => await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == id);
    }
}
