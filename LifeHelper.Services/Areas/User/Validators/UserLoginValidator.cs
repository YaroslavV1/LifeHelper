using FluentValidation;
using LifeHelper.Services.Areas.User.DTOs;

namespace LifeHelper.Services.Areas.User.Validators;

public class UserLoginValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginValidator()
    {
        RuleFor(userLogin => userLogin.Login)
            .NotEmpty().WithMessage("{PropertyName} is required");
        
        RuleFor(userLogin => userLogin.Password)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Password();
    }
}