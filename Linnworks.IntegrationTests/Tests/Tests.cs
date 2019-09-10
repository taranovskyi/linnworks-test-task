using System;
using System.Threading.Tasks;
using Linnworks.IntegrationTests.Core;
using LinnworksTest;
using LinnworksTest.DataAccess;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Linnworks.IntegrationTests.Tests
{
    public class Tests
    {
        private readonly CustomWebApplicationFactory<Startup> _webApplicationFactory;
        
        public Tests()
        {
            var mockTokenRepo = new Mock<IGenericRepository<Category>>();
            mockTokenRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Category
                {
                    Id = Guid.NewGuid(),
                    CategoryName = "test category"
                });
            _webApplicationFactory = new CustomWebApplicationFactory<Startup>();
            _webApplicationFactory
                .WithWebHostBuilder(b => b.ConfigureServices(s => s.AddScoped(sp => mockTokenRepo.Object)));
        }

        [Test]
        public async Task GetAuth()
        {
            // Arrange
            var client = _webApplicationFactory.CreateClient();
            
            // Act 
            var response = await client.GetAsync("/api/details/123");

            response.EnsureSuccessStatusCode();
        }
    }
}
