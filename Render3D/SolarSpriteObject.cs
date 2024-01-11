using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarObjects;
namespace Game1;

#pragma warning disable CA1001
public class SolarSpriteObject : ISolarObject
{
    private readonly ISolarObject _solarObject;

    private Texture2D _texture;
    private Texture2D _textureleft;
    private Texture2D _textureUp;
    private Texture2D _textureRight;
    private Texture2D _textureDown;

    public SolarSpriteObject(ISolarObject solarObject, Texture2D texture)
    {
        _texture = null!;
        _solarObject = solarObject;
        Texture = texture;
        _textureleft = null!;
        _textureUp = null!;
        _textureRight = null!;
        _textureDown = null!;
        TextureScale = 1;
    }

    public SolarSpriteObject(ISolarObject solarObject, Texture2D textureleft, Texture2D textureUp, Texture2D textureRight, Texture2D textureDown)
    {
        _texture = null!;
        _solarObject = solarObject;
        _textureleft = textureleft;
        _textureUp = textureUp;
        _textureRight = textureRight;
        _textureDown = textureDown;
        Texture = _textureUp;
        TextureScale = 1;
    }

    public float TextureScale { get; set; }

    public Texture2D Texture
    {
        get
        {
            if (_solarObject.Coordinates.Y > 700)
            {
                _texture = _textureleft;
                return _texture;
            }

            if (_solarObject.Coordinates.Y < 300)
            {
                _texture = _textureUp;
                return _texture;
            }
            else
            {
                return _texture;
            }
        }
        set
        {
            _texture = value;
        }
    }

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
        var bias = new Vector2(Texture.Width * TextureScale / 2, Texture.Height * TextureScale / 2);
        Vector2 vector2Coordinates = _solarObject.Coordinates - bias;

        spriteBatch.Draw(Texture, vector2Coordinates, null, Color.White, 0, Vector2.Zero, TextureScale, SpriteEffects.None, 0);
    }
}