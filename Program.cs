using GameLibrary.Games;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

// fixing the encoding issue with the console output
Console.OutputEncoding = System.Text.Encoding.UTF8;

List<Game> games = new List<Game>();
string? userInput;

System.Console.WriteLine("Welcome to the Game Library!");

do
{
    Console.WriteLine("[S]ee all games");
    Console.WriteLine("[T]otal value of games");
    Console.WriteLine("[A]dd game");
    Console.WriteLine("[M]odify game");
    Console.WriteLine("[R]emove game");
    Console.WriteLine("[E]xit");

    userInput = Console.ReadLine();
    
    switch (userInput?.ToUpper())
    {
        case "S":
            ShowAllGames();
            break;
        case "T":
            ShowTotalValue();
            break;
        case "A":
            AddNewGame();
            break;
        case "M":
            ModifyGame();
            break;
        case "R":
            RemoveGame();
            break;
        case "E":
            System.Console.WriteLine("Bye!");
            break;
        default:
            System.Console.WriteLine("Select an option from the menu");
            System.Console.WriteLine();
            break;
    }
} while (!string.Equals(userInput, "E", StringComparison.OrdinalIgnoreCase));

void ShowTotalValue()
{
    decimal totalValue = games.Sum(game => game.Price);
    System.Console.WriteLine($"Total value of games: {totalValue:C}");
    System.Console.WriteLine();
}

void ShowAllGames()
{
    if (games.Count == 0)
    {
        MessageEmpty();
    }
    else
    {
        PrintGameListExtended(games);
    }
}

void AddNewGame()
{
    // Ask for game type until a valid input is provided
    GameType gameType = AskForEnumInput<GameType>("Insert the type of game:");

    // Ask for game name until a valid input is provided
    string userGameName = AskForTextInput("Insert the name of the game:");

    // Ask for game genre until a valid input is provided
    string userGameGenre = AskForTextInput("Insert the genre of the game:");

    // Ask for game price until a valid input is provided
    decimal userGamePrice = AskForDecimalInput("Insert the price of the game:");

    //choose game type and ask for additional information based on the type
    if (gameType == GameType.Digital)
    {
        // Ask for game pegi until a valid input is provided
        PegiRating pegi = AskForEnumInput<PegiRating>("Insert the PEGI of the game:");

        // Ask for game platform until a valid input is provided
        Platform platform = AskForEnumInput<Platform>("Insert the platform of the game:");

        games.Add(new DigitalGame(userGameName, userGameGenre, userGamePrice, pegi, platform));
        MessageSuccess("added");
    }
    else if (gameType == GameType.Physical)
    {
        // Ask for game condition until a valid input is provided
        Condition condition = AskForEnumInput<Condition>("Insert the condition of the game:");

        games.Add(new PhysicalGame(userGameName, userGameGenre, userGamePrice, condition));
        MessageSuccess("added");
    }
}

void RemoveGame()
{
    if (games.Count == 0)
    {
        MessageEmpty();
    }
    else
    {
        Console.WriteLine("Select the index of the game to remove:");
        PrintGameList(games);

        int indexToRemove = AskForIndexInput(games.Count);

        games.RemoveAt(indexToRemove);
        MessageSuccess("removed");
    }
}

void ModifyGame()
{
    if (games.Count == 0)
    {
        MessageEmpty();
    }
    else
    {
        Console.WriteLine("Select the index of the game to modify:");
        PrintGameList(games);

        int indexToModify = AskForIndexInput(games.Count);

        Game gameToModify = games[indexToModify];

        List<PropertyInfo> settableProperties = gameToModify.GetType()
            .GetProperties()
            .Where(p => p.SetMethod is { IsPublic: true }
                && !p.SetMethod.ReturnParameter.GetRequiredCustomModifiers().Contains(typeof(IsExternalInit)))
            .OrderBy(p => p.DeclaringType != typeof(Game))
            .ThenBy(p => p.Name != nameof(Game.Name))
            .ToList();

        System.Console.WriteLine("Enter the index of the property you want to modify:");

        int totalProperties = settableProperties.Count;
        for (int i = 0; i < totalProperties; i++)
        {
            System.Console.WriteLine($"{i + 1}. {settableProperties[i].Name} (current: {settableProperties[i].GetValue(gameToModify)})");
        }

        int propertyIndex = AskForIndexInput(totalProperties);

        PropertyInfo propertyToModify = settableProperties[propertyIndex];
        Type propertyType = propertyToModify.PropertyType;
        string prompt = $"Enter the new value for {propertyToModify.Name}:";

        // Map the runtime type to a compile-time type to call the generic method
        object newValue = propertyType switch
        {
            _ when propertyType == typeof(Condition) => AskForEnumInput<Condition>(prompt),
            _ when propertyType == typeof(Platform) => AskForEnumInput<Platform>(prompt),
            _ when propertyType == typeof(PegiRating) => AskForEnumInput<PegiRating>(prompt),
            _ when propertyType == typeof(decimal) => AskForDecimalInput(prompt),
            _ => AskForTextInput(prompt)
        };

        propertyToModify.SetValue(gameToModify, newValue);
        MessageSuccess("modified");
    }
}
void MessageEmpty()
{
    System.Console.WriteLine("No games available");
    System.Console.WriteLine();
}

void MessageInvalidInput()
{
    System.Console.WriteLine("Invalid input, please try again");
    System.Console.WriteLine();
}

void MessageSuccess(string action)
{
    System.Console.WriteLine($"Game {action} successfully!");
    System.Console.WriteLine();
}

void PrintOptions<T>() where T : struct, Enum
{
    foreach (T option in Enum.GetValues<T>())
    {
        System.Console.WriteLine($"{Convert.ToInt32(option)}. {option}");
    }
}

void PrintGameListExtended(List<Game> games)
{
    for (int i = 0; i < games.Count; i++)
    {
        System.Console.WriteLine($"{i + 1}. {games[i].Describe()}");
    }
    System.Console.WriteLine();
}

void PrintGameList(List<Game> games)
{
    for (int i = 0; i < games.Count; i++)
    {
        System.Console.WriteLine($"{i + 1}. {games[i].Name}");
    }
    System.Console.WriteLine();
}

string AskForTextInput(string prompt)
{
    string? input;
    do
    {
        Console.WriteLine(prompt);
        input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            MessageInvalidInput();
        }
    } while (string.IsNullOrWhiteSpace(input));

    return input;
}

T AskForEnumInput<T>(string prompt) where T : struct, Enum
{
    T value;
    bool isValidInput;
    do
    {
        Console.WriteLine(prompt);
        PrintOptions<T>();
        isValidInput = Enum.TryParse(Console.ReadLine(), true, out value) && Enum.IsDefined(value);

        if (!isValidInput)
        {
            MessageInvalidInput();
        }
    } while (!isValidInput);

    return value;
}

decimal AskForDecimalInput(string prompt)
{
    decimal value;
    bool isValidInput;
    do
    {
        Console.WriteLine(prompt);
        // Accept both ',' and '.' as decimal separator, regardless of the system culture
        string input = (Console.ReadLine() ?? "").Replace(',', '.');
        isValidInput = decimal.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value)
            && value >= 0;

        if (!isValidInput)
        {
            MessageInvalidInput();
        }
    } while (!isValidInput);

    return value;
}

int AskForIndexInput(int maxIndex)
{
    string? userChoice;
    bool isIndexPresent;
    int index;

    do
    {
        userChoice = Console.ReadLine();
        bool isValidInput = int.TryParse(userChoice, out int value);
        index = value - 1;
        isIndexPresent = (index >= 0) && (maxIndex > index);
        if (!isValidInput || !isIndexPresent)
        {
            MessageInvalidInput();
        }
    } while (string.IsNullOrWhiteSpace(userChoice) || !int.TryParse(userChoice, out int result) || !isIndexPresent); 

    return index;
}