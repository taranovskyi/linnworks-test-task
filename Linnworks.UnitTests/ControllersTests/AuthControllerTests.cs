using System;
using System.Collections;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using LinnworksTest.Controllers;
using LinnworksTest.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Linnworks.UnitTests.ControllersTests
{
    public class AuthControllerTests
    {
        private AuthController _authController;
        private readonly Mock<ITokenRepository> _mockTokenRepository;

        private readonly AuthController.Account _account = new AuthController.Account
        {
            Token = Guid.NewGuid().ToString()
        };

        public AuthControllerTests()
        {
            _mockTokenRepository = new Mock<ITokenRepository>();
        }

        [TestCaseSource(typeof(TestDataClass), nameof(TestDataClass.InvalidAccounts))]
        public async Task Login_WhenInvalidAccount_ReturnsBadRequest(AuthController.Account account)
        {
            // ARRANGE
            _authController = new AuthController(_mockTokenRepository.Object);

            // ACT
            var result = await _authController.Login(account);

            // ASSERT
            AssertThatTokenIsInvalid(result);
        }

        [Test]
        public async Task Login_WhenTokenIsNotPresentInDb_ReturnsBadRequest()
        {
            // ARRANGE
            _mockTokenRepository
                .Setup(r => r.IsValidTokenAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);
            _authController = new AuthController(_mockTokenRepository.Object);

            // ACT
            var result = await _authController.Login(_account);

            // ASSERT
            AssertThatTokenIsInvalid(result);
        }

        [Test]
        public async Task Login_WhenAccountValid_ReturnsOkObjectResult()
        {
            // ARRANGE
            var configuredAuthController = ConfigureAuthController(_authController);

            // ACT
            var result = await configuredAuthController.Login(_account);

            // ASSERT
            configuredAuthController.ModelState.IsValid.Should()
                .BeTrue();
            configuredAuthController.ModelState.ErrorCount.Should()
                .Be(0);
            result.Should()
                .BeOfType<OkObjectResult>();
        }

        private AuthController ConfigureAuthController(AuthController authController)
        {
            var httpContextMock = new Mock<HttpContext>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            httpContextMock.Setup(c => c.RequestServices)
                .Returns(serviceProviderMock.Object);
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IAuthenticationService)))
                .Returns(authenticationServiceMock.Object);
            authenticationServiceMock.Setup(a => a.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);
            _mockTokenRepository
                .Setup(r => r.IsValidTokenAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            return new AuthController(_mockTokenRepository.Object) {ControllerContext = new ControllerContext {HttpContext = httpContextMock.Object}};
        }

        private void AssertThatTokenIsInvalid(IActionResult result)
        {
            _authController.ModelState.IsValid.Should()
                .BeFalse();
            _authController.ModelState.ErrorCount.Should()
                .Be(1);
            _authController.ModelState["login_failure"].Errors.Should()
                .Contain(e => e.ErrorMessage.Equals("Invalid token."));
            result.Should()
                .BeOfType<BadRequestObjectResult>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockTokenRepository.Reset();
        }
    }

    public class TestDataClass
    {
        public static IEnumerable InvalidAccounts
        {
            get
            {
                yield return null;
                yield return new AuthController.Account {Token = string.Empty};
                yield return new AuthController.Account {Token = "test"};
            }
        }
    }
}
