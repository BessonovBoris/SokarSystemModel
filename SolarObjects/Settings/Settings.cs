using SolarObjects.Services;

namespace SolarObjects.Settings;

public class Settings : ISettings
{
    public Settings(float distanceScale, float massScale, float earthVelocity, float sunMass, float earthMass, int fps)
    {
        Fps = fps;

        DistanceScale = distanceScale;
        MassScale = massScale;

        EarthVelocity = earthVelocity.Equal(-1) ? 29.783f / (DistanceScale * Fps) : earthVelocity;
        Console.WriteLine(EarthVelocity);

        SunMass = sunMass / MassScale;
        EarthMass = earthMass / MassScale;

        ConstantG = 6.67e-11f; // in H*m^2/kg^2
    }

    // 1 newKM = DistanceScale km
    public float DistanceScale { get; }

    // 1 newKG = MassScale kg
    public float MassScale { get; }

    // in H*kn^2 / new kg^2
    public float ConstantG { get; }

    // in km per 1 hour
    public float EarthVelocity { get; }

    // in kg
    public float SunMass { get; }

    // in kg
    public float EarthMass { get; }
    public int Fps { get; }
}