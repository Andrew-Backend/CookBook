using Microsoft.Data.SqlClient;

namespace DAL.Connection;

public class DbConnection
{
    private readonly string _connectionString;

    public DbConnection(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}