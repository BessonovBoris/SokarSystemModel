using Microsoft.Xna.Framework;

namespace SolarObjects;

public interface ISolarObject
{
    Vector2 Coordinates { get; }
    int Mass { get; }
    void InteractWithAnotherObject(ISolarObject solarObject);
    void Update();
}