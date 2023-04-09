using FluentValidation;
using LifeHelper.Services.Areas.Expenses.DTOs;
using LifeHelper.Services.Extensions;
using static LifeHelper.Services.LifeHelperConstants;

namespace LifeHelper.Services.Areas.Expenses.Validators;

public class ExpenseInputValidator : AbstractValidator<ExpenseInputDto>
{
    public ExpenseInputValidator()
    {
        RuleFor(expense => expense.SpentMoney)
            .IsRequired().Range(decimal.Zero, MaximumAmount);
    }
}