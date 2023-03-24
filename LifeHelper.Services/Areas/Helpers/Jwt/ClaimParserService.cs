using System.Security.Claims;
using LifeHelper.Services.Areas.Helpers.Jwt.DTOs;
using Microsoft.AspNetCore.Http;

namespace LifeHelper.Services.Areas.Helpers.Jwt;

using Infrastructure.Entities;
public class ClaimParserService : IClaimParserService
{
    public TokenInfoDto ParseInfoFromClaims(HttpContext context)
    {
        return new TokenInfoDto
        {
            Id = Convert.ToInt32(context.User.FindFirstValue(ClaimTypes.NameIdentifier)),
            Role = Convert.ToString(context.User.FindFirstValue(ClaimTypes.Role))
        };
    }
}