using GameLibrary.Games;

namespace GameLibrary.Utility;

public static class Printer
{
    public static void PrintOptions<T>() where T : struct, Enum
    {
        foreach (T option in Enum.GetValues<T>())
        {
            System.Console.WriteLine($"{Convert.ToInt32(option)}. {option}");
        }
    }

    public static void PrintGameListExtended(List<Game> games)
    {
        for (int i = 0; i < games.Count; i++)
        {
            System.Console.WriteLine($"{i + 1}. {games[i].Describe()}");
        }
        System.Console.WriteLine();
    }

    public static void PrintGameList(List<Game> games)
    {
        for (int i = 0; i < games.Count; i++)
        {
            System.Console.WriteLine($"{i + 1}. {games[i].Name}");
        }
        System.Console.WriteLine();
    }

    public static void MessageEmpty()
    {
        System.Console.WriteLine("No games available");
        System.Console.WriteLine();
    }

    public static void MessageInvalidInput()
    {
        System.Console.WriteLine("Invalid input, please try again");
        System.Console.WriteLine();
    }

    public static void MessageSuccess(string action)
    {
        System.Console.WriteLine($"Game {action} successfully!");
        System.Console.WriteLine();
    }
}