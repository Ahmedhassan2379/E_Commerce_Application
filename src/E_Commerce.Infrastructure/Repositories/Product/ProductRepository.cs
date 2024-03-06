using AutoMapper;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Entities;
using E_Commerce.Core.Interfaces;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace E_Commerce.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _fileProvider = fileProvider;
        }

        public async Task<bool> AddAsync(CreateProductDto entity)
        {
            var src = "";
            if (entity.Image is not null)
            {
                
                var root = "/images/products/";
                var productImage = $"{Guid.NewGuid()}" + entity.Image.FileName;
                if (!Directory.Exists("wwwroot" + root))
                {
                    Directory.CreateDirectory("wwwroot" + root);
                }

                 src = root + productImage;
                var picInfo = _fileProvider.GetFileInfo(src);
                var rootPath = picInfo.PhysicalPath;
                using (var stream = new FileStream(rootPath, FileMode.Create))
                {
                    await entity.Image.CopyToAsync(stream);
                };
                
            }
            var newProduct = _mapper.Map<Product>(entity);
            newProduct.ProductPicture = src;
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(int id,UpdateProductDto entity)
        {
            var ExistedProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if(ExistedProduct is not null)
            {
                var src = "";
                if (entity.Image is not null)
                {
                    var root = "/images/products/";
                    var productImage = $"{Guid.NewGuid()}" + entity.Image.FileName;
                    if (!Directory.Exists("wwwroot" + root))
                    {
                        Directory.CreateDirectory("wwwroot" + root);
                    }

                    src = root + productImage;
                    var picInfo = _fileProvider.GetFileInfo(src);
                    var rootPath = picInfo.PhysicalPath;
                    using (var stream = new FileStream(rootPath, FileMode.Create))
                    {
                        await entity.Image.CopyToAsync(stream);
                    };
                }
                if (!string.IsNullOrEmpty(ExistedProduct.ProductPicture))
                {
                    var picInfo = _fileProvider.GetFileInfo(ExistedProduct.ProductPicture);
                    var root = picInfo.PhysicalPath;
                    System.IO.File.Delete(root);
                } 
                var result = _mapper.Map<Product>(entity);
                result.ProductPicture = src;
                result.Id = id;
                //_context.Products.Remove(ExistedProduct);
                //_context.Products.Add(result);
                _context.Products.Update(result);
                await _context.SaveChangesAsync();
              
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteWithPicAsync(int id)
        {
            var ExistedProduct = await _context.Products.FindAsync(id);
            if(ExistedProduct is not null)
            {
                if (!string.IsNullOrEmpty(ExistedProduct.ProductPicture))
                {
                    var picInfo = _fileProvider.GetFileInfo(ExistedProduct.ProductPicture);
                    var root = picInfo.PhysicalPath;
                    System.IO.File.Delete(root);
                }
                _context.Products.Remove(ExistedProduct);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

