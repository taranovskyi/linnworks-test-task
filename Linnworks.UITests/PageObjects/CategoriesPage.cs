using System;
using System.Linq;
using OpenQA.Selenium;

namespace Linnworks.UITests.PageObjects
{
    public class CategoriesPage : BasePage
    {
        private const string EditCategoryLinkXPath = ".//a[text() = 'Edit']";
        private const string DeleteCategoryLinkXPath = ".//a[text() = 'Delete']";
        
        private IWebElement CategoriesLbl => Driver.FindElement(By.XPath("//app-fetch-category/h1[text() = 'Categories']"));
        private IWebElement CreateNewCategoryLink => Driver.FindElement(By.XPath("//a[text() = 'Create New']"));
        private IWebElement CategoriesTable => Driver.FindElement(By.TagName("table"));
        
        public AddCategoryPage GoToAddCategoryPage()
        {
            CreateNewCategoryLink.Click();
            return new AddCategoryPage();
        }
        
        public bool IsCategoriesLblDisplayed()
        {
            return CategoriesLbl.Displayed;
        }

        public CategoriesPage DeleteCategory(string category)
        {
            var row = GetCategoryRow(category);
            row.FindElement(By.XPath(DeleteCategoryLinkXPath)).Click();
            Driver.SwitchTo().Alert().Accept();
            return this;
        }

        public EditCategoryPage GoToUpdateCategoryPage(string category)
        {
            var row = GetCategoryRow(category);
            row.FindElement(By.XPath(EditCategoryLinkXPath)).Click();
            return new EditCategoryPage();
        }

        public bool IsCategoryRowDisplayed(string category)
        {
            var row = GetCategoryRow(category);

            return row.Displayed
                   && row.FindElement(By.XPath("./td[text() = '0']")).Displayed
                   && row.FindElement(By.XPath(EditCategoryLinkXPath)).Displayed
                   && row.FindElement(By.XPath(DeleteCategoryLinkXPath)).Displayed;
        }
        
        private IWebElement GetCategoryRow(string category)
        {
            if (TryGetCategoryRow(category, out var row))
            {
                return row;
            }
            throw new ArgumentException($"Can't get row for '{category}' category!");
        }
        
        public bool TryGetCategoryRow(string category, out IWebElement row)
        {
            var elements = CategoriesTable.FindElements(By.XPath($".//tr[./td[text() = '{category}']]"));
            row = null;
            if (!elements.Any()) 
                return false;
            row = elements.FirstOrDefault();
            return true;
        }
    }
}
