using Microsoft.EntityFrameworkCore;
using PictlData.Models;
using PictlData.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public class PhotosService : IPhotosService
    {
        private readonly IRepository repo;

        public PhotosService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<Photo> GetPhotoByIdAsync(int id)
        {
            return await this.repo.Db.Photos.FirstOrDefaultAsync(x => x.ID == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<Photo>> GetPhotosAsync()
        {
            return await this.repo.Db.Photos.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Photo>> GetPhotosByCategoryNameAsync(string name)
        {
            return await this.repo.Db.Photos
                .Where(p => !p.IsDeleted && p.Categories
                .Any(c => c.Name == name))
                .ToListAsync();
        }
    }
}
