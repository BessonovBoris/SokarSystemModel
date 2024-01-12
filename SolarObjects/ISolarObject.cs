using Microsoft.Xna.Framework;

namespace SolarObjects;

public interface ISolarObject
{
    Vector2 Coordinates { get; }
    float Mass { get; }
    void InteractWithAnotherObject(ISolarObject solarObject);
    void Update();
}