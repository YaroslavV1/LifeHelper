using LifeHelper.Services.Areas.Helpers.Jwt.DTOs;
using Microsoft.AspNetCore.Http;

namespace LifeHelper.Services.Areas.Helpers.Jwt;

public interface IClaimParserService
{
    public TokenInfoDto ParseInfoFromClaims(HttpContext context);
}