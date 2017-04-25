using AspModular.Data.Models;
using AspModular.Data.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AspModular.Data.Abstractions
{
    public interface IRepositoryWithTypedId<T, in TId> where T : class, IEntityWithTypedId<TId>
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        int Count();
        T GetSingle(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> Query();
        void Add(T entity);
        void Delete(T entity);
        void DeleteBy(Expression<Func<T, bool>> predicate);
        void Edit(T entity);
        void Commit();
    }
}
