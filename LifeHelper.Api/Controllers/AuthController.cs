using System.Net.Mime;
using LifeHelper.Services.Areas.Authentication;
using LifeHelper.Services.Areas.User.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeHelper.Api.Controllers;


[ApiController]
[Route("api/auth")]
[Produces(MediaTypeNames.Application.Json)]
[AllowAnonymous]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    /// <summary>
    /// User Registration
    /// </summary>
    /// <param name="userInputDto"></param>
    /// <returns></returns>
    [HttpPost("registration")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegistrationAsync([FromBody] UserInputDto userInputDto)
    {
        await _authService.RegistrationAsync(userInputDto);
        
        return Ok();
    }
    
    /// <summary>
    /// User Login
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto loginDto)
    {
        var jwtToken = await _authService.LoginAsync(loginDto);

        return Ok(jwtToken);
    }
}