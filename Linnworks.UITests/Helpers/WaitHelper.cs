using System;
using Linnworks.UITests.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Linnworks.UITests.Helpers
{
    public class WaitHelper
    {
        private const int DefaultTimeoutSeconds = 15; 
        
        private static readonly WebDriverWait Wait = new WebDriverWait(DriverManager.Current.Driver, TimeSpan.FromSeconds(DefaultTimeoutSeconds));
        
        public static T Until<T>(Func<IWebDriver, T> condition, int seconds = DefaultTimeoutSeconds)
        {
            Wait.Timeout = TimeSpan.FromSeconds(seconds);
            var result = Wait.Until(condition);
            Wait.Timeout = TimeSpan.FromSeconds(DefaultTimeoutSeconds);
            return result;
        }
    }
}
