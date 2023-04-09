using System.Security.Claims;
using LifeHelper.Services.Exceptions;
using LifeHelper.Services.Utilities.DTOs;
using Microsoft.AspNetCore.Http;

namespace LifeHelper.Services.Utilities;

public static class LifeHelperUtilities
{
    public static void ThrowIfNotFound(this object? o, int id)
    {
        if (o == null)
        {
            throw new NotFoundException($"Object with id: {id} not found");
        }
    }
 
    public static void ThrowIfNotFound(this object? o, string errorMessage)
    {
        if (o == null)
        {
            throw new NotFoundException(errorMessage);
        }
    }
    
    public static TokenInfoDto ParseInfoFromClaims(HttpContext context)
    {
        return new TokenInfoDto
        {
            Id = Convert.ToInt32(context.User.FindFirstValue(ClaimTypes.NameIdentifier)),
            Role = Convert.ToString(context.User.FindFirstValue(ClaimTypes.Role))
        };
    }
}