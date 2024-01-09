namespace SolarObjects.Settings;

public interface ISettings
{
    float DistanceScale { get; }
    float ConstantG { get; }
    float EarthVelocity { get; }
    int SunMass { get; }
    int EarthMass { get; }
}