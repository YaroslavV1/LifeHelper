using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Entities;
using LifeHelper.Infrastructure.Exceptions;
using LifeHelper.Services.Areas.Categories.DTOs;
using LifeHelper.Services.Areas.Helpers.Jwt;
using LifeHelper.Services.Areas.Helpers.Jwt.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LifeHelper.Services.Areas.Categories;

public class CategoryService : ICategoryService
{
    private readonly LifeHelperDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TokenInfoDto _currentUserInfo;
    
    public CategoryService(
        LifeHelperDbContext dbContext,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        IClaimParserService claimParserService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _currentUserInfo = claimParserService.ParseInfoFromClaims(contextAccessor.HttpContext);
    }
    
    public async Task<IList<CategoryDto>> GetListAsync()
    {
        var categories = await _dbContext.Categories
            .Where(category => category.UserId == _currentUserInfo.Id)
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return categories;
    }

    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        var category = await _dbContext.Categories
                           .Where(category => category.UserId == _currentUserInfo.Id)
                           .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                           .FirstOrDefaultAsync(category => category.Id == id)
                       ?? throw new NotFoundException($"Category with Id: {id} not found");

        return category;
    }

    public async Task<CategoryDto> CreateAsync(CategoryInputDto categoryInput)
    {
        var category = _mapper.Map<Category>(categoryInput);
        
        await ThrowIfTitleExistsAsync(categoryInput.Title, category.Id);
       
        category.UserId = _currentUserInfo.Id;
       
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
       
        var categoryDto = _mapper.Map<CategoryDto>(category);
        
        return categoryDto;
    }

    public async Task<CategoryDto> UpdateByIdAsync(int id, CategoryInputDto categoryInput)
    {
        var category = await _dbContext.Categories
                           .FirstOrDefaultAsync(category => category.Id == id && category.UserId == _currentUserInfo.Id) 
                       ?? throw new NotFoundException($"Category with Id: {id} not found");

        await ThrowIfTitleExistsAsync(categoryInput.Title, category.Id);
        
        _mapper.Map(categoryInput, category);
        
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
        
        var categoryDto = _mapper.Map<CategoryDto>(category);
        
        return categoryDto;
    }

    public async Task DeleteByIdAsync(int id)
    {
        var category = await _dbContext.Categories
                           .FirstOrDefaultAsync(category => category.Id == id && category.UserId == _currentUserInfo.Id)
                       ?? throw new NotFoundException($"Category with Id: {id} not found");
        
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }

    private async Task ThrowIfTitleExistsAsync(string title, int id)
    {
        var isTitleExists = await _dbContext.Categories
            .Where(category => category.UserId == _currentUserInfo.Id)
            .AnyAsync(category => category.Title.ToLower() == title.ToLower() && category.Id != id);

        if (isTitleExists)
        {
            throw new BadRequestException("Category with this title is already exists");
        }
    }
}