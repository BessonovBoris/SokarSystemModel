using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarObjects;

namespace Game1;

#pragma warning disable CA1001
public class SolarSpriteObject : ISolarObject
{
    private readonly ISolarObject _solarObject;
    private readonly Texture2D _texture;

    public SolarSpriteObject(ISolarObject solarObject, Texture2D texture)
    {
        _solarObject = solarObject;
        _texture = texture;
        TextureScale = 1;

        Console.WriteLine(_texture.Width);
    }

    public float TextureScale { get; set; }

    public Vector2 Coordinates => _solarObject.Coordinates;
    public int Mass => _solarObject.Mass;

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        _solarObject.InteractWithAnotherObject(solarObject);
    }

    public void Update()
    {
        _solarObject.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var vector2Coordinates = new Vector2(_solarObject.Coordinates.X - (_texture.Width * TextureScale / 2), _solarObject.Coordinates.Y - (_texture.Height * TextureScale / 2));
        spriteBatch.Draw(_texture, vector2Coordinates, null, Color.White, 0, Vector2.Zero, TextureScale, SpriteEffects.None, 0);
    }
}