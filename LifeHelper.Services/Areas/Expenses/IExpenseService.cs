using LifeHelper.Services.Areas.Expenses.DTOs;

namespace LifeHelper.Services.Areas.Expenses;

public interface IExpenseService
{
    public Task<IList<ExpenseDto>> GetListAsync(int categoryId);
    public Task<ExpenseDto> GetByIdAsync(int categoryId, int id);
    public Task<ExpenseDto> CreateAsync(ExpenseInputDto expenseInput);
    public Task<ExpenseDto> UpdateByIdAsync(int id, ExpenseInputDto expenseInput);
    public Task DeleteByIdAsync(int categoryId, int id);
}