using Application;
using Domain;
using Microsoft.AspNetCore.Builder;

public static class Endpoints
{
    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/DepthChart/AddPlayer",
            (Position position, string playerName, int playerNumber, int? depth, IDepthChartManager depthChartManager) =>
        {
            try
            {
                Player player = new() { Name = playerName, Number = playerNumber };

                if (depth == null)
                    depthChartManager.AddPlayerToDepthChart(position, player);
                else
                    depthChartManager.AddPlayerToDepthChart(position, player, depth.Value);

                return Results.Ok($"Player {player.Name} added to position {position} at depth {depth}.");
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        app.MapDelete("/DepthChart/RemovePlayer",
            (Position position, string playerName, int playerNumber, IDepthChartManager depthChartManager) =>
        {
            Player player = new() { Name = playerName, Number = playerNumber };
            List<Player> removedPlayers = depthChartManager.RemovePlayerFromDepthChart(position, player);

            if (removedPlayers.Count > 0)
                return Results.Ok($"Player {player.Name} removed from position {position}.");
            else
                return Results.NotFound($"Player {player.Name} not found at position {position}.");
        });

        app.MapGet("/DepthChart/GetBackups",
            (Position position, string playerName, int playerNumber, IDepthChartManager depthChartManager) =>
        {
            Player player = new() { Name = playerName, Number = playerNumber };
            List<Player> backups = depthChartManager.GetBackups(position, player);

            if (backups.Count > 0)
                return Results.Ok(backups);
            else
                return Results.NotFound($"No backups found for player {player.Name} at position {position}.");
        });

        app.MapGet("/DepthChart",
            (IDepthChartManager depthChartManager) =>
        {
            string depthChart = depthChartManager.GetFullDepthChart();
            return Results.Content(depthChart, contentType: "text/plain");

            // return Results.Ok(depthChart);
        });


        // Not part of Spec, just for initializing the depth chart
        app.MapPost("/DepthChart/MyInit",
        (IDepthChartManager depthChartManager) =>
        {
            depthChartManager.MyInitDepthChart();
            return Results.Ok("Depth chart initialized.");
        });
        
        app.MapDelete("/DepthChart/MyClearAll",
        (IDepthChartManager depthChartManager) =>
        {
            depthChartManager.MyClearAll();
            return Results.Ok("Depth chart cleared.");
        });
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}