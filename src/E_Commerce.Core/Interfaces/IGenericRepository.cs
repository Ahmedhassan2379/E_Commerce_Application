using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Interfaces
{
    public interface IGenericRepository<T> where  T : class
    {
        IEnumerable<T> GetAll();
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(T id,params Expression<Func<T, object>>[] includes);
        Task GetAsyncBy(T id);
        Task AddAsync(T entity);
        Task UpdateAsync(T id,T entity);
        Task DeleteAsync(T id);
    }
}
