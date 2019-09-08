using Linnworks.UITests.Core;
using OpenQA.Selenium;

namespace Linnworks.UITests.PageObjects
{
    public class BasePage
    {
        protected readonly IWebDriver Driver;

        protected BasePage()
        {
            Driver = DriverManager.Current.Driver;
        }
    }
}
