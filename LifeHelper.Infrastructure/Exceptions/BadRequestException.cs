using System.Net;

namespace LifeHelper.Infrastructure.Exceptions;

public class BadRequestException : CustomException
{
    private const HttpStatusCode StatusCode = HttpStatusCode.BadRequest;
    
    public BadRequestException(string errorMessage) : base(StatusCode, errorMessage) {}
}