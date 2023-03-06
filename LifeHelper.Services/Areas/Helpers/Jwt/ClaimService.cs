using System.Security.Claims;
using LifeHelper.Services.Areas.Helpers.Jwt.DTOs;
using Microsoft.AspNetCore.Http;

namespace LifeHelper.Services.Areas.Helpers.Jwt;

public class ClaimService : IClaimService
{
    public TokenInfoDto GetParsedInfo(HttpContext context)
    {
        return new TokenInfoDto
        {
            Id = Convert.ToInt32(context.User.FindFirstValue(ClaimTypes.NameIdentifier))
        };
    }
}