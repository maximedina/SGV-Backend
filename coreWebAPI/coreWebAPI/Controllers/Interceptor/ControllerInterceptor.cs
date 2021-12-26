using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MOM.Core.Services;

namespace MOM.Core.WebAPI.Controllers.Interceptor
{
    public class ControllerInterceptor : IActionFilter
    {
        public const string TOKEN_KEY = "sessionToken";

        private readonly SecurityService security;

        public ControllerInterceptor(IServiceProvider services)
        {
            security =  services.GetService<SecurityService>();
        }        

        public void OnActionExecuted(ActionExecutedContext context){ }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            bool methodExclusion = false;
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true);
                methodExclusion = actionAttributes.Any(a => a.GetType() == typeof(SecurityExclusion));
            }
            if(!methodExclusion)
            {
                context.HttpContext.Request.Headers.TryGetValue(TOKEN_KEY, out var tokenArg);
                if(string.IsNullOrEmpty(tokenArg.ToString()) || !security.HasSession(tokenArg.ToString()))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
        }
    }
}