using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarObjects;

namespace Game1;

public class Solar2D : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;

    private Matrix _projectionMatrix;
    private Matrix _viewMatrix;
    private Matrix _worldMatrix;

    private BasicEffect _effect;

    private SolarSpriteObject _sun;
    private SolarSpriteObject _earth;

    public Solar2D()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;

        _graphics.PreferredBackBufferWidth = 1600;
        _graphics.PreferredBackBufferHeight = 900;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _effect = new BasicEffect(GraphicsDevice);
        _effect.VertexColorEnabled = true;

        _sun = new SolarSpriteObject(
            new SolarObject(3329400, new Vector2((float)Window.ClientBounds.Width / 2, (float)Window.ClientBounds.Height / 2)),
            Content.Load<Texture2D>("Sun"));
        _sun.TextureScale = 0.5f;

        _earth = new SolarSpriteObject(
            new SolarObject(10, new Vector2(0, 0)),
            Content.Load<Texture2D>("Earth"));
        _earth.TextureScale = 0.2f;
    }

    protected override void Initialize()
    {
        _viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up);

        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            Window.ClientBounds.Width / (float)Window.ClientBounds.Height,
            1,
            100);

        _worldMatrix = Matrix.CreateWorld(new Vector3(0, 0, -88), new Vector3(0, 0, -1), Vector3.Up);

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

        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(0, 0, 0.5f));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(0, 0, -0.5f));
        }

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

        _effect.World = _worldMatrix;
        _effect.View = _viewMatrix;
        _effect.Projection = _projectionMatrix;

        _spriteBatch.Begin();
        _sun.Draw(_spriteBatch);
        _earth.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}