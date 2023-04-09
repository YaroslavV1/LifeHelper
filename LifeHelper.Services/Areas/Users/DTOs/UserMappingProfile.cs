using AutoMapper;
using LifeHelper.Infrastructure.Entities;

namespace LifeHelper.Services.Areas.Users.DTOs;

public class UserMappingProfile: Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserInputDto, User>()
            .ForMember(user => user.PasswordHash, option => option.MapFrom(userInput => userInput.Password));
    }
}