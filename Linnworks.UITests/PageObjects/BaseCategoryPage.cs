using OpenQA.Selenium;

namespace Linnworks.UITests.PageObjects
{
    public class BaseCategoryPage : BasePage
    {
        private IWebElement CategoryNameField => Driver.FindElement(By.XPath("//input[@formcontrolname='categoryName']"));
        private IWebElement SaveBtn => Driver.FindElement(By.XPath("//button[text() = 'Save']"));
        
        public CategoriesPage AddOrUpdateCategory(string category)
        {
            CategoryNameField.Clear();
            CategoryNameField.SendKeys(category);
            SaveBtn.Click();
            return new CategoriesPage();
        }
    }
}
