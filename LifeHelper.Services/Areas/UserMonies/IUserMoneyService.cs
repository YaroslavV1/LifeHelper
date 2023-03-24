using LifeHelper.Services.Areas.UserMonies.DTOs;

namespace LifeHelper.Services.Areas.UserMonies;

public interface IUserMoneyService
{
    public Task<UserMoneyDto> GetAsync();
    public Task<UserMoneyDto> AddAsync(UserMoneyInputDto moneyInput);
    public Task<UserMoneyDto> SubtractAsync(UserMoneyInputDto moneyInput);
    public Task<UserMoneyDto> UpdateAsync(UserMoneyInputDto moneyInput);
}