using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Entities;
using LifeHelper.Services.Areas.Expenses.DTOs;
using LifeHelper.Services.Exceptions;
using LifeHelper.Services.Utilities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static LifeHelper.Services.Utilities.LifeHelperUtilities;
using static LifeHelper.Services.LifeHelperConstants;

namespace LifeHelper.Services.Areas.Expenses;

public class ExpenseService : IExpenseService
{
    private readonly LifeHelperDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TokenInfoDto _currentUserInfo;

    public ExpenseService(LifeHelperDbContext dbContext, IMapper mapper, IHttpContextAccessor contextAccessor)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _currentUserInfo = ParseInfoFromClaims(contextAccessor.HttpContext);
    }
    
    public async Task<IList<ExpenseDto>> GetListAsync(int categoryId)
    {
        await ThrowIfCategoryIsNotExistsAsync(categoryId);
            
        var expenses = await _dbContext.Expenses
            .Where(expense => expense.CategoryId == categoryId)
            .ProjectTo<ExpenseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return expenses;
    }

    public async Task<ExpenseDto> GetByIdAsync(int categoryId, int expenseId)
    {
        await ThrowIfCategoryIsNotExistsAsync(categoryId);

        var expense = await _dbContext.Expenses
            .Where(expense => expense.CategoryId == categoryId)
            .ProjectTo<ExpenseDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(expense => expense.Id == expenseId);
        
        expense.ThrowIfNotFound(expenseId);

        return expense;
    }

    public async Task<ExpenseDto> CreateAsync(ExpenseInputDto expenseInput)
    {
        await ThrowIfCategoryIsNotExistsAsync(expenseInput.CategoryId);

        var expense = _mapper.Map<Expense>(expenseInput);

        await UpdateTotalAndLimitMoneyAsync(expense.CategoryId, expense.SpentMoney);
            
        await _dbContext.Expenses.AddAsync(expense);
        await _dbContext.SaveChangesAsync();

        var expenseDto = _mapper.Map<ExpenseDto>(expense);

        return expenseDto;
    }

    public async Task<ExpenseDto> UpdateByIdAsync(int expenseId, ExpenseInputDto expenseInput)
    {
        await ThrowIfCategoryIsNotExistsAsync(expenseInput.CategoryId);
        
        var expense = await _dbContext.Expenses
            .FirstOrDefaultAsync(expense => expense.Id == expenseId && expense.CategoryId == expenseInput.CategoryId);
        
        expense.ThrowIfNotFound(expenseId);
        
        var spentMoney = expenseInput.SpentMoney - expense.SpentMoney; 
        
        _mapper.Map(expenseInput, expense);

        await UpdateTotalAndLimitMoneyAsync(expense.CategoryId, spentMoney);
        
        _dbContext.Expenses.Update(expense);
        await _dbContext.SaveChangesAsync();

        var expenseDto = _mapper.Map<ExpenseDto>(expense);

        return expenseDto;
    }

    public async Task DeleteByIdAsync(int categoryId, int expenseId)
    {
        await ThrowIfCategoryIsNotExistsAsync(categoryId);
        
        var expense = await _dbContext.Expenses
            .FirstOrDefaultAsync(expense => expense.Id == expenseId && expense.CategoryId == categoryId);

        expense.ThrowIfNotFound(expenseId);

        var spentMoney = expense.SpentMoney * -1;

        await UpdateTotalAndLimitMoneyAsync(categoryId, spentMoney);
        
        _dbContext.Expenses.Remove(expense);
        await _dbContext.SaveChangesAsync();
    }

    private async Task ThrowIfCategoryIsNotExistsAsync(int categoryId)
    {
        var user = await _dbContext.Users
            .Include(user => user.Categories)
            .FirstOrDefaultAsync(user => user.Id == _currentUserInfo.Id);

        if (user is null || user.Categories.All(category => category.Id != categoryId))
        {
            throw new NotFoundException($"Category with Id: {categoryId} does not exist");
        }
    }

    private async Task UpdateTotalAndLimitMoneyAsync(int categoryId, decimal expensedMoney)
    {
        var userMoney = await _dbContext.UserMonies.FirstOrDefaultAsync(money => money.UserId == _currentUserInfo.Id);
        userMoney.ThrowIfNotFound("User money was not found to update money");
        
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(category => category.Id == categoryId && category.UserId == _currentUserInfo.Id);
        
        category.ThrowIfNotFound("Category was not found to update money limit");
        
        userMoney.Money -= expensedMoney;
        category.MoneyLimit -= expensedMoney;
        
        ThrowIfMoneyLessThanRange(userMoney.Money, category.MoneyLimit);

        _dbContext.UserMonies.Update(userMoney);
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
    }

    private void ThrowIfMoneyLessThanRange(decimal userMoney, decimal moneyLimit)
    {
        if(userMoney < MinimumAmount || moneyLimit < MinimumAmount)
        {
            throw new BadRequestException("User money or category money limit is less than range");
        }
    }
}