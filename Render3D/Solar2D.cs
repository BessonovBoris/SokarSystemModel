using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarObjects;
using SolarObjects.Settings;

namespace Game1;

public class Solar2D : Game
{
    private const int Width = 1600;
    private const int Height = 900;

    private readonly SolarSpriteObject _sun;
    private readonly SolarSpriteObject _earth;

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

        ISettings settings = JsonSettingsReader.LoadSettings("../../../../SolarObjects/Settings.json");

        _sun = new SolarSpriteObject(
            new SolarObject(settings.SunMass, new Vector2((float)Width / 2, (float)Height / 2), settings),
            Content.Load<Texture2D>("Sun"));
        _sun.TextureScale = 0.5f;

        _earth = new SolarSpriteObject(
            new SolarObject(settings.EarthMass, new Vector2((float)Width / 2, ((float)Height / 2) + 299.2f), settings),
            Content.Load<Texture2D>("Earth"));
        _earth.TextureScale = 0.2f;
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
        _earth.Update();

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
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}