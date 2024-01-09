namespace SolarObjects.Settings;

public class Settings : ISettings
{
    public Settings(float distanceScale, float constantG, float earthVelocity, int sunMass, int earthMass)
    {
        DistanceScale = distanceScale;
        ConstantG = constantG;
        EarthVelocity = earthVelocity;
        SunMass = sunMass;
        SunMass = sunMass;
        EarthMass = earthMass;
    }

    public float DistanceScale { get; }
    public float ConstantG { get; }
    public float EarthVelocity { get; }
    public int SunMass { get; }
    public int EarthMass { get; }
}