using FluentValidation;
using LifeHelper.Services.Areas.Users.DTOs;

namespace LifeHelper.Services.Areas.Users.Validators;

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