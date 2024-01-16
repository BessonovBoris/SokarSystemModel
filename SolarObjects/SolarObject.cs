using Microsoft.Xna.Framework;
using SolarObjects.Settings;

namespace SolarObjects;

public class SolarObject : ISolarObject
{
    private const float VelocityRatio = 1e+7f;

    private readonly ISettings _settings;

    private Vector3 _coordinates;
    private Vector3 _velocity;

    public SolarObject(float mass, Vector3 coordinates,  ISettings settings)
    {
        Mass = mass;
        _coordinates = coordinates;
        _settings = settings;

        _velocity = new Vector3(_settings.EarthVelocity, 0, 0) * VelocityRatio;
    }

    public Vector3 Coordinates => _coordinates;
    public float Mass { get; }

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        Vector3 radiusVector = _coordinates - solarObject.Coordinates;

        double ratio = -1 * _settings.ConstantG * 1e-9f * solarObject.Mass / (Math.Pow(radiusVector.Length() * _settings.DistanceScale, 3) * Math.Pow(_settings.Fps, 2));
        Vector3 velocity = (float)ratio * radiusVector;
        Console.WriteLine(velocity);

        _velocity += velocity * VelocityRatio * VelocityRatio;
    }

    public void Update()
    {
        _coordinates += _velocity;
    }
}