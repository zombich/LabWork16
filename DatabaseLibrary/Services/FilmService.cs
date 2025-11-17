using DatabaseLibrary.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Services
{
    public class FilmService(CinemaUserDbContext context)
    {
        private readonly CinemaUserDbContext _context = context;
        public static async Task<IEnumerable<Film>> GetFilmsByPages(int? page)
        {
            var films = _context.Films.AsQueryable();
            var pageSize = 2;
            films = films.Skip(pageSize * ((int)page - 1)).Take(pageSize);
            return await films.ToListAsync();
        }
        public static async Task<IEnumerable<Film>> GetFilms() 
            => await _context.Films.ToListAsync();
    }
}
