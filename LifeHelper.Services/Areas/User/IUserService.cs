using LifeHelper.Services.Areas.User.DTOs;

namespace LifeHelper.Services.Areas.User;

using Infrastructure.Entities;

public interface IUserService
{
    public Task<IList<UserDto>> GetListAsync();
    public Task<UserDto> GetByIdAsync(int id);
    public Task<UserDto> GetByNicknameAsync(string nickname);
    public Task<int> CreateAsync(UserInputDto userInputDto);
    public Task<int> UpdateByIdAsync(int id, UserInputDto userInputDto);
    public Task DeleteByIdAsync(int id);
    public Task<bool> CheckIfEmailIsAvailableAsync(string email);
    public Task<bool> VerifyHashedPasswordAsync(User user, string password);
}