using System.Globalization;

namespace GameLibrary.Utility;

public static class Prompter
{
    public static string AskForTextInput(string prompt)
    {
        string input;
        do
        {
            Console.WriteLine(prompt);
            input = ReadInput();
            if (string.IsNullOrWhiteSpace(input))
            {
                Printer.MessageInvalidInput();
            }
        } while (string.IsNullOrWhiteSpace(input));

        return input;
    }

    public static T AskForEnumInput<T>(string prompt) where T : struct, Enum
    {
        T value;
        bool isValidInput;
        do
        {
            Console.WriteLine(prompt);
            Printer.PrintOptions<T>();
            isValidInput = Enum.TryParse(ReadInput(), true, out value) && Enum.IsDefined(value);

            if (!isValidInput)
            {
                Printer.MessageInvalidInput();
            }
        } while (!isValidInput);

        return value;
    }

    public static decimal AskForDecimalInput(string prompt)
    {
        decimal value;
        bool isValidInput;
        do
        {
            Console.WriteLine(prompt);
            // Accept both ',' and '.' as decimal separator, regardless of the system culture
            string input = ReadInput().Replace(',', '.');
            isValidInput = decimal.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value)
                && value >= 0;

            if (!isValidInput)
            {
                Printer.MessageInvalidInput();
            }
        } while (!isValidInput);

        return value;
    }

    public static int AskForIndexInput(int maxIndex)
    {
        string userChoice;
        bool isIndexPresent;
        int index;

        do
        {
            userChoice = ReadInput();
            bool isValidInput = int.TryParse(userChoice, out int value);
            index = value - 1;
            isIndexPresent = (index >= 0) && (maxIndex > index);
            if (!isValidInput || !isIndexPresent)
            {
                Printer.MessageInvalidInput();
            }
        } while (string.IsNullOrWhiteSpace(userChoice) || !int.TryParse(userChoice, out int result) || !isIndexPresent); 

        return index;
    }

    public static string ReadInput()
    {
        // ReadLine returns null when the input stream is closed (e.g. Ctrl+Z on Windows):
        // no more input will ever arrive, so asking again would loop forever
        string? input = Console.ReadLine();
        if (input is null)
        {
            System.Console.WriteLine("Bye!");
            Environment.Exit(0);
        }

        return input;
    }
}