using System.Net;

namespace LifeHelper.Infrastructure.Exceptions;

public class NotFoundException : CustomException
{
    private const HttpStatusCode StatusCode = HttpStatusCode.NotFound;
    
    public NotFoundException( string message) : base(StatusCode, message) {}
}