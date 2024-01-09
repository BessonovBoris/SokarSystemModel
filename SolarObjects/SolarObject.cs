using Microsoft.Xna.Framework;
using SolarObjects.Settings;

namespace SolarObjects;

public class SolarObject : ISolarObject
{
    private readonly ISettings _settings;
    private Vector2 _coordinates;
    private Vector2 _velocity;

    public SolarObject(int mass, Vector2 coordinates,  ISettings settings)
    {
        Mass = mass;
        _coordinates = coordinates;
        _settings = settings;

        _velocity = new Vector2(_settings.EarthVelocity, 0);
    }

    public Vector2 Coordinates => _coordinates;
    public int Mass { get; }

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        // TODO: как сделать нормальный масштаб???? Я НЕ ПОНИМАЮ
        Vector2 radiusVector = _coordinates - solarObject.Coordinates;
        Vector2 velocity = -1 * _settings.ConstantG * solarObject.Mass * radiusVector / (float)Math.Pow(radiusVector.Length(), 3);

        _velocity += velocity;
    }

    public void Update()
    {
        _coordinates += _velocity;
    }
}