using System.Net.Mime;
using LifeHelper.Services.Areas.UserMonies;
using LifeHelper.Services.Areas.UserMonies.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeHelper.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class UserMoneyController : ControllerBase
{
    private readonly IUserMoneyService _userMoneyService;

    public UserMoneyController(IUserMoneyService userMoneyService)
    {
        _userMoneyService = userMoneyService;
    }

    /// <summary>
    /// Get the User Money
    /// </summary>
    /// <returns>User Money</returns>
    [HttpGet]
    [ProducesResponseType(typeof(UserMoneyDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync()
    {
        var userMoney = await _userMoneyService.GetAsync();

        return Ok(userMoney);
    }

    /// <summary>
    /// Add money to the User
    /// </summary>
    /// <param name="moneyInput">Enter the amount of money to add</param>
    /// <returns>User Money</returns>
    [HttpPut("add")]
    [ProducesResponseType(typeof(UserMoneyDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddAsync([FromBody] UserMoneyInputDto moneyInput)
    {
        var userMoney = await _userMoneyService.AddAsync(moneyInput);

        return Ok(userMoney);
    }

    /// <summary>
    /// Subtract money from the User
    /// </summary>
    /// <param name="moneyInput">Enter the amount of money to subtract</param>
    /// <returns>User Money</returns>
    [HttpPut("subtract")]
    [ProducesResponseType(typeof(UserMoneyDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubtractAsync([FromBody] UserMoneyInputDto moneyInput)
    {
        var userMoney = await _userMoneyService.SubtractAsync(moneyInput);

        return Ok(userMoney);
    }

    /// <summary>
    /// Update User money
    /// </summary>
    /// <param name="moneyInput">Enter the amount of money to update</param>
    /// <returns>User Money</returns>
    [HttpPut("update")]
    [ProducesResponseType(typeof(UserMoneyDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromBody] UserMoneyInputDto moneyInput)
    {
        var userMoney = await _userMoneyService.UpdateAsync(moneyInput);

        return Ok(userMoney);
    }
}