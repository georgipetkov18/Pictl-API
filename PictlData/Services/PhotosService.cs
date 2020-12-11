using Microsoft.EntityFrameworkCore;
using PictlData.Models;
using PictlData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public class PhotosService : IPhotosService
    {
        private readonly IRepository repo;
        private readonly IUserService userService;
        private readonly ICategoriesService categoriesService;
        private readonly IAlbumsService albumsService;

        public PhotosService(IRepository repo, IUserService userService, ICategoriesService categoriesService, IAlbumsService albumsService)
        {
            this.repo = repo;
            this.userService = userService;
            this.categoriesService = categoriesService;
            this.albumsService = albumsService;
        }

        public async Task<bool> DeletePhotoAsync(int id)
        {
            try
            {
                var photo = await repo.Db.Photos.SingleOrDefaultAsync(x => x.ID == id && !x.IsDeleted);
                photo.IsDeleted = true;
                await repo.SaveDbChangesAsync();
                return true;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            
        }

        public async Task<Photo> GetPhotoByIdAsync(int id)
        {
            return await this.repo.Db.Photos.SingleOrDefaultAsync(x => x.ID == id && !x.IsDeleted)
                ?? throw new ArgumentNullException("Photo does not exist!");
        }

        public async Task<IEnumerable<Photo>> GetPhotosAsync()
        {
            return await this.repo.Db.Photos.Where(x => !x.IsDeleted).ToListAsync()
                ?? throw new ArgumentNullException("There are no photos!");
        }

        //public async Task<IEnumerable<Photo>> GetPhotosByCategoryNameAsync(string name)
        //{
        //    return await this.repo.Db.Photos
        //        .Where(p => !p.IsDeleted && p.Categories
        //        .Any(c => c.Name == name))
        //        .ToListAsync()
        //        ?? throw new ArgumentNullException("There are no photos!");
        //}

        public async Task<bool> UploadPhotoAsync(int userId, string url, string categoryName)
        {
            try
            {
                var user = await userService.GetUserAsync(userId);
                var photo = new Photo()
                {
                    Url = url,
                    UserId = user.ID,
                    CreatedAt = DateTime.Now,
                    Likes = 0,
                    IsDeleted = false
                };

                var category = await this.categoriesService.GetCategoryAsync(categoryName);
                await this.repo.Db.Photos.AddAsync(photo);
                await this.repo.Db.CategoryPhotos.AddAsync(new CategoryPhoto()
                {
                    CategoryId = category.ID,
                    Photo = photo
                });
                await this.repo.SaveDbChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddToAlbumAsync(int photoId, int albumId)
        {
            try
            {
                var album = await this.albumsService.GetAlbumAsync(albumId);
                var photo = await this.GetPhotoByIdAsync(photoId);
                album.Photos.Add(photo);
                await this.repo.SaveDbChangesAsync();
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }
    }
}
