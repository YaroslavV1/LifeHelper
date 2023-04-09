using System.Net.Mime;
using LifeHelper.Services.Areas.Categories;
using LifeHelper.Services.Areas.Categories.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeHelper.Api.Controllers;

[ApiController]
[Route("api/categories")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Get the List of Categories
    /// </summary>
    /// <returns>List of Categories</returns>
    [HttpGet]
    public async Task<IActionResult> GetListAsync()
    {
        var categories = await _categoryService.GetListAsync();

        return Ok(categories);
    }

    /// <summary>
    /// Get the Category by ID
    /// </summary>
    /// <param name="id">Enter Category ID</param>
    /// <returns>Category</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        return Ok(category);
    }

    /// <summary>
    /// Create Category
    /// </summary>
    /// <param name="categoryInput"></param>
    /// <returns>Created Category</returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CategoryInputDto categoryInput)
    {
        var category = await _categoryService.CreateAsync(categoryInput);

        return Ok(category);
    }

    /// <summary>
    /// Update the Category by ID
    /// </summary>
    /// <param name="id">Enter Category ID</param>
    /// <param name="categoryInput"></param>
    /// <returns>Updated Category</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateByIdAsync([FromRoute] int id, [FromBody] CategoryInputDto categoryInput)
    {
        var category = await _categoryService.UpdateByIdAsync(id, categoryInput);

        return Ok(category);
    }

    /// <summary>
    /// Delete the Category by ID
    /// </summary>
    /// <param name="id">Enter Category ID</param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] int id)
    {
        await _categoryService.DeleteByIdAsync(id);

        return Ok();
    }
}