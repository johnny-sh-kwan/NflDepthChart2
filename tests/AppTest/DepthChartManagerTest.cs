using Application;
using Domain;
using FluentAssertions;
using Moq;

namespace AppTest;

public class DepthChartManagerTest
{
    private readonly Mock<ILiteDbRepo> _repoMock;
    private DepthChartManager? _manager;

    public DepthChartManagerTest()
    {
        _repoMock = new Mock<ILiteDbRepo>();
    }

    [Fact]
    public void AddPlayerToDepthChart_AddsPlayer_WhenPositionDoesNotExist()
    {
        // Arrange: Mock the Load method to return an empty DepthChart
        _repoMock.Setup(r => r.Load()).Returns(new DepthChart { Chart = new Dictionary<Position, List<Player>>() });
        _manager = new DepthChartManager(_repoMock.Object);
        var player = new Player { Name = "Tom Brady", Number = 12 };

        // Act
        _manager.AddPlayerToDepthChart(Position.QB, player);

        // Assert
        _manager.DepthChart.Chart.Should().ContainKey(Position.QB);
        _manager.DepthChart.Chart[Position.QB].First().Name.Should().Be("Tom Brady");
        _manager.DepthChart.Chart[Position.QB].First().Number.Should().Be(12);

        _repoMock.Verify(r => r.Save(It.IsAny<DepthChart>()), Times.Once);
    }

    [Fact]
    public void AddPlayerToDepthChart_AddsPlayer_WhenPositionExists_ToEnd()
    {
        // Arrange: Mock the Load method to return an empty DepthChart
        _repoMock.Setup(r => r.Load()).Returns(new DepthChart
        {
            Chart = new Dictionary<Position, List<Player>>()
            {
                { Position.QB, new List<Player>() { new Player { Name = "John Doe", Number = 1 } } }
            }
        });
        _manager = new DepthChartManager(_repoMock.Object);
        var player = new Player { Name = "Tom Brady", Number = 12 };

        // Act
        _manager.AddPlayerToDepthChart(Position.QB, player);

        // Assert
        _manager.DepthChart.Chart.Should().ContainKey(Position.QB);

        _manager.DepthChart.Chart[Position.QB].First().Name.Should().Be("John Doe");
        _manager.DepthChart.Chart[Position.QB].First().Number.Should().Be(1);
        _manager.DepthChart.Chart[Position.QB].Skip(1).First().Name.Should().Be("Tom Brady");
        _manager.DepthChart.Chart[Position.QB].Skip(1).First().Number.Should().Be(12);

        _repoMock.Verify(r => r.Save(It.IsAny<DepthChart>()), Times.Once);
    }
    
    [Fact]
    public void AddPlayerToDepthChart_AddsPlayer_WhenPositionExists_ToFront()
    {
        // Arrange: Mock the Load method to return an empty DepthChart
        _repoMock.Setup(r => r.Load()).Returns(new DepthChart
        {
            Chart = new Dictionary<Position, List<Player>>()
            {
                { Position.QB, new List<Player>() { new Player { Name = "John Doe", Number = 1 } } }
            }
        });
        _manager = new DepthChartManager(_repoMock.Object);
        var player = new Player { Name = "Tom Brady", Number = 12 };

        // Act
        _manager.AddPlayerToDepthChart(Position.QB, player, 0);

        // Assert
        _manager.DepthChart.Chart.Should().ContainKey(Position.QB);

        _manager.DepthChart.Chart[Position.QB].First().Name.Should().Be("Tom Brady");
        _manager.DepthChart.Chart[Position.QB].First().Number.Should().Be(12);
        _manager.DepthChart.Chart[Position.QB].Skip(1).First().Name.Should().Be("John Doe");
        _manager.DepthChart.Chart[Position.QB].Skip(1).First().Number.Should().Be(1);

        _repoMock.Verify(r => r.Save(It.IsAny<DepthChart>()), Times.Once);
    }
}
