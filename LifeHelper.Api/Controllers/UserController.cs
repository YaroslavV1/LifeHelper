using LifeHelper.Api.Models;
using LifeHelper.Infrastructure.Exceptions;
using LifeHelper.Services.Areas.User;
using LifeHelper.Services.Areas.User.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LifeHelper.Api.Controllers;


[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    /// <summary>
    /// Get the List of Users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IList<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync()
    {
        var users = await _userService.GetListAsync();

        return Ok(users);
    }

    
    /// <summary>
    /// Get the User by Id
    /// </summary>
    /// <param name="id">User Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var user = await _userService.GetByIdAsync(id);

        return Ok(user);
    }
    
    /// <summary>
    /// Get the User by Nickname
    /// </summary>
    /// <param name="nickname">User Nickname</param>
    /// <returns></returns>
    [HttpGet("{nickname}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByNicknameAsync([FromRoute] string nickname)
    {
        var user = await _userService.GetByNicknameAsync(nickname);

        return Ok(user);
    }

    /// <summary>
    /// Create new User
    /// </summary>
    /// <param name="userInputDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] UserInputDto userInputDto)
    {
        var userId = await _userService.CreateAsync(userInputDto);
        var user = await _userService.GetByIdAsync(userId);

        return CreatedAtAction("GetById", new { id = user.Id }, user);
    }

    /// <summary>
    /// Update User by Id
    /// </summary>
    /// <param name="id">User Id</param>
    /// <param name="userInputDto"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateByIdAsync([FromRoute] int id, [FromBody] UserInputDto userInputDto)
    {
        var userId = await _userService.UpdateByIdAsync(id, userInputDto);
        var user = await _userService.GetByIdAsync(id);

        return Ok(user);
    }

    /// <summary>
    /// Delete User by Id
    /// </summary>
    /// <param name="id">User Id</param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] int id)
    {
        await _userService.DeleteByIdAsync(id);

        return Ok();
    }
}