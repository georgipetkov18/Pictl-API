using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PictlData.Repositories
{
    public class Repository : IRepository
    {
        public ApplicationDbContext Db { get; set; }

        public Repository(ApplicationDbContext db)
        {
            this.Db = db;
        }

        public async Task SaveDbChangesAsync()
        {
            await this.Db.SaveChangesAsync();
        }

<<<<<<< HEAD
        public void MigrateDatabase()
=======
        public void Migrate()
>>>>>>> e210063112e1be6394cf3f38c00e63b80e52d6b5
        {
            this.Db.Database.Migrate();
        }

        public async Task AddAsync<T>(T entity)
        {
            await this.Db.AddAsync(entity);
        }
    }
}
