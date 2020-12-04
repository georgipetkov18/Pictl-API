using Microsoft.AspNetCore.Mvc;
using PictlData.Services;
using PictlHelpers;
using System.Threading.Tasks;

namespace PictlAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumsService albumsService;

        public AlbumsController(IAlbumsService albumsService)
        {
            this.albumsService = albumsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string rawInfo)
        {
            var parameters = rawInfo.GetParameters("albumName", "userId");
            var status = await this.albumsService.CreateAsync(parameters["albumName"], int.Parse(parameters["userId"]));
            if (status) return this.Ok();

            return this.BadRequest();
        }
    }
}
