namespace SolarObjects.Settings;

public class Settings : ISettings
{
    public Settings(float distanceScale, float constantG)
    {
        DistanceScale = distanceScale;
        ConstantG = constantG;
    }

    public float DistanceScale { get; }
    public float ConstantG { get; }
}