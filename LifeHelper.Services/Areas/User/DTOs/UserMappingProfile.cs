using AutoMapper;

namespace LifeHelper.Services.Areas.User.DTOs;

using Infrastructure.Entities;

public class UserMappingProfile: Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<UserInputDto, User>();
    }
}