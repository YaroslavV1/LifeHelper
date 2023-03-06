using System.Security.Claims;
using LifeHelper.Services.Areas.Helpers.Jwt.DTOs;
using Microsoft.AspNetCore.Http;

namespace LifeHelper.Services.Areas.Helpers.Jwt;

public interface IClaimService
{
    public TokenInfoDto GetParsedInfo(HttpContext context);
}