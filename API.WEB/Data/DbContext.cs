using System.Data;
using Microsoft.Data.SqlClient;

namespace API.WEB.Data;

public class DbContext
{
    private readonly string _connectionString;

    public DbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
