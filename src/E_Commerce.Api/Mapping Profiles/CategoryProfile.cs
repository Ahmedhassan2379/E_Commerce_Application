using AutoMapper;
using E_Commerce.Api.Dtos;
using E_Commerce.Core.Entities;

namespace E_Commerce.Api.Mapping_Profiles
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile() 
        {
            CreateMap<CategoryDto,Category>().ReverseMap();
            CreateMap<ListingCategoryDto,Category>().ReverseMap();
            CreateMap<UpdateCategoryDto,Category>().ReverseMap();
        }
    }
}
