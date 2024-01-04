﻿using Microsoft.Xna.Framework;

namespace SolarObjects;

public class SolarObject : ISolarObject
{
    private const float Scale = 149600000 / 300;
    private const float G = 12;
    private Vector2 _coordinates;
    private Vector2 _velocity;

    public SolarObject(int mass, Vector2 coordinates)
    {
        Mass = mass;
        _coordinates = coordinates;
        _velocity = Vector2.Zero;
    }

    public SolarObject(int mass, Vector2 coordinates, Vector2 velocity)
    {
        Mass = mass;
        _coordinates = coordinates;
        _velocity = velocity;
    }

    public Vector2 Coordinates => _coordinates;
    public int Mass { get; }

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        Vector2 radiusVector = _coordinates - solarObject.Coordinates;
        Vector2 velocity = -G * solarObject.Mass * radiusVector / (radiusVector.Length() * Scale);

        _velocity += velocity;
    }

    public void Update()
    {
        _coordinates += _velocity / 100;
    }
}