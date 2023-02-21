using AutoMapper;

namespace LifeHelper.Services.Areas.User.DTOs;

using Infrastructure.Entities;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, User>();
    }
}