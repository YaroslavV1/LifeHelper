using LifeHelper.Services.Areas.User.DTOs;

namespace LifeHelper.Services.Areas.Authentication;

public interface IAuthService
{
    public Task RegistrationAsync(UserInputDto userInputDto);
    public Task<string> LoginAsync(UserLoginDto loginDto);
}