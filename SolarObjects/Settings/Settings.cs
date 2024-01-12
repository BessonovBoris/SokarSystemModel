namespace SolarObjects.Settings;

public class Settings : ISettings
{
    public Settings(float distanceScale, float massScale, float earthVelocity, float sunMass, float earthMass)
    {
        DistanceScale = distanceScale;

        MassScale = massScale;
        EarthVelocity = earthVelocity; // in km per hour

        SunMass = sunMass / MassScale;
        EarthMass = earthMass / MassScale;

        ConstantG = 6.67e-11f; // in H*m^2/new kg^2
        ConstantG /= 1e+6f; // in H*kg^2/new kg^2
        ConstantG *= (float)Math.Pow(MassScale / DistanceScale, 2); // in H*newKM^2/new newKG^2

        // TODO: это исправить
        ConstantG = 6.67e-10f;
    }

    // 1 newKM = DistanceScale km
    public float DistanceScale { get; }

    // 1 newKG = MassScale kg
    public float MassScale { get; }

    // in H*newKM^2 / new newKG^2
    public float ConstantG { get; }

    // in km per 1 hour
    public float EarthVelocity { get; }

    // in kg
    public float SunMass { get; }

    // in kg
    public float EarthMass { get; }
}