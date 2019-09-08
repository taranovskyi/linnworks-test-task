using System;
using System.Threading.Tasks;
using FluentAssertions;
using Linnworks.IntegrationTests.Core;
using LinnworksTest.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Linnworks.IntegrationTests.Tests
{
    public class AuthIntegrationTests : BaseIntegrationTests
    {
        private ITokenRepository _tokenRepository;
        private Guid _token;
        
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            DatabaseOperations.CleanupTable("Tokens");
        }

        [SetUp]
        public void Setup()
        {
            _tokenRepository = DatabaseTestStartup.ConfigureServiceProvider().GetRequiredService<ITokenRepository>();
        }

        [Test]
        public async Task IsValidTokenAsync_WhenTokenIsPresentInDatabase_ReturnsTrue()
        {
            //ARRANGE
            _token = Guid.NewGuid();
            DatabaseOperations.AddToken(_token);
            
            // ACT
            var isTokenValid = await _tokenRepository.IsValidTokenAsync(_token);
            
            // ASSERT
            isTokenValid.Should().BeTrue();
        }
        
        [Test]
        public async Task IsValidTokenAsync_WhenTokenIsNotPresentInDatabase_ReturnsFalse()
        {
            // ACT
            var isTokenValid = await _tokenRepository.IsValidTokenAsync(Guid.NewGuid());
            
            // ASSERT
            isTokenValid.Should().BeFalse();
        }

        [TearDown]
        public void Teardown()
        {
            if (_token != Guid.Empty)
            {
                DatabaseOperations.RemoveTokenByValue(_token);
            }
        }
    }
}
