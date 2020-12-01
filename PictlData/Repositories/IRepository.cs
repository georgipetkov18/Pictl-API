using System.Threading.Tasks;

namespace PictlData.Repositories
{
    public interface IRepository
    {
        public ApplicationDbContext Db { get; }
        void MigrateDatabase();
        Task SaveDbChangesAsync();
        Task AddAsync<T>(T entity);
    }
}
