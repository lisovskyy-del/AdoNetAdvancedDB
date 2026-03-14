using Microsoft.Data.SqlClient;
using System.Data;

namespace MainProgram.Levels;

internal class Level3
{
    public static void Run(SqlConnection connection)
    {
        PlaceOrder(connection);
    }

    public static void PlaceOrder(SqlConnection connection)
    {
        int productId = InputHelpers.IntInput("\nEnter product ID: ");
        int quantity = InputHelpers.IntInput("\nEnter product's quantity: ");
        string customerName = InputHelpers.StringInput("\nEnter customer's name: ");

        SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

        try
        {
            using SqlCommand productStock = new SqlCommand(@"SELECT Stock FROM Products WHERE Id = @id", connection, transaction);

            productStock.Parameters.Add("@id", SqlDbType.Int).Value = productId;

            int stock = (int)productStock.ExecuteScalar();

            if (stock >= quantity)
            {
                SqlCommand insertOrder = new SqlCommand(@"INSERT INTO Orders " +
                    "(ProductId, Quantity, CustomerName, OrderDate) VALUES" +
                    "(@productId, @quantity, @customerName, GETDATE())", connection, transaction);

                insertOrder.Parameters.Add("@productId", SqlDbType.Int).Value = productId;
                insertOrder.Parameters.Add("@quantity", SqlDbType.Int).Value = quantity;
                insertOrder.Parameters.Add("@customerName", SqlDbType.NVarChar).Value = customerName;

                insertOrder.ExecuteNonQuery();

                SqlCommand updateStock = new SqlCommand(@"UPDATE Products SET Stock = Stock - @quantity WHERE Id = @id", connection, transaction);

                updateStock.Parameters.Add("@quantity", SqlDbType.Int).Value = quantity;
                updateStock.Parameters.Add("@id", SqlDbType.Int).Value = productId;

                updateStock.ExecuteNonQuery();

                transaction.Commit();
                Console.WriteLine("\nOrder created.");
            }
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
}
