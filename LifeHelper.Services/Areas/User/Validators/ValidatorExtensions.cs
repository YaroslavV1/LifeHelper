using FluentValidation;

namespace LifeHelper.Services.Areas.User.Validators;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> CheckPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithMessage("{PropertyName} is required!")
            .Length(8,32)
                .WithMessage("{PropertyName} must be least 8 characters in length, but no more than 32")
            .Matches("[0-9]")
                .WithMessage("{PropertyName} must least one digit")
            .Matches("[a-z]")
                .WithMessage("{PropertyName} must least one lowercase character!")
            .Matches("[A-Z]")
                .WithMessage("{PropertyName} least one uppercase character");
    }
}