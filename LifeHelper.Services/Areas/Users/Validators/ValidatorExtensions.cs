﻿using FluentValidation;

namespace LifeHelper.Services.Areas.Users.Validators;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .IsRequired()
            .Length(8,32).WithMessage("{PropertyName} must least from 8 to 32 characters")
            .Matches("[0-9]").WithMessage("{PropertyName} must least one digit")
            .Matches("[a-z]").WithMessage("{PropertyName} must least one lowercase character")
            .Matches("[A-Z]").WithMessage("{PropertyName} must least one uppercase character");
    }

    public static IRuleBuilderOptions<T, string> IsRequired<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().WithMessage("{PropertyName} is required");
    }
}