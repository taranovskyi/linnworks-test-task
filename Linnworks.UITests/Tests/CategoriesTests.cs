using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Linnworks.DatabaseOperations.Models;
using Linnworks.UITests.Core;
using Linnworks.UITests.Helpers;
using Linnworks.UITests.PageObjects;
using NUnit.Framework;

namespace Linnworks.UITests.Tests
{
    public class CategoriesTests : BaseTest
    {
        private readonly RestApiHelper _restApiHelper;
        private readonly DatabaseOperations.Core.DatabaseOperations _databaseOperations;
        private CategoriesPage _categoriesPage;
        private string _categoryToDelete;

        private const string CategoryName = "UI Test Category";
        private const string CategoryNameUpdated = "UI Test Category Updated";

        public CategoriesTests()
        {
            _restApiHelper = new RestApiHelper();
            _categoriesPage = new CategoriesPage();
            _databaseOperations = new DatabaseOperations.Core.DatabaseOperations();
        }

        [SetUp]
        public async Task Setup()
        {
            // ARRANGE
            await _restApiHelper.FastLoginAsync();
            DriverManager.Current.Driver.Navigate().GoToUrl(Constants.ServiceUrl + "/fetch-category");
        }

        [Test]
        public void AddedCategoryShouldBeDisplayedOnCategoriesPage()
        {
            // ACT
            var addCategoryPage = _categoriesPage.GoToAddCategoryPage();
            _categoriesPage = addCategoryPage.AddOrUpdateCategory(CategoryName);
            _categoryToDelete = CategoryName;

            // ASSERT
            _categoriesPage.IsCategoryRowDisplayed(CategoryName).Should()
                           .BeTrue();
        }

        [Test]
        public void UpdatedCategoryShouldBeDisplayedOnCategoriesPage()
        {
            // ARRANGE
            AddCategoryViaDb();

            // ACT
            var updateCategoryPage = _categoriesPage.GoToUpdateCategoryPage(CategoryName);
            _categoriesPage = updateCategoryPage.AddOrUpdateCategory(CategoryNameUpdated);
            _categoryToDelete = CategoryNameUpdated;

            // ASSERT
            _categoriesPage.IsCategoryRowDisplayed(CategoryNameUpdated).Should()
                           .BeTrue();
        }

        [Test]
        public void DeletedCategoryShouldNotBeDisplayedOnCategoriesPage()
        {
            // ARRANGE
            AddCategoryViaDb();

            // ACT
            _categoriesPage = _categoriesPage.DeleteCategory(CategoryName);

            // ASSERT
            WaitHelper.Until(dr => !_categoriesPage.TryGetCategoryRow(CategoryName, out _)).Should()
                      .BeTrue();
        }

        [TearDown]
        public void Teardown()
        {
            if (_databaseOperations.GetCategories().ToList().Any(c => c.CategoryName.Equals(_categoryToDelete)))
            {
                _databaseOperations.RemoveCategoryByName(_categoryToDelete);
            }
        }

        private void AddCategoryViaDb()
        {
            _databaseOperations.AddCategory(new Category
                                            {
                                                Id = Guid.NewGuid(),
                                                CategoryName = CategoryName
                                            });
            DriverManager.Current.Driver.Navigate().Refresh();
        }
    }
}