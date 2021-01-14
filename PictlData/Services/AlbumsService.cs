using Microsoft.EntityFrameworkCore;
using PictlData.Models;
using PictlData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public class AlbumsService : IAlbumsService
    {
        private readonly IRepository repo;
        private readonly IUserService userService;

        public AlbumsService(IRepository repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
        }

        public async Task<bool> CreateAsync(string albumName, int userId)
        {
            try
            {
                var user = await userService.GetUserAsync(userId);
                var album = new Album()
                {
                    Name = albumName,
                    User = user,
                    IsDeleted = false
                };
                await this.repo.AddAsync(album);
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Album> CreateAlbumAsync(string albumName, int userId)
        {
            if (await this.repo.Db.Albums.AnyAsync(a => a.Name == albumName && a.UserId == userId && !a.IsDeleted))
            {
                return await this.GetAlbumAsync(albumName);
            }
            var user = await userService.GetUserAsync(userId);
            var album = new Album()
            {
                Name = albumName,
                UserId = user.ID,
                IsDeleted = false
            };
            await this.repo.AddAsync(album);
            return album;
        }

        public async Task<Album> GetAlbumAsync(int id)
        {
            return await this.repo.Db.Albums.SingleOrDefaultAsync(x => x.ID == id && !x.IsDeleted)
                ?? throw new ArgumentNullException("Album does not exist!");
        }

        public async Task<IEnumerable<Photo>> GetPicturesByAlbumNameAsync(string albumName)
        {
            var album = await this.repo.Db.Albums
                .SingleOrDefaultAsync(a => a.Name == albumName && !a.IsDeleted);

            return await album.Photos.Where(p => !p.IsDeleted).AsQueryable().ToListAsync();
        }

        public async Task<Album> GetAlbumAsync(string name)
        {
            return await this.repo.Db.Albums.SingleOrDefaultAsync(x => x.Name == name && !x.IsDeleted)
                ?? throw new ArgumentNullException("Album does not exist!");
        }

        public async Task<IEnumerable<Photo>> GetPhotosAsync(int albumId)
        {
            if (await this.repo.Db.Albums.AnyAsync(a => a.ID != albumId || a.IsDeleted)) 
                throw new ArgumentNullException($"Album with id {albumId} does not exist!");

            return await this.repo.Db.Photos.Where(p => p.AlbumId == albumId && !p.IsDeleted).ToListAsync();
        }
    }
}
