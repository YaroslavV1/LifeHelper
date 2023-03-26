namespace LifeHelper.Services.Areas.Helpers.Jwt.DTOs;

using Infrastructure.Entities;

public class TokenInfoDto
{
    public int Id { get; set; }
    public string? Role { get; set; }
}