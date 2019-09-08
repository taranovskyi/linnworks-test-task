using FluentAssertions;
using Linnworks.UITests.Core;
using Linnworks.UITests.PageObjects;
using NUnit.Framework;

namespace Linnworks.UITests.Tests
{
    public class LoginTests : BaseTest
    {
        private readonly LoginPage _loginPage;
        private readonly CategoriesPage _categoriesPage;

        public LoginTests()
        {
            _categoriesPage = new CategoriesPage();
            _loginPage = new LoginPage();
        }

        [SetUp]
        public void Setup()
        {
            // ARRANGE
            DriverManager.Current.Driver.Navigate().GoToUrl(Constants.ServiceUrl + "/login");
        }

        [Test]
        public void SuccessfulLoginShouldRedirectToCategoriesPage()
        {
            // ACT
            _loginPage
                .SetToken(Constants.Token)
                .PressLogin();

            // ASSERT
            _categoriesPage.IsCategoriesLblDisplayed().Should()
                .BeTrue();
        }
        
        [Test]
        public void FailedLoginShouldDisplayErrorMessage()
        {
            // ACT
            _loginPage
                .SetToken("bccf905c")
                .PressLogin();

            // ASSERT
            _loginPage.GetTextFromErrorBlock().Should()
                .BeEquivalentTo("Invalid token.");
        }
    }
}
