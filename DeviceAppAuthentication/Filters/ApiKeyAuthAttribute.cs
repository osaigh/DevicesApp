﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceAppAuthentication.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var requestApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = config.GetValue<string>(key: "ApiKey");

            if (String.Compare(requestApiKey,apiKey, StringComparison.OrdinalIgnoreCase) != 0)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
