using Domain;

namespace Application;

public interface ILiteDbRepo
{
    DepthChart Load();
    void Save(DepthChart depthChart);
}
