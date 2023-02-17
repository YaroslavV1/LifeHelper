using System.Net;
using System.Net.Mime;
using LifeHelper.Api.Models;
using LifeHelper.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace LifeHelper.Api.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await ExceptionAsync(e, context);
        }
    }

    private async Task ExceptionAsync(Exception exception, HttpContext context)
    {
        _logger.LogError(exception.Message);

        context.Response.ContentType = MediaTypeNames.Application.Json;

        var errorModel = new ErrorModel();
        if (exception is CustomException customException)
        {
            context.Response.StatusCode = (int)customException.StatusCode;
            errorModel.Message = customException.Message;
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            errorModel.Message = "Internal Server Error";
        }

        await context.Response.WriteAsJsonAsync(errorModel);
    }
}