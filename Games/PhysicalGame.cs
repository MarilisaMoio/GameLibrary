namespace GameLibrary.Games;

public class PhysicalGame : Game
{
    public Condition Condition
    {
        get;
        set => field = Enum.IsDefined(value) ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    public PhysicalGame(string name, string genre, decimal price, Condition condition) : base(name, genre, price)
    {
        Condition = condition;
    }

    public override string Describe()
    {
        return base.Describe() + $", Condition: {Condition}";
    }
}