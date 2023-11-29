using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidCore;

public interface IEntity
{
    /// <summary>
    /// The position of the player, it's the center.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// The velocity of the ship, each update the velocity is added to the position.
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    /// The Scale of the ship.
    /// </summary>
    public float Scale { get; set; }

    /// <summary>
    /// The rotation of the ship in radians.
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    /// All the entities have a sphere as a collider, you can use this variable to increase or decrease the collider size.
    /// The combination of <see cref="Scale"/>, <see cref="CollisionScale"/> and <see cref="Position"/> gives the final collider size and position.
    /// </summary>
    public int CollisionRadius { get; set; }

    /// <summary>
    /// The points that draw the outline of the shape.
    /// </summary>
    public Vector2[] Vertices { get; set; }

    /// <summary>
    /// The color of the entity.
    /// </summary>
    public Color Color { get; set; }
}

