using FluentValidation;
using LifeHelper.Services.Areas.User.DTOs;

namespace LifeHelper.Services.Areas.User.Validators;

public class UserLoginValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginValidator()
    {
        RuleFor(userLogin => userLogin.Login)
            .IsRequired();
        
        RuleFor(userLogin => userLogin.Password)
            .IsRequired()
            .Password();
    }
}