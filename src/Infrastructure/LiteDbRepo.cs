using Application;
using Domain;
using LiteDB;

namespace Infrastructure;

public class LiteDbRepo : ILiteDbRepo
{
    private const string DbName = "MyLiteDb.db";
    private const string DepthCharts = "DepthCharts";

    public void Save(DepthChart depthChart)
    {
        Console.WriteLine($"Saving DepthChart with {depthChart.Chart.Count} positions.");

        string dbPath = Path.Combine(Environment.CurrentDirectory, DbName);

        using (var db = new LiteDatabase(dbPath))
        {
            var collection = db.GetCollection<DepthChart>(DepthCharts);

            // just delete all and insert new one, only 1 team for demo
            collection.DeleteAll();
            collection.Insert(depthChart);
        }
    }

    public DepthChart Load()
    {
        // Implementation for loading the depth chart from LiteDB
        Console.WriteLine($"Loading DepthChart");

        string dbPath = Path.Combine(Environment.CurrentDirectory, DbName);

        using (var db = new LiteDatabase(dbPath))
        {
            var collection = db.GetCollection<DepthChart>(DepthCharts);
            return collection.FindAll().Count() == 0
                ? new()
                : collection.FindAll().First();
        }
    }
}
