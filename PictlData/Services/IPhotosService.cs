using PictlData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public interface IPhotosService
    {
        Task<Photo> GetPhotoByIdAsync(int id);
        Task<IEnumerable<Photo>> GetPhotosAsync();
        Task<bool> DeletePhotoAsync(int id);
        Task<bool> UploadPhotoAsync(int userId, string url, string categoryName);
        Task<bool> AddToAlbumAsync(int photoId, int albumId);
        Task<IEnumerable<Photo>> GetUserPhotosAsync(int userId);
        Task<int> IncrementLikesAsync(int photoId);
        Task<int> ReduceLikesAsync(int photoId);
        Task<IEnumerable<Photo>> GetPhotosByCategoryNameAsync(string name);
        Task<IEnumerable<Photo>> GetOrderedByDatePhotosAsync();
    }
}
