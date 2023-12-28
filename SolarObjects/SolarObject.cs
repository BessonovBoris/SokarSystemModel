using System.Numerics;

namespace SolarObjects;

public class SolarObject : ISolarObject
{
    private readonly int _mass;
    private Vector3 _coordinates;

    public SolarObject(int mass)
    {
        _coordinates = Vector3.Zero;
        _mass = mass;
    }

    public SolarObject(int mass, Vector3 coordinates)
    {
        _mass = mass;
        _coordinates = coordinates;
    }

    public Vector3 Coordinates => _coordinates;
    public int Mass => _mass;

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        throw new NotImplementedException();
    }
}