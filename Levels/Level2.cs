using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.Common;

namespace MainProgram.Levels;

internal class Level2
{
    private readonly DbProviderFactory _factory;

    public Level2(DbProviderFactory factory)
    {
        _factory = factory;
    }

    public static void Run(DbProviderFactory factory, string connectionString, SqlConnection connection)
    {
        Level2 level2 = new Level2(factory);
        TransferMoney(connection);
        level2.ConnectToDatabase(connectionString);
    }

    public static void TransferMoney(SqlConnection connection)
    {
        int fromAccount = InputHelpers.IntInput("\nEnter an id to withdraw: ");
        int toAccount = InputHelpers.IntInput("\nEnter an id to deposit: ");
        decimal amount = InputHelpers.DecimalInput("\nEnter an amount: ");

        SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

        try
        {
            using SqlCommand cmdWithdraw = new SqlCommand(@"UPDATE Accounts SET Balance = Balance - @amount WHERE Id = @from", connection, transaction);

            cmdWithdraw.Parameters.Add("@amount", SqlDbType.Decimal).Value = amount;
            cmdWithdraw.Parameters.Add("@from", SqlDbType.Int).Value = fromAccount;
            cmdWithdraw.ExecuteNonQuery();

            throw new Exception("Server is down!");

            using SqlCommand cmdDeposit = new SqlCommand(@"UPDATE Accounts SET Balance = Balance + @amount WHERE Id = @to", connection, transaction);

            cmdDeposit.Parameters.Add("@amount",SqlDbType.Decimal).Value = amount;
            cmdDeposit.Parameters.Add("@to", SqlDbType.Int).Value = toAccount;
            cmdDeposit.ExecuteNonQuery();

            transaction.Commit();
            Console.WriteLine("Withdraw successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Rollback launched.");

            try
            {
                transaction.Rollback();
                Console.WriteLine("Rollback initiated.");
            }
            catch (Exception rollbackEx)
            {
                Console.WriteLine($"Rollback error: {rollbackEx}");
            }
        }
    }

    public void ConnectToDatabase(string connectionString)
    {
        using DbConnection connection = _factory.CreateConnection();
        connection.ConnectionString = connectionString;
        connection.Open();

        using DbCommand cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT GETDATE()";
        var result = cmd.ExecuteScalar();
        Console.WriteLine($"Server time: {result}");
    }
}