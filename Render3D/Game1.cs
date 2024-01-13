using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1;

public class Game1 : Game
{
    private const float CamSpeed = -0.001f;

    private readonly Vector3 _camPos;
    private Vector3 _camTar;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;

    private Matrix _projectionMatrix;
    private Matrix _viewMatrix;
    private Matrix _worldMatrix;

    private Model? _model;
    private Model? _model2;

    public Game1()
    {
        _camPos = new Vector3(0, 0, 0);
        _camTar = new Vector3(1f, 0, 1f);

        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;

        _graphics.PreferredBackBufferWidth = 864;
        _graphics.PreferredBackBufferHeight = 486;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _viewMatrix = Matrix.CreateLookAt(_camPos, _camTar, Vector3.Up);

        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            Window.ClientBounds.Width / (float)Window.ClientBounds.Height,
            1,
            100);

        _worldMatrix = Matrix.CreateWorld(new Vector3(0f, 0f, 10f), new Vector3(0, 0, -1), Vector3.Up);

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
            // _worldMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(1));
            var rotateMatrixY = new Matrix(
                new Vector4((float)Math.Cos(CamSpeed), 0, -(float)Math.Sin(CamSpeed), 0),
                new Vector4(0, 1, 0, 0),
                new Vector4((float)Math.Sin(CamSpeed), 0, (float)Math.Cos(CamSpeed), 0),
                new Vector4(0, 0, 0, 1));

            _camTar.Y += rotateMatrixY.Determinant();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            // _worldMatrix *= Matrix.CreateRotationX(-1 * MathHelper.ToRadians(1));
            var rotateMatrixY = new Matrix(
                new Vector4((float)Math.Cos(CamSpeed), 0, -(float)Math.Sin(CamSpeed), 0),
                new Vector4(0, 1, 0, 0),
                new Vector4((float)Math.Sin(CamSpeed), 0, (float)Math.Cos(CamSpeed), 0),
                new Vector4(0, 0, 0, 1));

            _camTar.Y -= rotateMatrixY.Determinant();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            // _worldMatrix *= Matrix.CreateRotationY(-1 * MathHelper.ToRadians(1));
            var rotateMatrixZ = new Matrix(
                new Vector4((float)Math.Cos(CamSpeed), (float)Math.Sin(CamSpeed), 0, 0),
                new Vector4(-(float)Math.Sin(CamSpeed), (float)Math.Cos(CamSpeed), 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 1));

            _camTar.Z -= rotateMatrixZ.Determinant();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            // _worldMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(1));
            var rotateMatrixZ = new Matrix(
                new Vector4((float)Math.Cos(CamSpeed), (float)Math.Sin(CamSpeed), 0, 0),
                new Vector4(-(float)Math.Sin(CamSpeed), (float)Math.Cos(CamSpeed), 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 1));

            _camTar.Z += rotateMatrixZ.Determinant();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(0, 0, -0.1f));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(0, 0, 0.1f));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(-0.1f, 0, 0));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            _worldMatrix *= Matrix.CreateTranslation(new Vector3(0.1f, 0, 0));
        }

        // _worldMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(1));
        // _worldMatrix *= Matrix.CreateRotationY(-1 * MathHelper.ToRadians(1));
        _viewMatrix = Matrix.CreateLookAt(_camPos, _camTar, Vector3.Up);

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

    protected override void Dispose(bool disposing)
    {
        if (_spriteBatch is null)
        {
            return;
        }

        _spriteBatch.Dispose();
        _graphics.Dispose();

        base.Dispose(disposing);
    }
}