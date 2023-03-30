using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Entities;
using LifeHelper.Infrastructure.Exceptions;
using LifeHelper.Services.Areas.Expenses.DTOs;
using LifeHelper.Services.Areas.Helpers.Jwt;
using LifeHelper.Services.Areas.Helpers.Jwt.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LifeHelper.Services.Areas.Expenses;

public class ExpenseService : IExpenseService
{
    private readonly LifeHelperDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TokenInfoDto _currentUserInfo;
    private const decimal MinimumAmount = -999_999_999.99m;

    public ExpenseService(
        LifeHelperDbContext dbContext,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        IClaimParserService claimParserService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _currentUserInfo = claimParserService.ParseInfoFromClaims(contextAccessor.HttpContext);
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

    public async Task<ExpenseDto> GetByIdAsync(int categoryId, int id)
    {
        await ThrowIfCategoryIsNotExistsAsync(categoryId);

        var expense = await _dbContext.Expenses
                          .Where(expense => expense.CategoryId == categoryId)
                          .ProjectTo<ExpenseDto>(_mapper.ConfigurationProvider)
                          .FirstOrDefaultAsync(expense => expense.Id == id)
                      ?? throw new NotFoundException($"Expense with Id: {id} does not exist");

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

    public async Task<ExpenseDto> UpdateByIdAsync(int id, ExpenseInputDto expenseInput)
    {
        await ThrowIfCategoryIsNotExistsAsync(expenseInput.CategoryId);
        
        var expense = await _dbContext.Expenses.
                          FirstOrDefaultAsync(expense => expense.Id == id && expense.CategoryId == expenseInput.CategoryId) 
                      ?? throw new NotFoundException($"Expense with Id: {id} does not exist");
        
        var spentMoney = expenseInput.SpentMoney - expense.SpentMoney; 
        
        _mapper.Map(expenseInput, expense);

        await UpdateTotalAndLimitMoneyAsync(expense.CategoryId, spentMoney);
        
        _dbContext.Expenses.Update(expense);
        await _dbContext.SaveChangesAsync();

        var expenseDto = _mapper.Map<ExpenseDto>(expense);

        return expenseDto;
    }

    public async Task DeleteByIdAsync(int categoryId, int id)
    {
        await ThrowIfCategoryIsNotExistsAsync(categoryId);
        
        var expense = await _dbContext.Expenses
                          .FirstOrDefaultAsync(expense => expense.Id == id && expense.CategoryId == categoryId)
                      ?? throw new NotFoundException($"Expense with Id: {id} does not exist");

        var spentMoney = -expense.SpentMoney;

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
        var userMoney = await _dbContext.UserMonies.FirstOrDefaultAsync(money => money.UserId == _currentUserInfo.Id) 
                        ?? throw new NotFoundException("User money was not found to update money");
        
        var category = await _dbContext.Categories
                           .FirstOrDefaultAsync(category =>
                               category.Id == categoryId && category.UserId == _currentUserInfo.Id) 
                       ?? throw new NotFoundException("Category was not found to update money limit");
        
        userMoney.Money -= expensedMoney;
        category.MoneyLimit -= expensedMoney;
        
        ThrowIfMoneyOutOfRange(userMoney.Money, category.MoneyLimit);

        _dbContext.UserMonies.Update(userMoney);
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
    }

    private void ThrowIfMoneyOutOfRange(decimal userMoney, decimal moneyLimit)
    {
        if(userMoney < MinimumAmount || moneyLimit < MinimumAmount)
        {
            throw new BadRequestException("User money or category money limit is out of range");
        }
    }
}