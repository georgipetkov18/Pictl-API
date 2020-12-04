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

        public void MigrateDatabase()
        {
            this.Db.Database.Migrate();
        }

        public async Task AddAsync<T>(T entity)
        {
            await this.Db.AddAsync(entity);
            await this.SaveDbChangesAsync();
        }
    }
}
