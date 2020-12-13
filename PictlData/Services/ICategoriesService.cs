using PictlData.Models;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public interface ICategoriesService
    {
        Task<bool> CreateCategoryAsync(string name);
        Task<Category> GetCategoryAsync(string name);
        Task<Category> GetCategoryAsync(int id);
        Task<bool> CategoryExistsAsync(string name);
    }
}
