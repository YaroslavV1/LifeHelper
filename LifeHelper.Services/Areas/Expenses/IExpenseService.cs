using LifeHelper.Services.Areas.Expenses.DTOs;

namespace LifeHelper.Services.Areas.Expenses;

public interface IExpenseService
{
    public Task<IList<ExpenseDto>> GetListAsync(int categoryId);
    public Task<ExpenseDto> GetByIdAsync(int categoryId, int expenseId);
    public Task<ExpenseDto> CreateAsync(ExpenseInputDto expenseInput);
    public Task<ExpenseDto> UpdateByIdAsync(int expenseId, ExpenseInputDto expenseInput);
    public Task DeleteByIdAsync(int categoryId, int expenseId);
}