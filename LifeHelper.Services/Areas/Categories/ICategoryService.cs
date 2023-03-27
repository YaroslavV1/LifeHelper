using LifeHelper.Services.Areas.Categories.DTOs;

namespace LifeHelper.Services.Areas.Categories;

public interface ICategoryService
{
    public Task<IList<CategoryDto>> GetListAsync();
    public Task<CategoryDto> GetByIdAsync(int id);
    public Task<CategoryDto> CreateAsync(CategoryInputDto categoryInput);
    public Task<CategoryDto> UpdateByIdAsync(int id, CategoryInputDto categoryInput);
    public Task DeleteByIdAsync(int id);
}