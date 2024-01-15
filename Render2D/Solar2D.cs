using System;
using System.Collections.Generic;
using Game1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarObjects;
using SolarObjects.Settings;

namespace Render2D;

public class Solar2D : Game
{
    private const int Width = 1600;
    private const int Height = 900;

    private readonly SolarSpriteObject _sun;
    private readonly SolarSpriteObject _earth;

    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;

    private Vector2 _worldCoordinates;

    private Vector2? _firstMousePosition;
    private Vector2 _secondMousePosition;

    public Solar2D()
    {
        _worldCoordinates = new Vector2(0, 0);

        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;

        _graphics.PreferredBackBufferWidth = Width;
        _graphics.PreferredBackBufferHeight = Height;
        _graphics.ApplyChanges();

        Content.RootDirectory = "C:\\Users\\boris\\RiderProjects\\SolarSystemModel\\Program\\Content";
        IsMouseVisible = true;

        ISettings settings = JsonSettingsReader.LoadSettings("../../../../SolarObjects/Settings.json");

        IAnimation sunAnimation = new SunAnimation(new List<Texture2D> { Content.Load<Texture2D>("Sun") });
        _sun = new SolarSpriteObject(
            new SolarObject(settings.SunMass, new Vector3((float)Width / 2, (float)Height / 2, 0), settings),
            sunAnimation);
        _sun.TextureScale = 0.2f;

        IAnimation earthAnimation = new EarthAnimation(new List<Texture2D> { Content.Load<Texture2D>("FirstEarth"), Content.Load<Texture2D>("SecondEarth") });
        _earth = new SolarSpriteObject(
            new SolarObject(settings.EarthMass, new Vector3((float)Width / 2, ((float)Height / 2) + 299.2f, 0), settings),
            earthAnimation);
        _earth.TextureScale = 0.1f;

        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1d / settings.Fps);

        _firstMousePosition = null;
        _secondMousePosition = Vector2.Zero;
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
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        MouseState mouseState = Mouse.GetState();
        MouseTranslation(mouseState);

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
        _sun.Draw(_spriteBatch, _worldCoordinates);
        _earth.Draw(_spriteBatch, _worldCoordinates);

        float r = 299.2f;
        DrawOrbit((x) => (float)Math.Sqrt((r * r) - (float)Math.Pow(x - (Width / 2), 2)) + (Height / 2), (Width / 2) - 299.2f, (Width / 2) + 299.2f, 100);
        DrawOrbit((x) => -(float)Math.Sqrt((r * r) - (float)Math.Pow(x - (Width / 2), 2)) + (Height / 2), (Width / 2) - 299.2f, (Width / 2) + 299.2f, 100);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    protected override void Dispose(bool disposing)
    {
        _graphics.Dispose();
        _spriteBatch?.Dispose();

        base.Dispose(disposing);
    }

    private void MouseTranslation(MouseState mouseState)
    {
        var currentMousePosition = new Vector2(mouseState.X, mouseState.Y);

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            if (_firstMousePosition is null)
            {
                _firstMousePosition = currentMousePosition;
                _secondMousePosition = currentMousePosition;
            }
            else if (_firstMousePosition != _secondMousePosition)
            {
                Vector2? translationMouse = _secondMousePosition - _firstMousePosition;
                _worldCoordinates += translationMouse ?? throw new ArgumentException("translationMouse is null");
                _firstMousePosition = _secondMousePosition;
            }
            else
            {
                _secondMousePosition = currentMousePosition;
            }
        }

        if (mouseState.LeftButton == ButtonState.Released)
        {
            _firstMousePosition = null;
        }
    }

    private void DrawOrbit(Func<float, float> function, float startX, float endX, int accuracy)
    {
        float delta = (endX - startX) / accuracy;

        float oldX = startX;
        float oldY = function(oldX);

        for (float i = startX; i <= endX; i += delta)
        {
            float newX = i;
            float newY = function(newX);

            DrawLine(new Vector2(oldX, oldY), new Vector2(newX, newY));
            oldX = newX;
            oldY = newY;
        }
    }

    private void DrawLine(Vector2 firstCoordinates, Vector2 secondCoordinates)
    {
        Vector2 lineVector = secondCoordinates - firstCoordinates;
        float angle = lineVector.X / lineVector.Length();
        angle = (float)Math.Acos(angle);

        if (lineVector.Y < 0)
        {
            angle *= -1;
        }

        using var basePart = new Texture2D(_graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        basePart.SetData(new[] { Color.White });
        var rectangle = new Rectangle((int)firstCoordinates.X, (int)firstCoordinates.Y, (int)lineVector.Length(), 2);

        _spriteBatch?.Draw(basePart, rectangle, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 1);
    }
}