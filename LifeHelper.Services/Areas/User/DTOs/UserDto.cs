using LifeHelper.Infrastructure.Entities;

namespace LifeHelper.Services.Areas.User.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public IList<Role> Roles { get; set; }
}