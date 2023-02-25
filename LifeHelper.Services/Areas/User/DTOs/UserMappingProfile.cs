using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LifeHelper.Services.Areas.User.DTOs;

using Infrastructure.Entities;

public class UserMappingProfile: Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserInputDto, User>()
            .ForMember(user => user.PasswordHash, option => option.MapFrom(userInput => userInput.Password));
    }
}