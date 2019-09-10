using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LinnworksTest;
using LinnworksTest.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Linnworks.IntegrationTests.Tests
{
    public class AuthenticationEndpointTests
    {
        private const string SetCookieHeaderKey = "Set-Cookie";
        private readonly HttpClient _client;

        public AuthenticationEndpointTests()
        {
            var tokenRepo = new Mock<ITokenRepository>();
            tokenRepo.Setup(r => r.IsValidTokenAsync(It.IsAny<Guid>()))
                         .ReturnsAsync(false);
            tokenRepo.Setup(r => r.IsValidTokenAsync(new Guid("bccf905d-6593-40f1-8db1-c976791fa40a")))
                         .ReturnsAsync(true);
            var testServer = new TestServer(
                                         new WebHostBuilder()
                                             .ConfigureTestServices(s => s.AddScoped(sp => tokenRepo.Object))
                                             .UseStartup<Startup>());
            _client = testServer.CreateClient();
        }

        [Test]
        public async Task SuccessfulAuthenticationShouldReturnCookie()
        {
            // ACT 
            HttpResponseMessage responseMessage;
            using (var content = new StringContent("{\"token\":\"bccf905d-6593-40f1-8db1-c976791fa40a\"}",
                                                   Encoding.UTF8, "application/json"))
            {
                responseMessage = await _client.PostAsync( "api/auth/login", content);
                responseMessage.EnsureSuccessStatusCode();
            }
            
            // ASSERT
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            responseMessage.Headers.TryGetValues(SetCookieHeaderKey, out var cookies);
            var cookie = cookies?.FirstOrDefault();
            cookie.Should().StartWith(".AspNetCore.Cookies");
        }

        [Test]
        public async Task GetAuth1()
        {
            // ACT 
            HttpResponseMessage responseMessage;
            using (var content = new StringContent("{\"token\":\"bccf905d-6592-40f1-8db1-c976791fa40a\"}",
                                                   Encoding.UTF8, "application/json"))
            {
                responseMessage = await _client.PostAsync( "api/auth/login", content);
            }

            // ASSERT
            responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseMessage.Headers.TryGetValues(SetCookieHeaderKey, out _).Should().BeFalse();
        }
    }
}