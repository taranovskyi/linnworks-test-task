using System;
using Linnworks.UITests.Core;
using Linnworks.UITests.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Linnworks.UITests.Tests
{
    public class BaseTest
    {
        protected BaseTest()
        {
            DriverManager.Current.Driver.Navigate().GoToUrl("about:blank");
            DriverManager.Current.Driver.Navigate().GoToUrl(Constants.ServiceUrl);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DriverManager.Current.Driver.Quit();
            DriverManager.Current.Dispose();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            {
                Console.WriteLine("Saving screenshot for failed test");
                ScreenshotHelper.TakeScreenshot();
            }

            DriverManager.Current.Driver.Manage().Cookies.DeleteAllCookies();
            DriverManager.Current.Driver.Navigate().GoToUrl(Constants.ServiceUrl);
        }
    }
}
