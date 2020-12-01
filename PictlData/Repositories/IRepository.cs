using System.Threading.Tasks;

namespace PictlData.Repositories
{
    public interface IRepository
    {
        public ApplicationDbContext Db { get; }
<<<<<<< HEAD
        void MigrateDatabase();
=======
        void Migrate();
>>>>>>> e210063112e1be6394cf3f38c00e63b80e52d6b5
        Task SaveDbChangesAsync();
        Task AddAsync<T>(T entity);
    }
}
