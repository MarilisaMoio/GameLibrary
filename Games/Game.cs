namespace GameLibrary.Games;

public abstract class Game
{
    public string Name { get; set; }
    public string Genre { get; set; }

    protected Game(string name, string genre)
    {
        Name = name;
        Genre = genre;
    }

    public virtual string Describe()
    {
        return $"Game: {Name}, Genre: {Genre}";
    }
}