using System;
using System.Collections.Generic;
using Apos.Shapes;
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
    private const float EarthEccentricity = 0.0167086f;

    private readonly float _p;

    private readonly float _earthRadius;

    private readonly ShapeBatch _shapeBatch;

    private readonly SolarSpriteObject _sun;
    private readonly SolarSpriteObject _earth;

    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;

    private Vector2 _worldCoordinates;

    private Vector2? _firstMousePosition;
    private Vector2 _secondMousePosition;

    public Solar2D()
    {
        ISettings settings = JsonSettingsReader.LoadSettings("../../../../SolarObjects/Settings.json");
        _p = 149.6e+6f * (1 - (EarthEccentricity * EarthEccentricity)) / settings.DistanceScale;

        _earthRadius = 149.6e+6f / settings.DistanceScale;
        _worldCoordinates = new Vector2(0, 0);

        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;

        _graphics.PreferredBackBufferWidth = Width;
        _graphics.PreferredBackBufferHeight = Height;
        _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        _graphics.ApplyChanges();

        Content.RootDirectory = "C:\\Users\\boris\\RiderProjects\\SolarSystemModel\\Program\\Content";
        IsMouseVisible = true;

        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1d / settings.Fps);

        _firstMousePosition = null;
        _secondMousePosition = Vector2.Zero;

        _shapeBatch = new ShapeBatch(GraphicsDevice, Content);

        IAnimation sunAnimation = new SunAnimation(new List<Texture2D> { Content.Load<Texture2D>("Sun") });
        _sun = new SolarSpriteObject(
            new SolarObject(settings.SunMass, new Vector3((float)Width / 2, (float)Height / 2, 0), settings),
            sunAnimation);
        _sun.TextureScale = 0.2f;

        IAnimation earthAnimation = new EarthAnimation(new List<Texture2D> { Content.Load<Texture2D>("FirstEarth"), Content.Load<Texture2D>("SecondEarth") });
        _earth = new SolarSpriteObject(
            new SolarObject(settings.EarthMass, new Vector3((float)Width / 2, ((float)Height / 2) + _earthRadius, 0), settings),
            earthAnimation);
        _earth.TextureScale = 0.1f;
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
        _spriteBatch.End();

        _shapeBatch.Begin();
        DrawOrbit((x) => _p / (1 + (EarthEccentricity * (float)Math.Cos(x))), 0, 2 * (float)Math.PI, 1000);
        _shapeBatch.End();

        base.Draw(gameTime);
    }

    protected override void Dispose(bool disposing)
    {
        _graphics.Dispose();
        _spriteBatch?.Dispose();

        base.Dispose(disposing);
    }

    private static float FromPolarToX(float r, float phi)
    {
        return r * (float)Math.Cos(phi);
    }

    private static float FromPolarToY(float r, float phi)
    {
        return r * (float)Math.Sin(phi);
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

            var firstPoint = new Vector2(FromPolarToX(oldY, oldX) + (Width / 2), FromPolarToY(oldY, oldX) + (Height / 2));
            var secondPoint = new Vector2(FromPolarToX(newY, newX) + (Width / 2), FromPolarToY(newY, newX) + (Height / 2));

            firstPoint += _worldCoordinates;
            secondPoint += _worldCoordinates;

            _shapeBatch.FillLine(
                firstPoint,
                secondPoint,
                1,
                Color.White);

            oldX = newX;
            oldY = newY;
        }
    }
}