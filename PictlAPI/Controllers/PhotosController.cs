using Microsoft.AspNetCore.Mvc;
using PictlData.Attributes;
using PictlData.Services;
using PictlHelpers;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Delete(int id)
        {
            var status = await photosService.DeletePhotoAsync(id);
            if (status) return this.Ok();

            return this.BadRequest();
        }

        [Authorize]
        public async Task<IActionResult> Upload([FromBody] string rawInfo)
        {
            var parameters = rawInfo.GetParameters("url");
            var token = this.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            var userID = int.Parse(handler.ReadJwtToken(token).Claims.First().Value);
            var status = await this.photosService.UploadPhotoAsync(userID, parameters["url"]);
            if (status) return this.Ok();

            return this.BadRequest();
        }

        [Authorize]
        public async Task<IActionResult> AssignToCategory(string categoryName, int photoId)
        {
            var status = await this.photosService.AssignToCategoryAsync(categoryName, photoId);
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
    }
}
