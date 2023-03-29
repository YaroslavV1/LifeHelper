using FluentValidation;
using LifeHelper.Services.Areas.Expenses.DTOs;
using LifeHelper.Services.Areas.Helpers.Validators;

namespace LifeHelper.Services.Areas.Expenses.Validators;

public class ExpenseInputValidator : AbstractValidator<ExpenseInputDto>
{
    private const decimal MinimumAmount = decimal.Zero;
    private const decimal MaximumAmount = 999_999_999.99m;
    
    public ExpenseInputValidator()
    {
        RuleFor(expense => expense.SpentMoney).IsRequired().Range(MinimumAmount, MaximumAmount);
    }
}