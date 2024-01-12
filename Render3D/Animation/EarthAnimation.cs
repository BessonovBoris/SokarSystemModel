using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1;

public class EarthAnimation : IAnimation
{
    private readonly IList<Texture2D> _textures;
    private Texture2D _currentTexture;

    public EarthAnimation(IList<Texture2D> textures)
    {
        _textures = textures;
        _currentTexture = _textures[0];
    }

    public Texture2D CurrentTexture => _currentTexture;

    public void ChangeTexture(Vector2 coordinates)
    {
        var v1 = new Vector2(1, 0);
        float angle = (coordinates.X * v1.X) + (coordinates.Y * v1.Y);
        angle /= coordinates.Length();

        if (angle.Equal(1))
        {
            _currentTexture = _textures[1];
        }
        else if (angle.Equal(-1))
        {
            _currentTexture = _textures[0];
        }
    }
}