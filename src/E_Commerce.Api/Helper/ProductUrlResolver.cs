using AutoMapper;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Entities;

namespace E_Commerce.Api.Helper
{
    public class ProductUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration _Config;
        public ProductUrlResolver(IConfiguration config)
        {
                _Config = config;
        }
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.ProductPicture)) 
            { 
                var result = _Config["ApiUrl"] + source.ProductPicture;
                return result;
            }
            return null;
        }
    }
}
