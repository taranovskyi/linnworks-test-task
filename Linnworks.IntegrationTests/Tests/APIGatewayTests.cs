using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Linnworks.IntegrationTests.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace Linnworks.IntegrationTests.Tests
{
    public class ApiGatewayTests
    {
        private readonly TestServer _testServer;

        public ApiGatewayTests()
        {
            _testServer = new TestServer(
                WebHost.CreateDefaultBuilder()
                    .UseStartup<TestStartup>());
        }

        [Test]
        public async Task GetAuth()
        {
            // Arrange
            var client = _testServer.CreateClient();

            // Act 
            HttpResponseMessage responseMessage;
            using (var content = new StringContent("{\"token\":\"bccf905d-6593-40f1-8db1-c976791fa40a\"}", Encoding.UTF8, "application/json"))
            {
                responseMessage = await client.PostAsync(client.BaseAddress + "api/auth/login", content);
                responseMessage.EnsureSuccessStatusCode();
            }

            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            //responseMessage.Headers.TryGetValues("");
        }
    }
}
