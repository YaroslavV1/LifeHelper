using FluentValidation;
using LifeHelper.Services.Areas.Categories.DTOs;
using LifeHelper.Services.Extensions;
using static LifeHelper.Services.LifeHelperConstants;

namespace LifeHelper.Services.Areas.Categories.Validators;

public class CategoryInputValidator : AbstractValidator<CategoryInputDto>
{
    public CategoryInputValidator()
    {
        RuleFor(category => category.Title)
            .Length(3, 50).WithMessage("Length of the title must be from {MinLength} - {MaxLength} characters");

        RuleFor(category => category.MoneyLimit)
            .Range(decimal.Zero, MaximumAmount);
    }
}