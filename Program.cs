using Microsoft.Data.SqlClient;

namespace SQLBulkInsert;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=localhost,1433;Initial Catalog=SQLBulkInsert;User ID=SA;Password=@G14u12i2024;Encrypt=false;TrustServerCertificate=true";

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            Database.EnsureDatabaseSetup(connectionString);

            var filePath = @"C:\Work\gfmaurila\GitHub\SQLBulkInsert\User.txt";
            var users = Database.LoadUsersFromTxt(filePath, connection);

            Database.BulkInsertUsers(connectionString, users);
            Console.WriteLine("Dados inseridos com sucesso.");
        }
    }
}