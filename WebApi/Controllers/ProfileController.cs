using DatabaseLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        // GET: api/<ProfileController>
        [Authorize]
        [HttpGet]
        public ActionResult<CinemaUser> GetUserData()
        {


            return null;
        }
    }
}
