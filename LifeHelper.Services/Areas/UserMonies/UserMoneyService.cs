using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Entities;
using LifeHelper.Services.Areas.UserMonies.DTOs;
using LifeHelper.Services.Exceptions;
using LifeHelper.Services.Utilities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static LifeHelper.Services.Utilities.LifeHelperUtilities;
using static LifeHelper.Services.LifeHelperConstants;

namespace LifeHelper.Services.Areas.UserMonies;

public class UserMoneyService : IUserMoneyService
{
    private readonly LifeHelperDbContext _context;
    private readonly IMapper _mapper;
    private readonly TokenInfoDto _currentUserInfo;

    public UserMoneyService(LifeHelperDbContext context, IMapper mapper, IHttpContextAccessor contextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _currentUserInfo = ParseInfoFromClaims(contextAccessor.HttpContext);
    }
    
    public async Task<UserMoneyDto> GetAsync()
    {
        var userMoney = await _context.UserMonies
            .Where(money => money.UserId == _currentUserInfo.Id)
            .ProjectTo<UserMoneyDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        
        userMoney.ThrowIfNotFound("User money was not found!");

        return userMoney;
    }

    public async Task<UserMoneyDto> AddAsync(UserMoneyInputDto moneyInput)
    {
        var userMoney = await _context.UserMonies.FirstOrDefaultAsync(monies => monies.UserId == _currentUserInfo.Id);
        userMoney.ThrowIfNotFound("User money was not found!");

        decimal totalAmountMoney = userMoney.Money + moneyInput.Amount;
        ThrowIfDecimalOutOfRange(totalAmountMoney);
        
        await UpdateUserMoneyAsync(userMoney, totalAmountMoney);

        var userMoneyDto = _mapper.Map<UserMoneyDto>(userMoney);

        return userMoneyDto;
    }

    public async Task<UserMoneyDto> SubtractAsync(UserMoneyInputDto moneyInput)
    {
        var userMoney = await _context.UserMonies.FirstOrDefaultAsync(monies => monies.UserId == _currentUserInfo.Id);
        userMoney.ThrowIfNotFound("User money was not found!");

        decimal totalAmountMoney = userMoney.Money - moneyInput.Amount;
        ThrowIfDecimalOutOfRange(totalAmountMoney);
        
        await UpdateUserMoneyAsync(userMoney, totalAmountMoney);

        var userMoneyDto = _mapper.Map<UserMoneyDto>(userMoney);

        return userMoneyDto;
    }

    public async Task<UserMoneyDto> UpdateAsync(UserMoneyInputDto moneyInput)
    {
        var userMoney = await _context.UserMonies.FirstOrDefaultAsync(monies => monies.UserId == _currentUserInfo.Id);
        userMoney.ThrowIfNotFound("User money was not found!");

        ThrowIfDecimalOutOfRange(moneyInput.Amount);
        
        await UpdateUserMoneyAsync(userMoney, moneyInput.Amount);
        
        var userMoneyDto = _mapper.Map<UserMoneyDto>(userMoney);

        return userMoneyDto;
    }

    private async Task UpdateUserMoneyAsync(UserMoney userMoney, decimal money)
    {
        userMoney.Money = money;
        _context.UserMonies.Update(userMoney);
        await _context.SaveChangesAsync();
    }

    private void ThrowIfDecimalOutOfRange(decimal amount)
    {
        if (amount >= MaximumAmount || amount <= MinimumAmount)
        {
            throw new BadRequestException("Amount of money is not included in the allowed values");
        }
    }
}