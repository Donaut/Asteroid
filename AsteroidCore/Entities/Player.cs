using System;
using System.Drawing;
using System.Numerics;

namespace AsteroidCore;

public class Player : IEntity
{
    /// <inheritdoc />
    public float Scale { get; set; }

    /// <inheritdoc />
    public float Rotation { get; set; }

    /// <inheritdoc />
    public Vector2 Position { get; set; }

    /// <inheritdoc />
    public Vector2 Velocity { get; set; } = Vector2.Zero; // Vector[]

    /// <inheritdoc />
    public int CollisionRadius { get; set; }

    /// <inheritdoc />
    public Vector2[] Vertices { get; set; } = Array.Empty<Vector2>();

    /// <inheritdoc />
    public Color Color { get; set; } = Color.YellowGreen;
}