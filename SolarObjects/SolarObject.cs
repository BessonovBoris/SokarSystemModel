using Microsoft.Xna.Framework;
using SolarObjects.Settings;

namespace SolarObjects;

public class SolarObject : ISolarObject
{
    private readonly ISettings _settings;
    private Vector2 _coordinates;
    private Vector2 _velocity;

    public SolarObject(int mass, Vector2 coordinates, ISettings settings)
    {
        Mass = mass;
        _coordinates = coordinates;
        _velocity = Vector2.Zero;
        _settings = settings;
    }

    public SolarObject(int mass, Vector2 coordinates, Vector2 velocity,  ISettings settings)
    {
        Mass = mass;
        _coordinates = coordinates;
        _velocity = velocity;
        _settings = settings;
    }

    public Vector2 Coordinates => _coordinates;
    public int Mass { get; }

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        Vector2 radiusVector = _coordinates - solarObject.Coordinates;
        Vector2 velocity = -_settings.ConstantG * solarObject.Mass * radiusVector / (radiusVector.Length() * _settings.DistanceScale);

        _velocity += velocity;
    }

    public void Update()
    {
        _coordinates += _velocity / 100;
    }
}