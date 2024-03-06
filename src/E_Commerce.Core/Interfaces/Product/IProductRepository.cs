using E_Commerce.Core.Dtos;
using E_Commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> AddAsync(CreateProductDto entity);
        Task<bool> UpdateAsync(int id,UpdateProductDto entity);
        Task<bool> DeleteWithPicAsync(int id);
    }
}
