using Arch.Data.Context;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
namespace Arch.Data.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ArchContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(ArchContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public TEntity Find(int id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public virtual DbRawSqlQuery Execute(Type a, string sql, params object[] Parameter)
        {
            return _context.Database.SqlQuery(a, sql, Parameter);
        }
    }
}