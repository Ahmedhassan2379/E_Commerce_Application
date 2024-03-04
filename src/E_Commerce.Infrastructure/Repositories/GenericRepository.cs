using E_Commerce.Core.Entities;
using E_Commerce.Core.Interfaces;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commerce.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity<int>
    {
        private readonly ApplicationDbContext _Context;
        public GenericRepository(ApplicationDbContext context)
        {
                _Context = context;
        }
        public async Task AddAsync(T entity)
        {
            await _Context.AddAsync(entity);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
             _Context.Set<T>().Remove(entity);
            await _Context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll()
        { 
            return _Context.Set<T>().AsNoTracking().ToList();
        }



        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _Context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _Context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _Context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _Context.Set<T>().Where(x=>x.Id==id);
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var Existedentity = await _Context.Set<T>().FindAsync(id);
            if (Existedentity != null)
            {
                _Context.Set<T>().Update(entity);
                await _Context.SaveChangesAsync();
            }
        }
    }
}
