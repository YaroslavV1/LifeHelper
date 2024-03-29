﻿using FluentValidation;
using LifeHelper.Services.Areas.Users.DTOs;
using LifeHelper.Services.Extensions;

namespace LifeHelper.Services.Areas.Users.Validators;

public class UserInputValidator : AbstractValidator<UserInputDto>
{
    public UserInputValidator()
    {
        RuleFor(userInput => userInput.Nickname)
            .IsRequired()
            .Length(3, 21).WithMessage("{PropertyName} must least from {MinLength} to {MaxLength} characters");
        
        RuleFor(userInput => userInput.Email)
            .IsRequired()
            .EmailAddress().WithMessage("Incorrect Email address");

        RuleFor(userInput => userInput.Password)
            .IsRequired()
            .Password();
        
        
    }
}