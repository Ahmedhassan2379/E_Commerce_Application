using E_Commerce.Api.Errors;
using Microsoft.OpenApi.Exceptions;
using System.Net;
using System.Text.Json;

namespace E_Commerce.Api.MiddleWare
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public ExceptionMiddleWare(RequestDelegate requestDelegate,ILogger<ExceptionMiddleWare> logger,IHostEnvironment hostEnvironment) 
        { 
            _requestDelegate = requestDelegate;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
                _logger.LogInformation("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"This Error Come From Exception Midlleware {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = _hostEnvironment.IsDevelopment() ? new ApiExecption((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) 
                                     : new ApiExecption((int)HttpStatusCode.InternalServerError) ;
                var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response,option);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
