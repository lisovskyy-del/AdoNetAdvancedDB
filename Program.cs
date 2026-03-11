namespace MainProgram;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        string connectionString = configuration.GetConnectionString("AdoNetAdvancedDB")
            ?? throw new InvalidOperationException("Connection string 'AdoNetAdvancedDB' not found.");

        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            while (true)
            {
                Console.WriteLine("\nChoose level:\n");
                Console.WriteLine("1. Level 1");
                Console.WriteLine("0. Exit");
                Console.WriteLine("\nYour choice: ");
                string? input = InputHelpers.StringInput("\nYour choice: ");

                if (int.TryParse(input, out int userChoice))
                {
                    if (userChoice == 0)
                    {
                        Console.WriteLine("\nExitting...");
                        return;
                    }
                    else if (userChoice == 1)
                    {
                        Level1.Run(connection);
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid choice! Enter a digit between 0-3!");
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input! Enter a digit!");
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }
}