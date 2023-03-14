using LifeHelper.Services.Areas.User.DTOs;

namespace LifeHelper.Services.Areas.User;

public interface IUserService
{
    public Task<IList<UserDto>> GetListAsync();
    public Task<UserDto> GetByIdAsync(int id);
    public Task<UserDto> GetByLoginAsync(UserLoginDto loginDto);
    public Task<UserDto> CreateAsync(UserInputDto userInputDto);
    public Task<UserDto> UpdateByIdAsync(int id, UserInputDto userInputDto);
    public Task DeleteByIdAsync(int id);
    public Task<bool> VerifyHashedPasswordAsync(int userId, string password);
}