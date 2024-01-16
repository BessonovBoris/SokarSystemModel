using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarObjects;
using SolarObjects.Settings;

namespace Game1;

public class Solar3D : Game
{
    private readonly Vector3 _camPos;
    private readonly Vector3 _camTar;
#pragma warning disable CA2213
    private readonly BasicEffect _effect;

    private readonly SolarModelObject _sun;
    private readonly SolarModelObject _earth;

    private GraphicsDeviceManager _graphics;
#pragma warning restore CA2213
    private Matrix _projectionMatrix;
    private Matrix _viewMatrix;
    private Matrix _worldMatrix;

    public Solar3D()
    {
        _camPos = new Vector3(0, 0, 1);
        _camTar = new Vector3(0, 0, 0);

        _graphics = new GraphicsDeviceManager(this);

        _graphics.IsFullScreen = false;

        _graphics.PreferredBackBufferWidth = 1900;
        _graphics.PreferredBackBufferHeight = 1060;
        _graphics.ApplyChanges();
        _graphics.GraphicsProfile = GraphicsProfile.HiDef;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _effect = new BasicEffect(GraphicsDevice);
        _effect.VertexColorEnabled = true;

        ISettings settings = JsonSettingsReader.LoadSettings("../../../../SolarObjects/Settings.json");

        _sun = new SolarModelObject(
            new SolarObject(settings.SunMass, new Vector3(0, 0, 0), settings),
            Content.Load<Model>("MySun"));

        _earth = new SolarModelObject(
            new SolarObject(settings.EarthMass, new Vector3(70.71f, 70.71f, 0), settings),
            Content.Load<Model>("MyEarth"));
    }

    protected override void Initialize()
    {
        _viewMatrix = Matrix.CreateLookAt(_camPos, _camTar, Vector3.Up);

        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            Window.ClientBounds.Width / (float)Window.ClientBounds.Height,
            1,
            10000000);

        _worldMatrix = Matrix.CreateWorld(new Vector3(0, 0, -200), new Vector3(0, 0, -1), Vector3.Up);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // _spriteBatch = new SpriteBatch(GraphicsDevice);
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

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(0.5f, 0, 0));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(-0.5f, 0, 0.5f));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(0, -0.5f, 0));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(0, 0.5f, 0));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(-0.5f, 0, 0));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(0.5f, 0, 0));
        }

        _earth.InteractWithAnotherObject(_sun);
        _earth.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkBlue);

        _effect.World = _worldMatrix;
        _effect.View = _viewMatrix;
        _effect.Projection = _projectionMatrix;

        _sun.Draw(_viewMatrix, _worldMatrix, _projectionMatrix);
        _earth.Draw(_viewMatrix, _worldMatrix, _projectionMatrix);

        base.Draw(gameTime);
    }
}