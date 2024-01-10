using Microsoft.Xna.Framework;
using SolarObjects.Settings;

namespace SolarObjects;

public class SolarObject : ISolarObject
{
    private readonly ISettings _settings;
    private Vector3 _coordinates;
    private Vector3 _velocity;

    public SolarObject(int mass, Vector3 coordinates,  ISettings settings)
    {
        Mass = mass;
        _coordinates = coordinates;
        _settings = settings;

        _velocity = new Vector3(_settings.EarthVelocity, 0, 0);
    }

    public Vector3 Coordinates => _coordinates;
    public int Mass { get; }

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        // TODO: как сделать нормальный масштаб???? Я НЕ ПОНИМАЮ
        Vector3 radiusVector = _coordinates - solarObject.Coordinates;
        Vector3 velocity = -1 * _settings.ConstantG * solarObject.Mass * radiusVector / (float)Math.Pow(radiusVector.Length(), 1);

        _velocity += velocity;
    }

    public void Update()
    {
        _coordinates += _velocity;
    }
}