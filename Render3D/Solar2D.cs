using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarObjects;

namespace Game1;

public class Solar2D : Game
{
    private const int Width = 1600;
    private const int Height = 900;

    private readonly SolarSpriteObject _sun;
    private readonly SolarSpriteObject _earth;
    private readonly SolarSpriteObject _earth2;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;

    public Solar2D()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;

        _graphics.PreferredBackBufferWidth = Width;
        _graphics.PreferredBackBufferHeight = Height;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _sun = new SolarSpriteObject(
            new SolarObject(3329400, new Vector2((float)Width / 2, (float)Height / 2)),
            Content.Load<Texture2D>("Sun"));
        _sun.TextureScale = 0.5f;

        _earth = new SolarSpriteObject(
            new SolarObject(10, new Vector2((float)Width / 2, ((float)Height / 2) + 300), new Vector2(1500, 0)),
            Content.Load<Texture2D>("Earth"));
        _earth.TextureScale = 0.2f;

        _earth2 = new SolarSpriteObject(
            new SolarObject(10, new Vector2(((float)Width / 2) + 600, (float)Height / 2), new Vector2(0, 1000)),
            Content.Load<Texture2D>("Earth"));
        _earth2.TextureScale = 0.2f;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _earth.InteractWithAnotherObject(_sun);
        _earth.InteractWithAnotherObject(_earth2);
        _earth.Update();

        _earth2.InteractWithAnotherObject(_sun);
        _earth2.InteractWithAnotherObject(_earth);
        _earth2.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_spriteBatch is null)
        {
            return;
        }

        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _sun.Draw(_spriteBatch);
        _earth.Draw(_spriteBatch);
        _earth2.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}