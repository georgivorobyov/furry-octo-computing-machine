using System.Linq;

namespace Sumc.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();

        T Get(int entityId);

        T Add(T entity);

        void Delete(T entity);

        void Delete(int entityId);

        void Update(T entity);

        void SaveChanges();
    }
}
