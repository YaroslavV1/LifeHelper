using System.Net.Mime;
using LifeHelper.Services.Areas.Expenses;
using LifeHelper.Services.Areas.Expenses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeHelper.Api.Controllers;

[ApiController]
[Route("api/expenses")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpenseController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    /// <summary>
    /// Get the List Of Expenses by Category ID
    /// </summary>
    /// <param name="categoryId">Enter the Category ID</param>
    /// <returns>List of Expenses</returns>
    [HttpGet("{categoryId:int}")]
    public async Task<IActionResult> GetListAsync([FromRoute] int categoryId)
    {
        var expenses = await _expenseService.GetListAsync(categoryId);

        return Ok(expenses);
    }

    /// <summary>
    /// Get the Expense by Category ID and Expense ID
    /// </summary>
    /// <param name="categoryId">Enter the Category ID</param>
    /// <param name="expenseId">Enter the Expense ID</param>
    /// <returns>Expense</returns>
    [HttpGet("{categoryId:int}/{expenseId:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int categoryId, [FromRoute] int expenseId)
    {
        var expense = await _expenseService.GetByIdAsync(categoryId, expenseId);

        return Ok(expense);
    }

    /// <summary>
    /// Create the Expense of the Category
    /// </summary>
    /// <param name="expenseInput"></param>
    /// <returns>Created Expense</returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ExpenseInputDto expenseInput)
    {
        var expense = await _expenseService.CreateAsync(expenseInput);

        return Ok(expense);
    }

    /// <summary>
    /// Update the Expense of the Category
    /// </summary>
    /// <param name="expenseId">Enter the Expense ID</param>
    /// <param name="expenseInput"></param>
    /// <returns>Updated Expense</returns>
    [HttpPut("{expenseId:int}")]
    public async Task<IActionResult> UpdateByIdAsync([FromRoute] int expenseId, [FromBody] ExpenseInputDto expenseInput)
    {
        var expense = await _expenseService.UpdateByIdAsync(expenseId, expenseInput);

        return Ok(expense);
    }

    /// <summary>
    /// Delete the Expense by Category ID and Expense ID
    /// </summary>
    /// <param name="categoryId">Enter the Category ID</param>
    /// <param name="expenseId">Enter the Expense ID</param>
    /// <returns></returns>
    [HttpDelete("{categoryId:int}/{expenseId:int}")]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] int categoryId, [FromRoute] int expenseId)
    {
        await _expenseService.DeleteByIdAsync(categoryId, expenseId);

        return Ok();
    }
}