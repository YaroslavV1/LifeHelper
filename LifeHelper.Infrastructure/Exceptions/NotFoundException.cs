using System.Net;

namespace LifeHelper.Infrastructure.Exceptions;

public class NotFoundException : CustomException
{
    private const HttpStatusCode StatusCode = HttpStatusCode.NotFound;
    
    public NotFoundException( string errorMessage) : base(StatusCode, errorMessage) {}
}