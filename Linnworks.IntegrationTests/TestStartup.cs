using System;
using LinnworksTest;
using LinnworksTest.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Linnworks.IntegrationTests.Core
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureDatabase(IServiceCollection services)
        {
            var mockTokenRepo = new Mock<ITokenRepository>();
            mockTokenRepo.Setup(r => r.IsValidTokenAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            services.AddScoped(sp => mockTokenRepo.Object);
        }
    }
}
