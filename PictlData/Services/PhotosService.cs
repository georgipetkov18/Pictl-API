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

        public async Task<bool> UploadPhotoAsync(int userId, string url, string categoryName)
        {
            try
            {
                var status = await this.categoriesService.CreateCategoryAsync(categoryName);
                if (!status) return false;

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

                var album = await this.albumsService.CreateAlbumAsync($"{user.FirstName} {user.LastName}", userId);
                album.Photos.Add(photo);

                await this.repo.SaveDbChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                var a = e.Message;
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

        public async Task<IEnumerable<Photo>> GetUserPhotosAsync(int userId)
        {
            return await this.repo.Db.Photos
                .Where(p => p.UserId == userId & !p.IsDeleted)
                .ToListAsync() ?? throw new ArgumentNullException("Current user has no photos.");

        }

        public async Task<int> IncrementLikesAsync(int photoId)
        {
            var photo = await this.GetPhotoByIdAsync(photoId);
            photo.Likes++;
            await this.repo.SaveDbChangesAsync();
            return photo.Likes;
        }

        public async Task<int> ReduceLikesAsync(int photoId)
        {
            var photo = await this.GetPhotoByIdAsync(photoId);
            if (photo.Likes > 0) photo.Likes--;
            await this.repo.SaveDbChangesAsync();
            return photo.Likes;
        }

        public async Task<IEnumerable<Photo>> GetPhotosByCategoryNameAsync(string name)
        {
            var category = await this.categoriesService.GetCategoryAsync(name);

            var categoryPhotos = await this.repo.Db.CategoryPhotos.Where(p => p.CategoryId == category.ID).ToListAsync();

            var photos = new List<Photo>();

            foreach (var categoryPhoto in categoryPhotos)
            {
                var photo = await this.GetPhotoByIdAsync(categoryPhoto.PhotoId);
                photos.Add(photo);
            }

            return photos;
        }

        public async Task<IEnumerable<Photo>> GetOrderedByDatePhotosAsync()
        {
            var photos = await this.repo.Db.Photos
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync() ?? throw new ArgumentNullException("There are no photos!");

            foreach (var photo in photos)
            {
                var user = await this.userService.GetUserAsync(photo.UserId);
                var categoryPhoto1 = await this.repo.Db.CategoryPhotos.FirstOrDefaultAsync(x => x.PhotoId == photo.ID);
                var categoryPhotos = await this.repo.Db.CategoryPhotos.Where(x => x.PhotoId == photo.ID).ToListAsync();
                photo.User = user;

                foreach (var categoryPhoto in categoryPhotos)
                {
                    var category = await this.categoriesService.GetCategoryAsync(categoryPhoto.CategoryId);
                    photo.CategoriesNames.Add(category.Name);
                }
            }

            return photos;
        }
    }
}
