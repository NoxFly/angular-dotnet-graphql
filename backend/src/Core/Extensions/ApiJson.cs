using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class ApiJsonControllerAttribute : ApiControllerAttribute, IFilterFactory
{
    public bool IsReusable => true;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return new ApiJsonControllerFilter();
    }
}

public class ApiJsonControllerFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var request = context.HttpContext.Request;

        if(!request.Headers.Accept.ToString().Contains("application/json"))
        {
            context.Result = new ObjectResult("Accept header must be application/json")
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) {}
}
