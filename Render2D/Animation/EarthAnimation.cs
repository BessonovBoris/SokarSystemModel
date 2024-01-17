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

    public void ChangeTexture(GameTime gameTime)
    {
        int index = ((int)gameTime.TotalGameTime.TotalMilliseconds / 10) % _textures.Count;
        _currentTexture = _textures[index];
    }
}