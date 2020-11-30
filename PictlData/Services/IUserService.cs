using PictlData.Models;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(int id);

        Task<User> GetUserAsync(string name);

        Task<User> GetUserAsync(string email, string password);

        Task<bool> RegisterUser(User user);
    }
}
