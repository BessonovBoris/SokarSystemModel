using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1;

public interface IAnimation
{
    Texture2D CurrentTexture { get; }
    void ChangeTexture(Vector2 coordinates);
}