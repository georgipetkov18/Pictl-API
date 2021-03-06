﻿using Microsoft.EntityFrameworkCore;
using PictlData.Models;
using PictlData.Repositories;
using System;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository repo;

        public CategoriesService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> CategoryExistsAsync(string name)
        {
            return await this.repo.Db.Categories.AnyAsync(x => x.Name == name);
        }

        public async Task<bool> CreateCategoryAsync(string name)
        {
            try
            {
                if (await this.CategoryExistsAsync(name)) return true;

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

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await repo.Db.Categories.SingleOrDefaultAsync(x => x.ID == id && !x.IsDeleted)
                ?? throw new ArgumentNullException($"Category with id: {id} does not exist!");
        }
    }
}
