using Microsoft.AspNetCore.Mvc;
using PictlData.Services;
using PictlHelpers;
using System.Threading.Tasks;

namespace PictlAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesService categoriesService;

        public CategoryController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> Create([FromBody] string rawInfo)
        {
            var parameters = rawInfo.GetParameters("name");
            var status = await this.categoriesService.CreateCategoryAsync(parameters["name"]);
            if (status) return this.Created("/category/create", parameters["name"]);

            return this.BadRequest();
        }
    }
}
