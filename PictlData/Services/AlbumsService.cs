using Microsoft.EntityFrameworkCore;
using PictlData.Models;
using PictlData.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public class AlbumsService : IAlbumsService
    {
        private readonly IRepository repo;

        public AlbumsService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<Photo>> GetPicturesByAlbumNameAsync(string albumName)
        {
            var album = await this.repo.Db.Albums
                .FirstOrDefaultAsync(a => a.Name == albumName && !a.IsDeleted);

            return await album.Photos.Where(p => !p.IsDeleted).AsQueryable().ToListAsync();
        }
    }
}
