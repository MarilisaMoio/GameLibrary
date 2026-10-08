namespace GameLibrary.Games;

public abstract class Game
{
    public string Name { get; set; }
    public string Genre { get; set; }
    public decimal Price { get; set; }

    protected Game(string name, string genre, decimal price)
    {
        Name = name;
        Genre = genre;
        Price = price;
    }

    public virtual string Describe()
    {
        return $"Game: {Name}, Genre: {Genre}, Price: {Price:C}";
    }
}