using System;
using OpenQA.Selenium;

namespace Linnworks.UITests.PageObjects
{
    public class NavigationBar : BasePage
    {
        private IWebElement MainElement => Driver.FindElement(By.ClassName("main-nav"));

        public T GoTo<T>(string page) where T : BasePage, new()
        {
            MainElement.FindElement(By.XPath($".//a[text()[contains(., '{page}')]]")).Click();

            return Activator.CreateInstance<T>();
        }
    }
}
