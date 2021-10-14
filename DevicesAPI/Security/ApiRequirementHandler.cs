using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevicesAPI.Security
{
    public class ApiRequirementHandler : AuthorizationHandler<ApiRequirement>
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public ApiRequirementHandler(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
            _configuration = configuration;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiRequirement requirement)
        {
            if (_httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var accessToken = authHeader.ToString().Split(' ')[1];

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _client.SetBearerToken(accessToken);
                var baseUrl = _configuration["AuthenticationServer:BaseUrl"];
                var response = await _client
                    .GetAsync($"{baseUrl}/authentication/validate");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
