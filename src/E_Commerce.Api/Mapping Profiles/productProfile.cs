using AutoMapper;
using E_Commerce.Api.Helper;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Entities;

namespace E_Commerce.Api.Mapping_Profiles
{
    public class productProfile:Profile
    {
        public productProfile()
        {
            CreateMap<Product,ProductDto>().ForMember(d=>d.CategoryName,o=>o.MapFrom(s=>s.category.Name)).ForMember(d=>d.ProductPicture,o=>o.MapFrom<ProductUrlResolver>()).ReverseMap();
            CreateMap<CreateProductDto, Product>().ReverseMap();
            CreateMap<UpdateProductDto, Product>().ReverseMap();
        }
        
    }
}
