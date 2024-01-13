using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarObjects;
using SolarObjects.Settings;

namespace Game1;

public class Solar2D : Game
{
    private readonly Vector3 _camPos;
    private Vector3 _camTar;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;

    private Matrix _projectionMatrix;
    private Matrix _viewMatrix;
    private Matrix _worldMatrix;

    private BasicEffect _effect;

    private TriangleObj _sun;
    private TriangleObj _earth;

    public Solar2D()
    {
        _camPos = new Vector3(0, 0, 88);
        _camTar = new Vector3(0, 0, 0);

        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;

        _graphics.PreferredBackBufferWidth = 1900;
        _graphics.PreferredBackBufferHeight = 1060;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _effect = new BasicEffect(GraphicsDevice);
        _effect.VertexColorEnabled = true;

        ISettings settings = JsonSettingsReader.LoadSettings("../../../../SolarObjects/Settings.json");

        _sun = new TriangleObj(new SolarObject(3329400, new Vector3(0, 0, 0), settings), GraphicsDevice, Color.Yellow);
        _earth = new TriangleObj(new SolarObject(10, new Vector3(0, 5, 0), settings), GraphicsDevice, Color.Blue);
    }

    protected override void Initialize()
    {
        _viewMatrix = Matrix.CreateLookAt(_camPos, _camTar, Vector3.Up);

        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            Window.ClientBounds.Width / (float)Window.ClientBounds.Height,
            1,
            100);

        _worldMatrix = Matrix.CreateWorld(new Vector3(0, 0, 0), new Vector3(0, 0, -1), Vector3.Up);

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
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(0, 0, -0.5f));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(0, 0, 0.5f));
        }

        _earth.InteractWithAnotherObject(_sun);
        _earth.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _effect.World = _worldMatrix;
        _effect.View = _viewMatrix;
        _effect.Projection = _projectionMatrix;

        _sun.Draw(GraphicsDevice, _effect);
        _earth.Draw(GraphicsDevice, _effect);

        base.Draw(gameTime);
    }

    protected override void Dispose(bool disposing)
    {
        if (_spriteBatch is null)
        {
            return;
        }

        _spriteBatch.Dispose();
        _graphics.Dispose();
        _effect.Dispose();

        base.Dispose(disposing);
    }
}