using LifeHelper.Services.Areas.Users.DTOs;

namespace LifeHelper.Services.Areas.Authentication;

public interface IAuthService
{
    public Task RegisterAsync(UserInputDto userInputDto);
    public Task<string> LoginAsync(UserLoginDto loginDto);
}