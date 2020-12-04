using PictlData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public interface IAlbumsService
    {
        Task<IEnumerable<Photo>> GetPicturesByAlbumNameAsync(string albumName);
        Task<bool> CreateAsync(string albumName, int userId);
        Task<Album> GetAlbumAsync(int id);
    }
}
