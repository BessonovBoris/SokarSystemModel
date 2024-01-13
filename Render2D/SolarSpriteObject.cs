using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarObjects;

namespace Game1;

public class SolarSpriteObject : ISolarObject
{
    private readonly ISolarObject _solarObject;
    private readonly IAnimation _animation;

    public SolarSpriteObject(ISolarObject solarObject, IAnimation animation)
    {
        _animation = animation;
        _solarObject = solarObject;
        TextureScale = 1;
    }

    public float TextureScale { get; set; }

    public Vector3 Coordinates => _solarObject.Coordinates;
    public float Mass => _solarObject.Mass;

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        var coordinates2D = new Vector2(Coordinates.X, Coordinates.Y);
        coordinates2D -= new Vector2(solarObject.Coordinates.X, solarObject.Coordinates.Y);

        _animation.ChangeTexture(coordinates2D);
        _solarObject.InteractWithAnotherObject(solarObject);
    }

    public void Update()
    {
        _solarObject.Update();
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 worldCoordinates)
    {
        Texture2D texture = _animation.CurrentTexture;
        var coordinates2D = new Vector2(Coordinates.X, Coordinates.Y);

        var bias = new Vector2(texture.Width * TextureScale / 2, texture.Height * TextureScale / 2);
        Vector2 vector2Coordinates = coordinates2D - bias + worldCoordinates;

        spriteBatch.Draw(texture, vector2Coordinates, null, Color.White, 0, Vector2.Zero, TextureScale, SpriteEffects.None, 0);
    }
}