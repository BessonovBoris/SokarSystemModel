namespace SolarObjects.Settings;

public interface ISettings
{
    float DistanceScale { get; }
    float MassScale { get; }
    float ConstantG { get; }
    float EarthVelocity { get; }
    float SunMass { get; }
    float EarthMass { get; }
    int Fps { get; }
}