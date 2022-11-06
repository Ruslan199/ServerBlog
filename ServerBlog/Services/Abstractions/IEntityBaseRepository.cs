using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServerBlog.Services.Abstractions
{
    public interface IEntityBaseRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        Task<int> Commit();
        IEnumerable<T> GetAll();
        ValueTask<T> GetAsync(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Update(T entity);
    }
}
