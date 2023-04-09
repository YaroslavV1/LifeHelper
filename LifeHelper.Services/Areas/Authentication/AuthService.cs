using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LifeHelper.Services.Areas.Users;
using LifeHelper.Services.Areas.Users.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LifeHelper.Services.Areas.Authentication;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AuthService(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }
    
    public async Task RegisterAsync(UserInputDto userInputDto)
    {
        await _userService.CreateAsync(userInputDto);
    }

    public async Task<string> LoginAsync(UserLoginDto loginDto)
    {
        var loggedInUser = await _userService.GetByLoginAsync(loginDto);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, loggedInUser.Id.ToString()),
            new(ClaimTypes.Name, loggedInUser.Nickname),
            new(ClaimTypes.Email, loggedInUser.Email),
            new(ClaimTypes.Role, loggedInUser.Roles.ElementAt(0).RoleName)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSecurity:SecurityKey").Value!));
        
        var signinCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _configuration.GetSection("JwtSecurity:Issuer").Value,
            audience: _configuration.GetSection("JwtSecurity:Audience").Value,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            notBefore: DateTime.UtcNow,
            signingCredentials: signinCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}