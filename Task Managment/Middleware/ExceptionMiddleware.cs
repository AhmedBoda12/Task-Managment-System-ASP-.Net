using System.Net;

namespace Task_Managment.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {


                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is UnauthorizedAccessException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else if (exception is KeyNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            var errorDetails = new
            {
                context.Response.StatusCode,
                Message = exception switch
                {
                    UnauthorizedAccessException => "Unauthorized access.",
                    KeyNotFoundException => "Resource not found.",
                    _ => "An unexpected error occurred. Please check the logs for more details."
                }
            };


            return context.Response.WriteAsJsonAsync(errorDetails);
        }
    }
}
