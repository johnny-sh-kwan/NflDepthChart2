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
