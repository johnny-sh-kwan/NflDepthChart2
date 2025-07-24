using System;

namespace Domain;

public class DepthChart
{
    public Dictionary<Position, List<Player>> Chart { get; init; }

    public DepthChart()
    {
        Chart = new();
    }
}
