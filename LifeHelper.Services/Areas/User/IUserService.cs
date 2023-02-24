using LifeHelper.Services.Areas.User.DTOs;

namespace LifeHelper.Services.Areas.User;

using Infrastructure.Entities;

public interface IUserService
{
    public Task<ICollection<UserDto>> GetAllUsersAsync();
    public Task<UserDto> GetByIdAsync(int id);
    public Task<UserDto> GetByNicknameAsync(string nickname);
    public Task<int> CreateAsync(UserInputDto userInputDto);
    public Task<int> UpdateAsync(int id, UserInputDto userInputDto);
    public Task DeleteAsync(int id);
    public bool VerifyHashedPassword(User user, string password);
}