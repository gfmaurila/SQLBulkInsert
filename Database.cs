using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace SQLBulkInsert;

public static class Database
{
    public static void EnsureDatabaseSetup(string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var cmdText = @"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
            BEGIN
                CREATE TABLE Users (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    FirstName VARCHAR(100),
                    LastName VARCHAR(100),
                    DateOfBirth DATE,
                    IsComplete BIT DEFAULT 1 -- Assume que por padrão os registros são completos
                )
            END

            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'InvalidLogs')
            BEGIN
                CREATE TABLE InvalidLogs (
                    LogId INT PRIMARY KEY IDENTITY(1,1),
                    UserId INT,
                    InvalidField VARCHAR(100),
                    InvalidValue VARCHAR(255),
                    TimeStamp DATETIME DEFAULT GETDATE()
                )
            END";
            using (var command = new SqlCommand(cmdText, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public static List<User> LoadUsersFromTxt(string filePath, SqlConnection connection)
    {
        var users = new List<User>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split('|');
            bool isComplete = true;
            string invalidField = "";
            string invalidValue = "";

            if (parts.Length == 4)
            {
                DateTime dateOfBirth;
                bool dateValid = DateTime.TryParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth);

                var user = new User
                {
                    FirstName = parts[1],
                    LastName = parts[2],
                    DateOfBirth = dateValid ? dateOfBirth : DateTime.MinValue,
                    IsComplete = dateValid
                };

                if (!dateValid)
                {
                    isComplete = false;
                    invalidField = "DateOfBirth";
                    invalidValue = parts[3];
                }

                // Adiciona usuário à lista
                users.Add(user);

                // Se não estiver completo, registra no log
                if (!isComplete)
                {
                    using (var cmd = new SqlCommand("INSERT INTO InvalidLogs (UserId, InvalidField, InvalidValue) VALUES (@UserId, @InvalidField, @InvalidValue)", connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", parts[0]);
                        cmd.Parameters.AddWithValue("@InvalidField", invalidField);
                        cmd.Parameters.AddWithValue("@InvalidValue", invalidValue);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        return users;
    }


    public static void BulkInsertUsers(string connectionString, List<User> users)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.DestinationTableName = "dbo.Users";

                // Mapeamento explícito de colunas para garantir a ordem correta
                bulkCopy.ColumnMappings.Add("FirstName", "FirstName");
                bulkCopy.ColumnMappings.Add("LastName", "LastName");
                bulkCopy.ColumnMappings.Add("DateOfBirth", "DateOfBirth");
                bulkCopy.ColumnMappings.Add("IsComplete", "IsComplete");

                var table = new DataTable();
                table.Columns.Add("FirstName", typeof(string));
                table.Columns.Add("LastName", typeof(string));
                table.Columns.Add("DateOfBirth", typeof(DateTime));
                table.Columns.Add("IsComplete", typeof(bool));

                foreach (var user in users)
                {
                    DataRow row = table.NewRow();
                    row["FirstName"] = user.FirstName;
                    row["LastName"] = user.LastName;
                    row["DateOfBirth"] = user.DateOfBirth;  // Assegure-se que este seja um DateTime
                    row["IsComplete"] = user.IsComplete;  // Assegure-se que este seja um Boolean
                    table.Rows.Add(row);
                }

                bulkCopy.WriteToServer(table);
            }
        }
    }
}

