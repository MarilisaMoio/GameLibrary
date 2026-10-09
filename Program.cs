using GameLibrary.Games;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using GameLibrary.Utility;

// fixing the encoding issue with the console output
Console.OutputEncoding = System.Text.Encoding.UTF8;

List<Game> games = new List<Game>();
string userInput;

string filePath = Path.Combine(AppContext.BaseDirectory, "games.json");
JsonSerializerOptions jsonOptions = new()
{
    WriteIndented = true,
    Converters = { new JsonStringEnumConverter() }
};

System.Console.WriteLine("Welcome to the Game Library!");

LoadGames();

do
{
    Console.WriteLine("[S]ee all games");
    Console.WriteLine("[T]otal value of games");
    Console.WriteLine("[A]dd game");
    Console.WriteLine("[M]odify game");
    Console.WriteLine("[R]emove game");
    Console.WriteLine("[E]xit");

    userInput = Prompter.ReadInput();
    
    switch (userInput.ToUpper())
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
        Printer.MessageEmpty();
    }
    else
    {
        Printer.PrintGameListExtended(games);
    }
}

void AddNewGame()
{
    // Ask for game type until a valid input is provided
    GameType gameType = Prompter.AskForEnumInput<GameType>("Insert the type of game:");

    // Ask for game name until a valid input is provided
    string userGameName = Prompter.AskForTextInput("Insert the name of the game:");

    // Ask for game genre until a valid input is provided
    string userGameGenre = Prompter.AskForTextInput("Insert the genre of the game:");

    // Ask for game price until a valid input is provided
    decimal userGamePrice = Prompter.AskForDecimalInput("Insert the price of the game:");

    //choose game type and ask for additional information based on the type
    if (gameType == GameType.Digital)
    {
        // Ask for game pegi until a valid input is provided
        PegiRating pegi = Prompter.AskForEnumInput<PegiRating>("Insert the PEGI of the game:");

        // Ask for game platform until a valid input is provided
        Platform platform = Prompter.AskForEnumInput<Platform>("Insert the platform of the game:");

        games.Add(new DigitalGame(userGameName, userGameGenre, userGamePrice, pegi, platform));
    }
    else if (gameType == GameType.Physical)
    {
        // Ask for game condition until a valid input is provided
        Condition condition = Prompter.AskForEnumInput<Condition>("Insert the condition of the game:");

        games.Add(new PhysicalGame(userGameName, userGameGenre, userGamePrice, condition));
    }

    SaveGames();
    Printer.MessageSuccess("added");
}

void SaveGames()
{
    string json = JsonSerializer.Serialize(games, jsonOptions);
    File.WriteAllText(filePath, json);
}

void RemoveGame()
{
    if (games.Count == 0)
    {
        Printer.MessageEmpty();
    }
    else
    {
        Console.WriteLine("Select the index of the game to remove:");
        Printer.PrintGameList(games);

        int indexToRemove = Prompter.AskForIndexInput(games.Count);

        games.RemoveAt(indexToRemove);
        SaveGames();
        Printer.MessageSuccess("removed");
    }
}

void ModifyGame()
{
    if (games.Count == 0)
    {
        Printer.MessageEmpty();
    }
    else
    {
        Console.WriteLine("Select the index of the game to modify:");
        Printer.PrintGameList(games);

        int indexToModify = Prompter.AskForIndexInput(games.Count);

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

        int propertyIndex = Prompter.AskForIndexInput(totalProperties);

        PropertyInfo propertyToModify = settableProperties[propertyIndex];
        Type propertyType = propertyToModify.PropertyType;
        string prompt = $"Enter the new value for {propertyToModify.Name}:";

        // Map the runtime type to a compile-time type to call the generic method
        object newValue = propertyType switch
        {
            _ when propertyType == typeof(Condition) => Prompter.AskForEnumInput<Condition>(prompt),
            _ when propertyType == typeof(Platform) => Prompter.AskForEnumInput<Platform>(prompt),
            _ when propertyType == typeof(PegiRating) => Prompter.AskForEnumInput<PegiRating>(prompt),
            _ when propertyType == typeof(decimal) => Prompter.AskForDecimalInput(prompt),
            _ => Prompter.AskForTextInput(prompt)
        };

        propertyToModify.SetValue(gameToModify, newValue);
        SaveGames();
        Printer.MessageSuccess("modified");
    }
}

void LoadGames()
{
    if (File.Exists(filePath))
    {
        try
        {
            string json = File.ReadAllText(filePath);
            games = JsonSerializer.Deserialize<List<Game>>(json, jsonOptions) ?? new List<Game>();
        }
        catch (JsonException)
        {
            System.Console.WriteLine("Error: The games.json file is corrupted. Starting with an empty game list.");
            games = new List<Game>();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error loading games: {ex.Message}");
            games = new List<Game>();
        }   
    }
}