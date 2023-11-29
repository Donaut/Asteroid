using AsteroidCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidCore;

public sealed class AsteroidOptions
{
    /// <inheritdoc cref="IEntity.Scale"/>
    public float Scale { get; set; }

    /// <inheritdoc cref="IEntity.CollisionRadius"/>
    public int CollisionRadius { get; set; }

    /// <inheritdoc cref="IEntity.Color"/>
    public Color Color { get; set; }

    /// <inheritdoc cref="Asteroid.AsteroidSize"/>
    public AsteroidSize AsteroidSize { get; set; }

    public int Speed { get; set; }
}

public sealed class PlayerOptions
{
    /// <inheritdoc cref="IEntity.Scale"/>
    public float Scale { get; set; }

    /// <inheritdoc cref="IEntity.CollisionRadius"/>
    public int CollisionRadius { get; set; }

    /// <inheritdoc cref="IEntity.Color"/>
    public Color Color { get; set; }

    /// <summary>
    /// The rate at which the player accelerates per second.
    /// </summary>
    public float Speed { get; set; } = 500;

    /// <summary>
    /// The maximum rate at which the player can accelerate.
    /// </summary>
    public float MaxSpeed { get; set; } = 150;
}

public sealed class BulletOptions
{
    /// <inheritdoc cref="IEntity.Scale"/>
    public float Scale { get; set; }

    /// <inheritdoc cref="IEntity.CollisionRadius"/>
    public int CollisionRadius { get; set; }

    /// <inheritdoc cref="IEntity.Color"/>
    public Color Color { get; set; }

    /// <summary>
    /// The rate at which moves per second.
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    /// The lifetime of the bullet in seconds.
    /// </summary>
    public float LifeTime { get; set; }
}

/// <summary>
/// Options that can be set for the game.
/// IMPORTANT: Most values are read once when the game starts changing it dynamically can cause issues.
/// TODO: Make the ship rotation into a variable make all the Random.Shared access into a variable.
/// </summary>
public class GameOptions
{
    /// <summary>
    /// Options for the player character.
    /// </summary>
    public PlayerOptions PlayerOptions { get; set; }

    /// <summary>
    /// Options for the bullets shot by the player.
    /// </summary>
    public BulletOptions BulletOptions { get; set; }

    /// <summary>
    /// Options for the small asteroid.
    /// </summary>
    public AsteroidOptions SmallAsteroidOptions { get; set; }

    /// <summary>
    /// Options for the medium asteroid.
    /// </summary>
    public AsteroidOptions MediumAsteroidOptions { get; set; }

    /// <summary>
    /// Options for the large asteroid.
    /// </summary>
    public AsteroidOptions LargeAsteroidOptions { get; set; }

    /// <summary>
    /// Number of asteroids to spawn at the beginning of the game.
    /// </summary>
    public int NumberOfAsteroids { get; set; } = 10;

    /// <summary>
    /// Number of seconds before the player can shoot again.
    /// </summary>
    public float ShootColdown { get; set; } = .3f;

    public GameOptions()
    {
        PlayerOptions = new PlayerOptions()
        {
            Scale = 5f,
            Speed = 200,
            MaxSpeed = 500,
            CollisionRadius = 10,
            Color = Color.Green
        };
        BulletOptions = new BulletOptions()
        {
            Scale = 1,
            Speed = 140,
            CollisionRadius = 10,
            Color = Color.White,
            LifeTime = 1
        };
        SmallAsteroidOptions = new AsteroidOptions()
        {
            Scale = 3,
            Speed = 32,
            CollisionRadius = 1,
            Color = Color.White,
            AsteroidSize = AsteroidSize.Small
        };
        MediumAsteroidOptions = new AsteroidOptions()
        {
            Scale = 5,
            Speed = 22,
            CollisionRadius = 5,
            Color = Color.White,
            AsteroidSize = AsteroidSize.Medium
        };
        LargeAsteroidOptions = new AsteroidOptions()
        {
            Scale = 10,
            Speed = 15,
            CollisionRadius = 10,
            Color = Color.White,
            AsteroidSize = AsteroidSize.Large
        };
    }
}
