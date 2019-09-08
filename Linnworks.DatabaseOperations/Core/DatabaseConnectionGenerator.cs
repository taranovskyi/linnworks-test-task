using System.Data;
using System.Data.SqlClient;

namespace Linnworks.DatabaseOperations.Core
{
    public class DatabaseConnectionGenerator
    {
        public IDbConnection Connection
        {
            get
            {
                var connection = new SqlConnection("Server=mssql,1433;Database=Linnworks.TestDb;User Id=sa;Password=yourStrong(!)Password;Integrated Security=False");
                connection.Open();
                return connection;
            }
        }
    }
}
