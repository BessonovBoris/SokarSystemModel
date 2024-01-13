using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1;

public class SunAnimation : IAnimation
{
    private readonly IList<Texture2D> _textures;
    private Texture2D _currentTexture;

    public SunAnimation(IList<Texture2D> textures)
    {
        _textures = textures;
        _currentTexture = _textures[0];
    }

    public Texture2D CurrentTexture => _currentTexture;

    public void ChangeTexture(Vector2 coordinates)
    {
        _currentTexture = _textures[0];
    }
}