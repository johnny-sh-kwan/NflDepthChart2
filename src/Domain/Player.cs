namespace Domain;

public class Player
{
    public required int Number { get; init; }
    public required string Name { get; init; }

    //public required Position Position { get; init; }   // position is not required, we know the position from the depth chart

    public override string ToString()
    {
        return $"{Name} (#{Number})";
    }
}

// Note: should implement IEquatable<Player>, applies more seemlessly to LINQ queries
public class PlayerComparer : IEqualityComparer<Player>
{
    public bool Equals(Player? x, Player? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return x.Number == y.Number && x.Name == y.Name;
    }

    public int GetHashCode(Player obj)
    {
        if (obj is null)
            return 0;

        return HashCode.Combine(obj.Number, obj.Name);
    }
}