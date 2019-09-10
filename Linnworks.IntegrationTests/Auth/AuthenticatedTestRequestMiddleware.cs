using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Linnworks.IntegrationTests.Core.Auth
{
    public class AuthenticatedTestRequestMiddleware
    {
        private const string TestingHeader = "ignore-auth";

        private readonly RequestDelegate _next;

        public AuthenticatedTestRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.Keys.Contains(TestingHeader) &&
                context.Request.Headers[TestingHeader].First().Equals("true"))
            {
                var claimsIdentity = new ClaimsIdentity(new List<Claim>
                                                        {
                                                            new Claim(ClaimTypes.Name, Guid.NewGuid().ToString())
                                                        }, "test-cookie");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                context.User = claimsPrincipal;
            }

            await _next(context);
        }
    }
}