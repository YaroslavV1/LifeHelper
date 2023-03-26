using AutoMapper;
using LifeHelper.Infrastructure.Entities;

namespace LifeHelper.Services.Areas.Categories.DTOs;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryInputDto, Category>();
    }
}