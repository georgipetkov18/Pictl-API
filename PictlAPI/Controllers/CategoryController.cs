using Microsoft.AspNetCore.Mvc;
using PictlData.Services;
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

        public async Task<IActionResult> Create(string name)
        {
            var status = await this.categoriesService.CreateCategoryAsync(name);
            if (status) return this.Created("/category/create", name);

            return this.BadRequest();
        }
    }
}
