using Microsoft.AspNetCore.Mvc;
using PictlData.Attributes;
using PictlData.Services;
using PictlHelpers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PictlAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotosService photosService;

        public PhotosController(IPhotosService photosService)
        {
            this.photosService = photosService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] string rawInfo)
        {
            var parameters = rawInfo.GetParameters("id");
            var status = await photosService.DeletePhotoAsync(int.Parse(parameters["id"]));
            if (status) return this.Ok();

            return this.BadRequest();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] string rawInfo)
        {
            var parameters = rawInfo.GetParameters("url", "categoryName");
            var token = this.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            var userID = int.Parse(handler.ReadJwtToken(token).Claims.First().Value);
            var status = await this.photosService.UploadPhotoAsync(userID, parameters["url"], parameters["categoryName"]);
            if (status) return this.Ok();

            return this.BadRequest();
        }

        [Authorize]

        public async Task<IActionResult> AddToAlbum([FromBody] string rawInfo)
        {
            var parameters = rawInfo.GetParameters("photoId", "albumId");

            var status = await this.photosService
                .AddToAlbumAsync(int.Parse(parameters["photoId"]), int.Parse(parameters["albumId"]));
            if (status) return this.Ok();

            return this.BadRequest();

        }

        [Authorize]
        public async Task<IActionResult> GetPhotoData([FromBody] string rawInfo)
        {
            try
            {
                var parameters = rawInfo.GetParameters("id");
                var photo = await this.photosService.GetPhotoByIdAsync(int.Parse(parameters["id"]));
                return new JsonResult(new { userId = photo.User.ID, url = photo.Url, createdAt = photo.CreatedAt });
            }

            catch (ArgumentNullException e)
            {
                return this.BadRequest(e.Message);
            }

        }
    }
}
