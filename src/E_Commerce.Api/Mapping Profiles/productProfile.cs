using AutoMapper;
using E_Commerce.Api.Dtos;
using E_Commerce.Core.Entities;

namespace E_Commerce.Api.Mapping_Profiles
{
    public class productProfile:Profile
    {
        public productProfile()
        {
            CreateMap<Product,ProductDto>().ForMember(d=>d.CategoryName,o=>o.MapFrom(s=>s.category.Name)).ReverseMap();
            CreateMap<CreateProductDto, Product>().ReverseMap();
            CreateMap<UpdateCategoryDto, Product>().ReverseMap();
        }
        
    }
}
