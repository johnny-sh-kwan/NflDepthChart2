using System.Text;
using System.Text.Json;
using Domain;

namespace Application;

public class DepthChartManager : IDepthChartManager
{
    private readonly ILiteDbRepo _liteDbRepo;

    public DepthChart DepthChart { get; private set; }

    public DepthChartManager(ILiteDbRepo liteDbRepo)
    {
        _liteDbRepo = liteDbRepo;

        // Load the depth chart from the database
        DepthChart = liteDbRepo.Load();
        if (DepthChart == null)
        {
            Console.WriteLine("No depth chart found in the database. Initializing a new one.");
            DepthChart = new DepthChart();
        }
    }

    public void AddPlayerToDepthChart(Position position, Player player, int depth = -1)
    {
        if (!DepthChart.Chart.ContainsKey(position))
        {
            Console.WriteLine($"Position {position} does not exist in the depth chart. Creating a new list for this position.");
            DepthChart.Chart[position] = new List<Player>();
        }

        // Ensure the player is not already in the list
        if (!DepthChart.Chart[position].Contains(player))
        {
            if (depth == -1)
            {
                DepthChart.Chart[position].Add(player);
            }
            else
                DepthChart.Chart[position].Insert(depth, player);

            _liteDbRepo.Save(DepthChart);
        }
        else
        {
            Console.WriteLine($"Player {player.Name} is already in the depth chart at position {position}.");
            throw new InvalidOperationException($"Player {player.Name} is already in the depth chart at position {position}.");
        }
    }

    public List<Player> RemovePlayerFromDepthChart(Position position, Player player)
    {
        if (DepthChart.Chart.ContainsKey(position)
            && DepthChart.Chart[position].FirstOrDefault(p => p.Name == player.Name && p.Number == player.Number) is Player existingPlayer)
        {
            DepthChart.Chart[position].Remove(existingPlayer);
            _liteDbRepo.Save(DepthChart);
            return [player];
        }
        else
        {
            Console.WriteLine($"Player {player.Name} is not in the depth chart at position {position}.");
            return [];
        }
    }

    public List<Player> GetBackups(Position position, Player player)
    {
        if (!DepthChart.Chart.ContainsKey(position))
        {
            Console.WriteLine($"Position {position} does not exist in the depth chart.");
            return [];
        }

        // Check if the player is in the depth chart at the specified position
        Player? existingPlayer = DepthChart.Chart[position].FirstOrDefault(p => p.Name == player.Name && p.Number == player.Number);
        if (existingPlayer == null)
        {
            Console.WriteLine($"Player {player.Name} is not in the depth chart at position {position}.");
            return [];
        }

        int playerIndex = DepthChart.Chart[position].IndexOf(existingPlayer);
        return DepthChart.Chart[position].Skip(playerIndex + 1).ToList();
    }

    public string GetFullDepthChart()
    {
        StringBuilder sb = new();
        foreach (var kvp in DepthChart.Chart)
        {
            sb.Append($"Position: {kvp.Key}, Players: ");

            string players = string.Join(", ", kvp.Value.Select(p => p.ToString()));
            sb.AppendLine(players);
        }
        return sb.ToString();

        // or, we could return the serialized JSON
        // return JsonSerializer.Serialize(DepthChart.Chart);
    }

    // Initialize the depth chart with some default values... not part of spec
    public void MyInitDepthChart()
    {
        DepthChart = new()
        {
            Chart = new Dictionary<Position, List<Player>>
            {
                {
                    Position.LWR, new()
                    { new Player { Name = "Mike Evans", Number = 13 }, new Player { Name = "Jaelon Darden", Number = 11 }, new Player { Name = "Scott Miller", Number = 10 } }
                },
                //{ Position.RWR, new List<Player>() },
                {
                    Position.LT, new()
                    { new Player { Name = "Donovan Smith", Number = 76 }, new Player { Name = "Josh Wells", Number = 72 } }
                },
                //{ Position.LG, new List<Player>() },
                //{ Position.C, new List<Player>() },
                //{ Position.RG, new List<Player>() },
                //{ Position.RT, new List<Player>() },
                //{ Position.TE1, new List<Player>() },
                //{ Position.TE2, new List<Player>() },
                { Position.QB, new()
                    { new Player { Name = "Blaine Gabbert", Number = 11 } }
                },
                //{ Position.RB, new List<Player>() },            
            }
        };

        _liteDbRepo.Save(DepthChart);
    }
    
    public void MyClearAll()
    {
        DepthChart = new() { Chart = new Dictionary<Position, List<Player>>() };
        _liteDbRepo.Save(DepthChart);
    }
}
