using AspModular.Data.Abstractions;
using AspModular.Data.Models;
using AspModular.Data.Models.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AspModular.Data
{
    public class RepositoryWithTypedId<T, TId> : IRepositoryWithTypedId<T, TId>
       where T : class, IEntityWithTypedId<TId>
    {
        #region Properties
        private StorageContext _context;
        protected DbSet<T> DbSet { get; }

        public RepositoryWithTypedId(StorageContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }
        #endregion
        public void Add(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public void DeleteBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = DbSet.Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        public void Edit(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DbSet.AsEnumerable();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).FirstOrDefault();
        }

        public IQueryable<T> Query()
        {
            return DbSet;
        }

        public virtual void Commit()
        {
            _context.SaveChanges();
        }

        public int Count()
        {
            return DbSet.Count();
        }
    }
}
