using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LifeHelper.Services.Areas.User.DTOs;

using Infrastructure.Entities;

public class UserMappingProfile: Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<UserInputDto, User>()
            .ForMember(des => des.PasswordHash, 
                op => op.MapFrom(u => u.Password));
    }
}