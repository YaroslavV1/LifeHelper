using FluentValidation;
using LifeHelper.Services.Areas.User.DTOs;

namespace LifeHelper.Services.Areas.User.Validators;

public class UserInputValidator : AbstractValidator<UserInputDto>
{
    public UserInputValidator()
    {
        RuleFor(userInput => userInput.Nickname)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Length(3, 21).WithMessage("{PropertyName} must least from {MinLength} to {MaxLength} characters");
        
        RuleFor(userInput => userInput.Email)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .EmailAddress().WithMessage("Incorrect Email address");

        RuleFor(userInput => userInput.Password)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Password();
        
        
    }
}