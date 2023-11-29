using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidCore.Entities;

public class Bullet : IEntity
{
    /// <inheritdoc />
    public float Scale { get; set; }

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
    /// The life time of the object in seconds if the life time reaches 0 the bullet is removed.
    /// </summary>
    public float LifeTime { get; set; }
}
