using FluentValidation;
using LifeHelper.Services.Areas.Helpers.Validators;
using LifeHelper.Services.Areas.UserMonies.DTOs;

namespace LifeHelper.Services.Areas.UserMonies.Validators;

public class UserMoneyInputValidator : AbstractValidator<UserMoneyInputDto>
{
    private const decimal MinimumAmount = 0;
    private const decimal MaximumAmount = 999_999_999.99m;
    
    public UserMoneyInputValidator()
    {
        RuleFor(money => money.Amount).Range(MinimumAmount, MaximumAmount);
    }
}