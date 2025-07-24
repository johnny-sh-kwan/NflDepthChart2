using System;

namespace Domain;

public class DepthChart
{
    public required Dictionary<Position, List<Player>> Chart { get; init; }

    public DepthChart(string team, string formation, Player[] players)
    {
        Chart = new();
    }
}
