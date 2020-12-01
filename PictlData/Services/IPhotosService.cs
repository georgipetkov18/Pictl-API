﻿using PictlData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public interface IPhotosService
    {
        Task<Photo> GetPhotoByIdAsync(int id);
        Task<IEnumerable<Photo>> GetPhotosByCategoryNameAsync(string name);
        Task<IEnumerable<Photo>> GetPhotosAsync();
    }
}