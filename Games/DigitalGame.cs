namespace GameLibrary.Games;

public class DigitalGame : Game
{
    public Platform Platform { get; set; }
    public PegiRating Pegi { get; set; }

    public DigitalGame(string name, string genre, decimal price, PegiRating pegi, Platform platform) : base(name, genre, price)
    {
        if (!Enum.IsDefined(pegi))
            throw new ArgumentOutOfRangeException(nameof(pegi));
        if (!Enum.IsDefined(platform))
            throw new ArgumentOutOfRangeException(nameof(platform));

        Platform = platform;
        Pegi = pegi;
    }

    public override string Describe()
    {
        return base.Describe() + $", Pegi: {(int)Pegi}, Platform: {Platform}";
    }
}