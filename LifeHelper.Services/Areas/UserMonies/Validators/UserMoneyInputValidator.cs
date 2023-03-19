using FluentValidation;
using LifeHelper.Services.Areas.UserMonies.DTOs;

namespace LifeHelper.Services.Areas.UserMonies.Validators;

public class UserMoneyInputValidator : AbstractValidator<UserMoneyInputDto>
{
    private const decimal MinimumValue = 0;
    private const decimal MaximumValue = 999_999_999.99m;
    
    public UserMoneyInputValidator()
    {
        RuleFor(money => money.Money)
            .GreaterThanOrEqualTo(MinimumValue)
            .WithMessage("Enter a value that is greater than or equal to {ComparisonValue}")
            .LessThanOrEqualTo(MaximumValue)
            .WithMessage("Enter a value that is less than or equal to {ComparisonValue}");
    }
}