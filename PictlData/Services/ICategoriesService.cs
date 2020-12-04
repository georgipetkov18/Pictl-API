using PictlData.Models;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public interface ICategoriesService
    {
        Task<bool> CreateCategoryAsync(string name);
        Task<Category> GetCategoryAsync(string name);
    }
}
