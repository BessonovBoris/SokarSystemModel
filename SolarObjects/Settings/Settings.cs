namespace SolarObjects.Settings;

public class Settings : ISettings
{
    public Settings(float distanceScale, float constantG, float earthVelocity)
    {
        DistanceScale = distanceScale;
        ConstantG = constantG;
        EarthVelocity = earthVelocity;
    }

    public float DistanceScale { get; }
    public float ConstantG { get; }
    public float EarthVelocity { get; }
}