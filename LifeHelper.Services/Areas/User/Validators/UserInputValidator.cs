using FluentValidation;
using LifeHelper.Services.Areas.User.DTOs;

namespace LifeHelper.Services.Areas.User.Validators;

public class UserInputValidator : AbstractValidator<UserInputDto>
{
    public UserInputValidator()
    {
        RuleFor(x => x.Nickname)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Length(3, 21).WithMessage("{PropertyName} must least from {MinLength} to {MaxLength} characters");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .EmailAddress().WithMessage("Incorrect Email address");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Password();
        
        
    }
}