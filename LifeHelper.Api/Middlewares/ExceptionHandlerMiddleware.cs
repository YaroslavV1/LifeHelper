﻿using System.Net;
using System.Net.Mime;
using LifeHelper.Api.Models;
using LifeHelper.Services.Exceptions;

namespace LifeHelper.Api.Middlewares;

public class ExceptionHandlerMiddleware
{
    private const HttpStatusCode InternalServerError = HttpStatusCode.InternalServerError;
    
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
            await HandleExceptionAsync(e, context);
        }
    }

    private async Task HandleExceptionAsync(Exception exception, HttpContext context)
    {
        _logger.LogError(exception.Message);

        context.Response.ContentType = MediaTypeNames.Application.Json;

        var errorModel = new ErrorModel();
        if (exception is CustomException customException)
        {
            context.Response.StatusCode = (int)customException.StatusCode;
            errorModel.ErrorMessage = customException.Message;
        }
        else
        {
            context.Response.StatusCode = (int)InternalServerError;
            errorModel.ErrorMessage = InternalServerError.ToString();
        }

        await context.Response.WriteAsJsonAsync(errorModel);
    }
}