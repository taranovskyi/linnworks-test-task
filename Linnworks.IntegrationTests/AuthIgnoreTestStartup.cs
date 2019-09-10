using Linnworks.IntegrationTests.Core.Auth;
using LinnworksTest;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Linnworks.IntegrationTests.Core
{
    public class AuthIgnoreTestStartup : Startup
    {
        public AuthIgnoreTestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseMiddleware<AuthenticatedTestRequestMiddleware>();
        }
    }
}