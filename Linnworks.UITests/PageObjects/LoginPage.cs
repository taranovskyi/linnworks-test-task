using OpenQA.Selenium;

namespace Linnworks.UITests.PageObjects
{
    public class LoginPage : BasePage
    {
        private IWebElement TokenField => Driver.FindElement(By.Id("token"));
        private IWebElement LoginBtn => Driver.FindElement(By.XPath("//button[text() = 'Login']"));
        private IWebElement ErrorBlock => Driver.FindElement(By.XPath("//div[@role='alert']"));

        public LoginPage SetToken(string value)
        {
            var driverTitle = Driver.Title;
            var driverPageSource = Driver.PageSource;
            var driverUrl = Driver.Url;
            TokenField.SendKeys(value);
            return this;
        }

        public void PressLogin()
        {
            LoginBtn.Click();
        }

        public string GetTextFromErrorBlock()
        {
            return ErrorBlock.FindElement(By.XPath(".//li")).Text;
        }
    }
}
