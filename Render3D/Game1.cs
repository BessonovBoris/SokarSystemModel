using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1;

public class Game1 : Game
{
    private const float CamSpeed = -0.001f;

    private Vector3 _camPos;
    private Vector3 _camTar;

#pragma warning disable CA2213
    private GraphicsDeviceManager _graphics;
#pragma warning restore CA2213
#pragma warning disable CA2213
    private SpriteBatch? _spriteBatch;
#pragma warning restore CA2213

    private Matrix _projectionMatrix;
    private Matrix _viewMatrix;
    private Matrix _worldMatrix;
    private Model? _model;
    private Model? _model2;

    public Game1()
    {
        _camPos = new Vector3(0, 0, 0);
        _camTar = new Vector3(0f, 0, 1f);

        _graphics = new GraphicsDeviceManager(this);

        _graphics.IsFullScreen = false;

        _graphics.PreferredBackBufferWidth = 864;
        _graphics.PreferredBackBufferHeight = 486;
        _graphics.ApplyChanges();
        _graphics.GraphicsProfile = GraphicsProfile.HiDef;

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
            var rotateMatrixY = new Matrix(
                new Vector4((float)Math.Cos(CamSpeed), 0, -(float)Math.Sin(CamSpeed), 0),
                new Vector4(0, 1, 0, 0),
                new Vector4((float)Math.Sin(CamSpeed), 0, (float)Math.Cos(CamSpeed), 0),
                new Vector4(0, 0, 0, 1));

            _camTar.Y += rotateMatrixY.Determinant();
            _camPos.Y += rotateMatrixY.Determinant();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            var rotateMatrixY = new Matrix(
                new Vector4((float)Math.Cos(CamSpeed), 0, -(float)Math.Sin(CamSpeed), 0),
                new Vector4(0, 1, 0, 0),
                new Vector4((float)Math.Sin(CamSpeed), 0, (float)Math.Cos(CamSpeed), 0),
                new Vector4(0, 0, 0, 1));

            _camTar.Y -= rotateMatrixY.Determinant();
            _camPos.Y -= rotateMatrixY.Determinant();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            var rotateMatrixZ = new Matrix(
                new Vector4((float)Math.Cos(CamSpeed), (float)Math.Sin(CamSpeed), 0, 0),
                new Vector4(-(float)Math.Sin(CamSpeed), (float)Math.Cos(CamSpeed), 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 1));

            _camTar.Z -= rotateMatrixZ.Determinant();
            _camPos.Z -= rotateMatrixZ.Determinant();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            var rotateMatrixZ = new Matrix(
                new Vector4((float)Math.Cos(CamSpeed), (float)Math.Sin(CamSpeed), 0, 0),
                new Vector4(-(float)Math.Sin(CamSpeed), (float)Math.Cos(CamSpeed), 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 1));

            _camTar.Z += rotateMatrixZ.Determinant();
            _camPos.Z += rotateMatrixZ.Determinant();
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