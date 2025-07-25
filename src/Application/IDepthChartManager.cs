using Domain;

namespace Application;

public interface IDepthChartManager
{
    DepthChart DepthChart { get; }

    void AddPlayerToDepthChart(Position position, Player player, int depth = -1);
    List<Player> GetBackups(Position position, Player player);
    string GetFullDepthChart();
    List<Player> RemovePlayerFromDepthChart(Position position, Player player);

    void MyInitDepthChart();
    void MyClearAll();
}
