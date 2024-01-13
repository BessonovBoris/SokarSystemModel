using System;
using System.Collections.Generic;
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

        Content.RootDirectory = "Content";
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
}