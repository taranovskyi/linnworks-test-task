using System;
using System.Threading.Tasks;
using FluentAssertions;
using Linnworks.IntegrationTests.Core;
using LinnworksTest.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Linnworks.IntegrationTests.Tests
{
    public class CategoriesIntegrationTests : BaseIntegrationTests
    {
        private IGenericRepository<Category> _categoryRepo;

        private Category _category;
        private const string CategoryName = "Integration Test Category";

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            DatabaseOperations.CleanupTable("Categories");
        }

        [SetUp]
        public void Setup()
        {
            _categoryRepo = DatabaseTestStartup.ConfigureServiceProvider()
                                               .GetRequiredService<IGenericRepository<Category>>();
        }

        [Test]
        public async Task CreateAsync_WhenCategoryDoesntExist_SuccessfullyCreate()
        {
            // ARRANGE
            _category = new Category
                        {
                            Id = Guid.NewGuid(),
                            CategoryName = CategoryName
                        };

            // ACT
            var createdCategory = await _categoryRepo.CreateAsync(_category);
            var categoriesFromDb = DatabaseOperations.GetCategoriesById(_category.Id);

            // ASSERT
            createdCategory.Should().BeEquivalentTo(_category);
            categoriesFromDb.Should()
                            .HaveCount(1)
                            .And
                            .Contain(c => c.Id.Equals(_category.Id) && c.CategoryName.Equals(_category.CategoryName));
        }

        [Test]
        public void CreateAsync_WhenCategoryExists_ThrowsException()
        {
            // ARRANGE
            _category = new Category
                        {
                            Id = Guid.NewGuid(),
                            CategoryName = CategoryName
                        };
            AddCategoryToDatabase(_category);

            // ACT and ASSERT
            var exception =
                Assert.ThrowsAsync<DbUpdateException>(async () => await _categoryRepo.CreateAsync(_category));
            var categoriesFromDb = DatabaseOperations.GetCategoriesById(_category.Id);
            var allCategories = DatabaseOperations.GetCategories();

            exception.InnerException?.Message.Should()
                     .Contain("Cannot insert duplicate key in object 'dbo.Categories'");
            allCategories.Should()
                         .HaveCount(1)
                         .And
                         .Contain(c => c.Id.Equals(_category.Id) && c.CategoryName.Equals(_category.CategoryName));
            categoriesFromDb.Should()
                            .HaveCount(1)
                            .And
                            .Contain(c => c.Id.Equals(_category.Id) && c.CategoryName.Equals(_category.CategoryName));
        }

        [Test]
        public async Task GetAsync_WhenCategoryExists_SuccessfullyReturn()
        {
            // ARRANGE
            _category = new Category
                        {
                            Id = Guid.NewGuid(),
                            CategoryName = CategoryName
                        };
            AddCategoryToDatabase(_category);

            // ACT
            var categoryFromRepo = await _categoryRepo.GetByIdAsync(_category.Id);

            // ASSERT
            categoryFromRepo.Should().BeEquivalentTo(_category);
        }

        [Test]
        public async Task GetAsync_WhenCategoryDoesntExist_DontReturn()
        {
            // ACT
            var categoryFromRepo = await _categoryRepo.GetByIdAsync(Guid.NewGuid());

            // ASSERT
            categoryFromRepo.Should().BeNull();
        }

        [Test]
        public async Task UpdateAsync_WhenCategoryExists_SuccessfullyUpdate()
        {
            // ARRANGE
            _category = new Category
                        {
                            Id = Guid.NewGuid(),
                            CategoryName = CategoryName
                        };

            AddCategoryToDatabase(_category);

            // update category name
            _category.CategoryName += " Updated";

            // ACT
            await _categoryRepo.UpdateAsync(_category.Id, _category);
            var categoriesFromDb = DatabaseOperations.GetCategoriesById(_category.Id);
            var allCategories = DatabaseOperations.GetCategories();

            // ASSERT
            allCategories.Should()
                         .HaveCount(1)
                         .And
                         .Contain(c => c.Id.Equals(_category.Id) && c.CategoryName.Equals(_category.CategoryName));
            categoriesFromDb.Should()
                            .HaveCount(1)
                            .And
                            .Contain(c => c.Id.Equals(_category.Id) && c.CategoryName.Equals(_category.CategoryName));
        }

        [Test]
        public void UpdateAsync_WhenCategoryDoesntExist_ThrowsException()
        {
            // ARRANGE
            var category = new Category
                           {
                               Id = Guid.NewGuid(),
                               CategoryName = CategoryName
                           };

            // ACT and ASSERT
            var exception =
                Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
                                                                     await _categoryRepo.UpdateAsync(category.Id,
                                                                                                     category));
            var categoriesFromDb = DatabaseOperations.GetCategoriesById(category.Id);
            var allCategories = DatabaseOperations.GetCategories();

            exception.Message.Should()
                     .Contain(
                              "Database operation expected to affect 1 row(s) but actually affected 0 row(s). Data may have been modified or deleted since entities were loaded.");
            allCategories.Should()
                         .BeEmpty();
            categoriesFromDb.Should()
                            .BeEmpty();
        }

        [Test]
        public async Task DeleteAsync_WhenCategoryExists_SuccessfullyDelete()
        {
            // ARRANGE
            var category = new Category
                           {
                               Id = Guid.NewGuid(),
                               CategoryName = CategoryName
                           };
            AddCategoryToDatabase(category);

            // ACT
            await _categoryRepo.DeleteAsync(category.Id);
            var categoriesFromDb = DatabaseOperations.GetCategoriesById(category.Id);
            var allCategories = DatabaseOperations.GetCategories();

            // ASSERT
            allCategories.Should()
                         .BeEmpty();
            categoriesFromDb.Should()
                            .BeEmpty();
        }

        [Test]
        public void DeleteAsync_WhenCategoryDoesntExist_ThrowsException()
        {
            // ARRANGE
            var category = new Category
                           {
                               Id = Guid.NewGuid(),
                               CategoryName = CategoryName
                           };

            // ACT and ASSERT
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _categoryRepo.DeleteAsync(category.Id));
            var categoriesFromDb = DatabaseOperations.GetCategoriesById(category.Id);
            var allCategories = DatabaseOperations.GetCategories();

            allCategories.Should()
                         .BeEmpty();
            categoriesFromDb.Should()
                            .BeEmpty();
        }

        private void AddCategoryToDatabase(Category category)
        {
            DatabaseOperations.AddCategory(new DatabaseOperations.Models.Category
                                           {
                                               CategoryName = category.CategoryName,
                                               Id = category.Id
                                           });
        }

        [TearDown]
        public void Teardown()
        {
            if (_category != null)
            {
                DatabaseOperations.RemoveCategoryByName(_category.CategoryName);
            }
        }
    }
}