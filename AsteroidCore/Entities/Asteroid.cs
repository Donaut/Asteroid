﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidCore.Entities;

public class Asteroid : IEntity
{
    /// <inheritdoc />
    public float Scale { get; set; } = 10;

    /// <inheritdoc />
    public float Rotation { get; set; }

    /// <inheritdoc />
    public Vector2 Position { get; set; }

    /// <inheritdoc />
    public Vector2 Velocity { get; set; }

    /// <inheritdoc />
    public int CollisionRadius { get; set; }

    /// <inheritdoc />
    public Vector2[] Vertices { get; set; } = Array.Empty<Vector2>();

    /// <inheritdoc />
    public Color Color { get; set; } = Color.White;

    /// <summary>
    /// 
    /// </summary>
    public AsteroidSize AsteroidSize { get; set; }
}

public enum AsteroidSize
{
    Small,
    Medium,
    Large,
}

