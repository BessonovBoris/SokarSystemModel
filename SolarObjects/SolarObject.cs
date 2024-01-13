using Microsoft.Xna.Framework;
using SolarObjects.Settings;

namespace SolarObjects;

public class SolarObject : ISolarObject
{
    private readonly ISettings _settings;
    private Vector3 _coordinates;
    private Vector3 _velocity;

    public SolarObject(float mass, Vector3 coordinates,  ISettings settings)
    {
        Mass = mass;
        _coordinates = coordinates;
        _settings = settings;

        _velocity = new Vector3(_settings.EarthVelocity, 0, 0);
    }

    public Vector3 Coordinates => _coordinates;
    public float Mass { get; }

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        Vector3 radiusVector = _coordinates - solarObject.Coordinates;

        double ratio = -1 * _settings.ConstantG * 1e-3f * solarObject.Mass / (Math.Pow(radiusVector.Length(), 3) * _settings.Fps * _settings.DistanceScale * _settings.DistanceScale);
        Vector3 velocity = (float)ratio * radiusVector;

        Console.Clear();
        Console.WriteLine($"\nVelocity: {velocity} \nCoordinates: {Coordinates}\n\r");

        _velocity += velocity * 1000000;
    }

    public void Update()
    {
        _coordinates += _velocity;
    }
}