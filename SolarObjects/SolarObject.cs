using Microsoft.Xna.Framework;

namespace SolarObjects;

public class SolarObject : ISolarObject
{
    private const float Scale = 29920000;
    private const float G = 12;
    private Vector3 _coordinates;
    private Vector3 _velocity;

    public SolarObject(int mass)
    {
        _coordinates = Vector3.Zero;
        _velocity = Vector3.Zero;
        Mass = mass;
    }

    public SolarObject(int mass, Vector3 coordinates)
    {
        Mass = mass;
        _coordinates = coordinates;

        _velocity = Vector3.Zero;
    }

    public Vector3 Coordinates => _coordinates;
    public int Mass { get; }

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        Vector3 radiusVector = _coordinates - solarObject.Coordinates;
        Vector3 velocity = -G * solarObject.Mass * radiusVector / (radiusVector.Length() * Scale);

        _velocity += velocity;
    }

    public void Update()
    {
        _coordinates += _velocity / 100;

        // _velocity = Vector3.Zero;
    }
}