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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string rawInfo)
        {
            var parameters = rawInfo.GetParameters("categoryName");
            var status = await this.categoriesService.CreateCategoryAsync(parameters["categoryName"]);
            if (status) return this.Created("/category/create", parameters["categoryName"]);

            return this.BadRequest();
        }
    }
}
