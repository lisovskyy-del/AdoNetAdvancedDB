namespace MainProgram.Utils;

internal class InputHelpers
{
    public static string StringInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            Console.WriteLine("\nInvalid input. Please enter a string.");
        }
    }

    public static int IntInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int result))
            {
                return result;
            }

            Console.WriteLine("\nInvalid input. Please enter a number.");
        }
    }

    public static decimal DecimalInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (decimal.TryParse(input, out decimal result))
            {
                return result;
            }

            Console.WriteLine("\nInvalid input. Please enter a number.");
        }
    }
}