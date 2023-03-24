using LifeHelper.Services.Areas.Role.DTOs;

namespace LifeHelper.Services.Areas.Users.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public IList<RoleDto> Roles { get; set; }
}