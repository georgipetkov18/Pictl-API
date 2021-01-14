using Microsoft.AspNetCore.Mvc;
using PictlData.Services;
using PictlHelpers;
using System;
using System.Threading.Tasks;

namespace PictlAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]/{id?}")]
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

        public async Task<IActionResult> GetAlbum(int id)
        {
            try
            {
                var photos = await this.albumsService.GetPhotosAsync(id);
                var album = await this.albumsService.GetAlbumAsync(id);

                var result = new JsonResult(new { albumName = album.Name, photos = photos });
                return this.Ok(result.Value);
            }

            catch (ArgumentNullException e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}
