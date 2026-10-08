using GameLibrary.Games;

List<Game> games = new List<Game>();
string? userInput;

System.Console.WriteLine("Welcome to the Game Library!");

do
{
    Console.WriteLine("[S]ee all games");
    Console.WriteLine("[A]dd game");
    Console.WriteLine("[R]emove game");
    Console.WriteLine("[E]xit");

    userInput = Console.ReadLine();
    
    switch (userInput?.ToUpper())
    {
        case "S":
            ShowAllGames();
            break;
        case "A":
            AddNewGame();
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

void ShowAllGames()
{
    if (games.Count == 0)
    {
        MessageEmpty();
    }
    else
    {
        PrintGameList(games);
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

    //choose game type and ask for additional information based on the type
    if (gameType == GameType.Digital)
    {
        // Ask for game pegi until a valid input is provided
        PegiRating pegi = AskForEnumInput<PegiRating>("Insert the PEGI of the game:");

        // Ask for game platform until a valid input is provided
        Platform platform = AskForEnumInput<Platform>("Insert the platform of the game:");

        games.Add(new DigitalGame(userGameName, userGameGenre, pegi, platform));
        MessageSuccess("added");
    }
    else if (gameType == GameType.Physical)
    {
        // Ask for game condition until a valid input is provided
        Condition condition = AskForEnumInput<Condition>("Insert the condition of the game:");

        games.Add(new PhysicalGame(userGameName, userGameGenre, condition));
        MessageSuccess("added");
    }
}

void RemoveGame()
{
    string? userChoice;
    bool isIndexPresent;

    if (games.Count == 0)
    {
        MessageEmpty();
    }
    else
    {
        Console.WriteLine("Select the index of the game to remove:");
        PrintGameList(games);
        do
        {
            userChoice = Console.ReadLine();
            bool isValidInput = int.TryParse(userChoice, out int value);
            int index = value - 1;
            isIndexPresent = (index >= 0) && (games.Count > index);
            if (!isValidInput || !isIndexPresent)
            {
                MessageInvalidInput();
            } else
            {
                games.RemoveAt(index);
                MessageSuccess("removed");
            }
        } while (string.IsNullOrWhiteSpace(userChoice) || !int.TryParse(userChoice, out int result) || !isIndexPresent);  
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

void PrintGameList(List<Game> games)
{
    for (int i = 0; i < games.Count; i++)
    {
        System.Console.WriteLine($"{i + 1}. {games[i].Describe()}");
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