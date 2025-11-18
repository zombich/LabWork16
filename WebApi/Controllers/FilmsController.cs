using DatabaseLibrary.Contexts;
using DatabaseLibrary.Models;
using DatabaseLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly FilmService _filmService = new(new CinemaUserDbContext());
        //[AllowAnonymous]
        //[HttpGet]
        //public async Task<IEnumerable<Film>> GetFilmsByPages(int? page = 1)
        //{
        //    return await _filmService.GetFilmsByPages(page);
        //}
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Film>> GetFilms()
        {
            return await _filmService.GetFilms();
        }
    }
}
