using System.Net;

namespace LifeHelper.Services.Exceptions;

public class NotFoundException : CustomException
{
    private const HttpStatusCode StatusCode = HttpStatusCode.NotFound;
    
    public NotFoundException(string errorMessage) : base(StatusCode, errorMessage) { }
}