using Microsoft.EntityFrameworkCore;
using PictlCommon.Exceptions;
using PictlData.Models;
using PictlData.Repositories;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository repo;

        public UserService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await repo.Db.Users.FirstOrDefaultAsync(x => x.ID == id && !x.IsDeleted) 
                ?? throw new NullReferenceException("User does not exist!");
        }

        public async Task<User> GetUserAsync(string name)
        {
            return await repo.Db.Users.FirstOrDefaultAsync(x => x.FirstName == name && !x.IsDeleted)
                ?? throw new NullReferenceException("User does not exist!");
        }

        public async Task<User> GetUserAsync(string email, string password)
        {
            var user = await repo.Db.Users.FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted) 
                ?? throw new NullReferenceException("User does not exist!");

            var encryptedPassword = EncryptPassword(password);

            if (encryptedPassword != user.Password) throw new WrongPasswordException();

            return user;
        }

        public async Task<bool> RegisterUser(User user)
        {
            try
            {
                user.Password = EncryptPassword(user.Password);
                await repo.AddAsync(user);
                await repo.SaveDbChangesAsync();
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        private string EncryptPassword(string password)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
