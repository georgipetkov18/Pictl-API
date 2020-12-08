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
    [Route("[controller]/[action]/{id?}")]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotosService photosService;

        public PhotosController(IPhotosService photosService)
        {
            this.photosService = photosService;
        }

        [Authorize]
        public async Task<IActionResult> Delete([FromBody] string rawInfo)
        {
            var parameters = rawInfo.GetParameters("id");
            var status = await photosService.DeletePhotoAsync(int.Parse(parameters["id"]));
            if (status) return this.Ok();

            return this.BadRequest();
        }

        [Authorize]
        public async Task<IActionResult> Upload([FromBody] string rawInfo)
        {
            var parameters = rawInfo.GetParameters("data", "categoryName");
            var token = this.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            var userID = int.Parse(handler.ReadJwtToken(token).Claims.First().Value);
            //var decodedData = parameters["data"].Replace("-", "");
            //var c = decodedData.Replace('_', '/');


            var b = HttpUtility.UrlDecode(parameters["data"], Encoding.ASCII);
            var status = await this.photosService.UploadPhotoAsync(userID, Convert.FromBase64String(b), parameters["categoryName"]);
            if (status) return this.Ok();

            return this.BadRequest();
        }

        public async Task<IActionResult> Display(int id)
        {
            var photo = await this.photosService.GetPhotoByIdAsync(id);
            return this.File(photo.Data, "image/png");
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
