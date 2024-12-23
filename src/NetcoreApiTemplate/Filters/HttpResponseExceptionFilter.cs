using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetcoreApiTemplate.Models.Dtos.Exceptions;

namespace NetcoreApiTemplate.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }


        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is HttpResponseException exception)
            {
                HttpResponseException ex = exception;
                context.Result = new ObjectResult(exception.GetValue())
                {
                    StatusCode = exception.GetStatusCode(),
                };
                context.ExceptionHandled = true;
            }
            else if (context.Exception is Exception)
            {
                HttpResponseException exx = new HttpResponseException().InternalServerError()
                    .Type("ERROR")
                    .Code("")
                    .Details($"{context.Exception?.Message}")
                    .MoreDetails($"{context.Exception?.InnerException?.Message}");
                context.Result = new ObjectResult(exx.GetValue())
                {
                    StatusCode = exx.GetStatusCode(),
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
