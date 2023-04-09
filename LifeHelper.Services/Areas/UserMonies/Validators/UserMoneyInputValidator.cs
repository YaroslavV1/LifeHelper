using FluentValidation;
using LifeHelper.Services.Areas.UserMonies.DTOs;
using LifeHelper.Services.Extensions;
using static LifeHelper.Services.LifeHelperConstants;

namespace LifeHelper.Services.Areas.UserMonies.Validators;

public class UserMoneyInputValidator : AbstractValidator<UserMoneyInputDto>
{
    public UserMoneyInputValidator()
    {
        RuleFor(money => money.Amount)
            .Range(decimal.Zero, MaximumAmount);
    }
}