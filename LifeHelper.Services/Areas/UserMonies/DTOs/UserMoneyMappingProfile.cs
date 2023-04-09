using AutoMapper;
using LifeHelper.Infrastructure.Entities;

namespace LifeHelper.Services.Areas.UserMonies.DTOs;

public class UserMoneyMappingProfile : Profile
{
    public UserMoneyMappingProfile()
    {
        CreateMap<UserMoney, UserMoneyDto>()
            .ForMember(moneyInput => moneyInput.Amount, options => options.MapFrom(userMoney => userMoney.Money));
    }
}