using AutoMapper;

namespace LifeHelper.Services.Areas.Role.DTOs;

using Infrastructure.Entities;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<Role, RoleDto>()
            .ForMember(roleDto => roleDto.RoleName, option => option.MapFrom(role => role.NormalName));
    }
}