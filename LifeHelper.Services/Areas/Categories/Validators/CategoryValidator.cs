using FluentValidation;
using LifeHelper.Services.Areas.Categories.DTOs;
using LifeHelper.Services.Areas.Helpers.Validators;
using LifeHelper.Services.Areas.Users.Validators;

namespace LifeHelper.Services.Areas.Categories.Validators;

public class CategoryValidator : AbstractValidator<CategoryInputDto>
{
    private const decimal MinimumAmount = decimal.Zero;
    private const decimal MaximumAmount = 999_999_999.99m;
    
    public CategoryValidator()
    {
        RuleFor(category => category.Title)
            .Length(3, 50).WithMessage("Length of the title must be from {MinLength} - {MaxLength} characters");

        RuleFor(category => category.MoneyLimit).Range(MinimumAmount, MaximumAmount);
    }
}