using DevicesAPI.Security;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DevicesAPI.Test
{
    public class TestApiRequirementHandler : AuthorizationHandler<ApiRequirement>
    {
        private readonly HttpContext _httpContext;
        public TestApiRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiRequirement requirement)
        {
            if (_httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var accessToken = authHeader.ToString().Split(' ')[1];

                if (!string.IsNullOrEmpty(accessToken) && String.Compare(accessToken, Config.Access_Token, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    context.Succeed(requirement);
                }
            }

        }
    }
}
