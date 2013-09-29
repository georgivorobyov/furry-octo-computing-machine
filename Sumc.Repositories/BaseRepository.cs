using Sumc.Data;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Sumc.Repositories
{
    public class BaseRepository<T> : IRepository<T>, IDisposable where T : class
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<T> dbSet;

        public BaseRepository()
        {
            this.dbContext = new SumcContext();
            this.dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> All()
        {
            var queryable = this.dbSet.AsQueryable<T>();
            return queryable;
        }

        public T Get(int entityId)
        {
            T entity = this.dbSet.Find(entityId);
            return entity;
        }

        public T Add(T entity)
        {
            var dbEntityEntry = this.dbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                this.dbSet.Add(entity);
            }
            else
            {
                dbEntityEntry.State = EntityState.Added;
            }

            return entity;
        }

        public void Delete(T entity)
        {
            this.dbSet.Remove(entity);
        }

        public void Delete(int entityId)
        {
            var entity = this.Get(entityId);
            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public void Update(T entity)
        {
            var dbEntityEntry = this.dbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }

            dbEntityEntry.State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        ~BaseRepository()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }

        public void Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }
    }
}
