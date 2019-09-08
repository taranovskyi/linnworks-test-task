using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Linnworks.UITests.Core
{
    public class DriverManager
    {
        private static DriverManager _instance;
        public readonly IWebDriver Driver;

        private DriverManager()
        {
            Driver = new RemoteWebDriver(new Uri("http://selenium:4444/wd/hub"), new ChromeOptions());
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            Driver.Manage().Window.Maximize(); 
        }
        
        public static DriverManager Current => _instance ?? (_instance = new DriverManager());

        public void Dispose()
        {
            _instance = null;
        }
    }
}
