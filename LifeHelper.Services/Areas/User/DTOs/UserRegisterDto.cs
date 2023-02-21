namespace LifeHelper.Services.Areas.User.DTOs;

public class UserRegisterDto
{
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}