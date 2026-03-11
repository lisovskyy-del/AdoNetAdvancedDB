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
}