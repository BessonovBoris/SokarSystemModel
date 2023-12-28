using System.Numerics;

namespace SolarObjects;

public interface ISolarObject
{
    Vector3 Coordinates { get; }
    int Mass { get; }
    void InteractWithAnotherObject(ISolarObject solarObject);
}