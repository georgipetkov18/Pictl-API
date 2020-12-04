using Microsoft.AspNetCore.Mvc;

namespace PictlAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        public IActionResult Home()
        {
            return this.Ok("WELCOME TO PICTL API");
        }
    }
}
