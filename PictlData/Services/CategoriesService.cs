using Microsoft.EntityFrameworkCore;
using PictlData.Models;
using PictlData.Repositories;
using System;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository repo;
        private readonly IPhotosService photosService;

        public CategoriesService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> CreateCategoryAsync(string name)
        {
            try
            {
                await this.repo.AddAsync(new Category()
                {
                    Name = name,
                    IsDeleted = false
                });

                await this.repo.SaveDbChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Category> GetCategoryAsync(string name)
        {
            return await repo.Db.Categories.SingleOrDefaultAsync(x => x.Name == name && !x.IsDeleted)
                ?? throw new ArgumentNullException($"Category with name: {name} does not exist!");
        }
    }
}
