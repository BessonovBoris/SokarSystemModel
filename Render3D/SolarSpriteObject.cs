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

    public Vector2 Coordinates => _solarObject.Coordinates;
    public float Mass => _solarObject.Mass;

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        _animation.ChangeTexture(Coordinates - solarObject.Coordinates);
        _solarObject.InteractWithAnotherObject(solarObject);
    }

    public void Update()
    {
        _solarObject.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Texture2D texture = _animation.CurrentTexture;

        var bias = new Vector2(texture.Width * TextureScale / 2, texture.Height * TextureScale / 2);
        Vector2 vector2Coordinates = _solarObject.Coordinates - bias;

        spriteBatch.Draw(texture, vector2Coordinates, null, Color.White, 0, Vector2.Zero, TextureScale, SpriteEffects.None, 0);
    }
}