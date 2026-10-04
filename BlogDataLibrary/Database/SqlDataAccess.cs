using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BlogDataLibrary.Database;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionStringName, bool isStoredProcedure)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);
        CommandType commandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            List<T> rows = connection.Query<T>(sqlStatement, parameters, commandType: commandType).ToList();
            return rows;
        }
    }

    public void SaveData<T>(string sqlStatement, T parameters, string connectionStringName, bool isStoredProcedure)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);
        CommandType commandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            connection.Execute(sqlStatement, parameters, commandType: commandType);
        }
    }
}