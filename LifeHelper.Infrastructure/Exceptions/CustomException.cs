using System.Net;

namespace LifeHelper.Infrastructure.Exceptions;

public class CustomException : Exception
{
    public HttpStatusCode StatusCode { get; }

    protected CustomException(HttpStatusCode statusCode, string errorMessage) : base(errorMessage)
    {
        StatusCode = statusCode;
    }
}