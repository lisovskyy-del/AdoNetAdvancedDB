using Microsoft.Data.SqlClient;
using System.Data;

namespace MainProgram.Levels;

class Level1
{
    public static void Run(SqlConnection connection)
    {
        Console.WriteLine(Login(connection));

        using SqlCommand cmd = new SqlCommand("SELECT Description FROM Products", connection);
        using SqlDataReader reader = cmd.ExecuteReader();

        PrintProducts(reader);
    }

    public static bool Login(SqlConnection connection)
    {
        string username = InputHelpers.StringInput("\nEnter username: ");
        string password = InputHelpers.StringInput("\nEnter password: ");

        using SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password", connection);

        cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username;
        cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 100).Value = password;

        int count = (int)cmd.ExecuteScalar();
        return count > 0;
    }

    public static void PrintProducts(SqlDataReader reader)
    {
        int descOrdinal = reader.GetOrdinal("Description");

        while (reader.Read())
        {
            string? description = reader.GetNullableString(descOrdinal);
            Console.WriteLine($"Description: {description}");
        }
    }
}