using System;
using System.Net.Http;
using System.Threading.Tasks;
using Linnworks.IntegrationTests.Core;
using LinnworksTest.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Linnworks.IntegrationTests.Tests
{
    public class CategoryEndpointTests
    {
        private readonly HttpClient _client;

        public CategoryEndpointTests()
        {
            var tokenRepo = new Mock<IGenericRepository<Category>>();
            tokenRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                         .ReturnsAsync(new Category
                                       {
                                           Id = Guid.NewGuid(),
                                           CategoryName = "test category"
                                       });
            var testServer = new TestServer(
                                         new WebHostBuilder()
                                             .ConfigureTestServices(s => s.AddScoped(sp => tokenRepo.Object))
                                             .UseStartup<AuthIgnoreTestStartup>());
            _client = testServer.CreateClient();
            _client.DefaultRequestHeaders.Add("ignore-auth", "true");
        }

        [Test]
        public async Task GetDetailsForExistingCategoryShouldReturnModel()
        {
            // ACT
            var responseMessage = await _client.GetAsync("api/category/details/bccf905d-6593-40f1-8db1-c976791fa40a");
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}