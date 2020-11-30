using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PictlData.Models;
using PictlData.Repositories;
using PictlHelpers.Exceptions;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PictlData.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository repo;
        private readonly AppSettings appSettings;

        public UserService(IRepository repo, IOptions<AppSettings> appSettings)
        {
            this.repo = repo;
            this.appSettings = appSettings.Value;
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

        public async Task<AuthenticateResponse> LogInAsync(string email, string password)
        {
            var user = await repo.Db.Users.FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted) 
                ?? throw new NullReferenceException("User does not exist!");

            var encryptedPassword = EncryptPassword(password);

            if (encryptedPassword != user.Password) throw new WrongPasswordException();

            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
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

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.ID.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
