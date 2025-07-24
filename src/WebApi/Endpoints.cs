using Application;
using Domain;
using Microsoft.AspNetCore.Builder;

public static class Endpoints
{
    public static void MapEndpoints(WebApplication app)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", () =>
        {
            Console.WriteLine("weatherforecast 2");

            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast");



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

        app.MapGet("/DepthChart",
            (IDepthChartManager depthChartManager) =>
        {
            string depthChart = depthChartManager.GetFullDepthChart();
            return Results.Content(depthChart, contentType: "text/plain");

            // return Results.Ok(depthChart);
        });
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}