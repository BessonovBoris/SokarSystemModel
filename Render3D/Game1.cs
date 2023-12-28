using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;

    private Vector3 _camPos;
    private Vector3 _camTar;

    private Matrix _projectionMatrix;
    private Matrix _viewMatrix;
    private Matrix _worldMatrix;

    private Model? _model;
    private Model? _model2;

    public Game1()
    {
        _camPos = new Vector3(3, 0, 4);
        _camTar = new Vector3(0, 0, 0);

        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;

        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _viewMatrix = Matrix.CreateLookAt(_camPos, _camTar, Vector3.Up);

        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            (float)Window.ClientBounds.Width / (float)Window.ClientBounds.Height,
            1,
            100);

        _worldMatrix = Matrix.CreateWorld(new Vector3(0f, 0f, 0f), new Vector3(0, 0, -1), Vector3.Up);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _model = Content.Load<Model>("CubeBlender");
        _model2 = Content.Load<Model>("CubeBlender");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            _worldMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(1));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            _worldMatrix *= Matrix.CreateRotationX(-1 * MathHelper.ToRadians(1));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            _worldMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(1));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            _worldMatrix *= Matrix.CreateRotationY(-1 * MathHelper.ToRadians(1));
        }

        // _worldMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(1));
        // _worldMatrix *= Matrix.CreateRotationY(-1 * MathHelper.ToRadians(1));
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_model is null)
        {
            return;
        }

        if (_model2 is null)
        {
            return;
        }

        GraphicsDevice.Clear(Color.CornflowerBlue);

        foreach (ModelMesh mesh in _model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.View = _viewMatrix;
                effect.World = _worldMatrix;
                effect.Projection = _projectionMatrix;
            }

            mesh.Draw();
        }

        // _worldMatrix += Matrix.CreateTranslation(new Vector3(2, 2, 0));
        foreach (ModelMesh mesh in _model2.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.View = _viewMatrix;
                effect.World = _worldMatrix * Matrix.CreateTranslation(new Vector3(0, 1, 1));
                effect.Projection = _projectionMatrix;
            }

            mesh.Draw();
        }

        base.Draw(gameTime);
    }
}