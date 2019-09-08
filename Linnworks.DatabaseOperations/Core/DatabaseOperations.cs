using System;
using System.Collections.Generic;
using Dapper;
using Linnworks.DatabaseOperations.Models;

namespace Linnworks.DatabaseOperations.Core
{
    public class DatabaseOperations
    {
        public void AddToken(Guid token)
        {
            using (var connection = new DatabaseConnectionGenerator().Connection)
            {
                connection.Query($"INSERT INTO Tokens (ID, Value) Values ('{Guid.NewGuid().ToString()}', '{token.ToString()}')");
            }
        }
        
        public void RemoveTokenByValue(Guid token)
        {
            using (var connection = new DatabaseConnectionGenerator().Connection)
            {
                connection.Query($"DELETE FROM Tokens WHERE Value = '{token.ToString()}'");
            }
        }
        
        public IEnumerable<Category> GetCategoriesById(Guid id)
        {
            using (var connection = new DatabaseConnectionGenerator().Connection)
            {
                return  connection.Query<Category>($"SELECT * FROM Categories WHERE ID = '{id.ToString()}'");
            }
        }
        
        public IEnumerable<Category> GetCategories()
        {
            using (var connection = new DatabaseConnectionGenerator().Connection)
            {
                return  connection.Query<Category>($"SELECT * FROM Categories");
            }
        }
        
        public void RemoveCategoryByName(string categoryName)
        {
            using (var connection = new DatabaseConnectionGenerator().Connection)
            {
                connection.Query($"DELETE FROM Categories WHERE CategoryName = '{categoryName}'");
            }
        }
        
        public void AddCategory(Category category)
        {
            using (var connection = new DatabaseConnectionGenerator().Connection)
            {
                connection.Query($"INSERT INTO Categories (ID, CategoryName) Values ('{category.Id.ToString()}', '{category.CategoryName}')");
            }
        }

        public void CleanupTable(string tableName)
        {
            using (var connection = new DatabaseConnectionGenerator().Connection)
            {
                connection.Query($"DELETE FROM {tableName}");
            }
        }
    }
}
