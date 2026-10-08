namespace GameLibrary.Games;

public class PhysicalGame : Game
{
    public Condition Condition
    {
        get;
        set => field = Enum.IsDefined(value) ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    public PhysicalGame(string name, string genre, Condition condition) : base(name, genre)
    {
        Condition = condition;
    }

    public override string Describe()
    {
        return base.Describe() + $", Condition: {Condition}";
    }
}