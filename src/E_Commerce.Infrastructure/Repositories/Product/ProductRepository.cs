using AutoMapper;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Entities;
using E_Commerce.Core.Interfaces;
using E_Commerce.Core.Sharing;
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

        public async Task<IEnumerable<ProductDto>> GetAllAsync(ProductParams productParams)
        {
            var products = await _context.Products.Include(x=>x.category).AsNoTracking().ToListAsync();

            //Searching
            if (!string.IsNullOrEmpty(productParams.Search))
            {
                products = products.Where(x => x.Name.ToLower().Contains(productParams.Search)).ToList();
            }
            //Get By CategoryId
            if (productParams.CategoryId.HasValue)
            {
                products = products.Where(x=>x.CategoryId == productParams.CategoryId.Value).ToList();
            }

            //Sorting
            if(!string.IsNullOrWhiteSpace(productParams.sort))
            {
                switch (productParams.sort)
                {
                    case "PriceAsending":
                        products = products.OrderBy(x=>x.Price).ToList(); 
                        break;
                    case "PriceDescinding":
                        products =products.OrderByDescending(x=>x.Price).ToList();
                        break;
                    case "NameDescinding":
                        products = products.OrderByDescending(x => x.Name).ToList();
                        break;
                    case "NameAsending":
                        products = products.OrderBy(x => x.Name).ToList();
                        break;
                    default:
                        products = products.OrderBy(x=>x.Id).ToList();
                        break;
                }
            }
            //Paging
            productParams.PageNumber = productParams.PageNumber > 0 ? productParams.PageNumber : 1;
            productParams.PageSize = productParams.PageSize > 0 ? productParams.PageSize : 3;
            products = products.Skip((productParams.PageSize) * (productParams.PageNumber - 1)).Take(productParams.PageSize).ToList();


            var result =  _mapper.Map<List<ProductDto>>(products);
            return result;
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

