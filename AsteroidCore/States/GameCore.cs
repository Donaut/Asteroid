using AsteroidCore.Entities;
using AsteroidCore.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AsteroidCore.States;

/// <summary>
/// The <see cref="Game"/> class is a facade to set things up and provide a unified system for the game.
/// Game core is the real thing when the player is inside the game.
/// </summary>
internal class GameCore : IState
{
    private bool _initialized = false;

    private readonly IWindowSize _window;
    private readonly ICommand<GameEnded> _gameEndedCommand;

    private float _canShoot; // Resource!
    private Player? _player; // Resource!
    private readonly List<Bullet> _bullets = new(); // Resource!
    private readonly List<Asteroid> _asteroids = new(); // Resource!

    /// <summary>
    /// The options for the game. Like hardness.
    /// </summary>
    public GameOptions Options { get; set; } = new GameOptions();

    public GameCore(IWindowSize window, ICommand<GameEnded> gameEndedCommand)
    {
        _window = window;
        _gameEndedCommand = gameEndedCommand;
    }

    public void Initialize()
    {
        if (_initialized) 
            return;

        _player = CreatePlayer(Options.PlayerOptions);

        _initialized = true;
    }

    public virtual void Reset()
    {
        ThrowIfNotInitialized();

        var width = _window.Width;
        var height = _window.Height;
        var center = new Vector2(width / 2, height / 2);

        // Reset asteroids
        _asteroids.Clear();
        var minRadius = 100;
        var maxRadius = Math.Max(width, height);
        var asteroidOptions = Options.LargeAsteroidOptions;
        for (int i = 0; i < Options.NumberOfAsteroids; i++)
        {
            var asteroidPosition = MathExtensions.GenerateRandomPointInRing(minRadius, maxRadius) + center;
            var asteroidVelocity = VectorExtensions.FromAngle(Random.Shared.NextSingle(0, MathF.Tau)) * asteroidOptions.Speed;
            var asteroid = CreateAsteroid(asteroidOptions, asteroidPosition, asteroidVelocity);

            _asteroids.Add(asteroid);
        }

        // Reset player
        _player.Position = center;
        _player.Rotation = 0;
        _player.Velocity = Vector2.Zero;

        //Reset shoting
        _canShoot = 0;
    }

    /// <inheritdoc cref="Game.Update(float, Input)"/>
    public void Update(float elapsedSeconds, Input input)
    {
        ThrowIfNotInitialized();

        _canShoot += elapsedSeconds;
        var isDead = false;

        // Handle collisions and remove bullets that are end of lifetime.
        for (var asteroidIndex = _asteroids.Count - 1; asteroidIndex >= 0; asteroidIndex--)
        {
            var asteroid = _asteroids[asteroidIndex];

            if (MathExtensions.CirclesIntersects(_player, asteroid))
            {
                isDead = true;
            }

            for (var bulletIndex = _bullets.Count - 1; bulletIndex >= 0; bulletIndex--)
            {
                var bullet = _bullets[bulletIndex];

                if (bullet.LifeTime < 0)
                {
                    _bullets.RemoveBySwap(bulletIndex);
                    continue;
                }

                if (MathExtensions.CirclesIntersects(asteroid, bullet))
                {
                    if (asteroid.AsteroidSize == AsteroidSize.Medium || asteroid.AsteroidSize == AsteroidSize.Large)
                    {
                        var angle = _player.Rotation + MathF.PI;

                        // The idea is that new asteroids will always go away from the player never towards it,
                        // so we select the angle that are pointing towards the player and widen it. Then we generate a random point ouitside that angle range.
                        var angleOffset = (MathF.Tau / 360) * 90;
                        var angleStart = angle + angleOffset;
                        var angleEnd = angle + MathF.Tau - angleOffset;

                        var asteroidOptions = asteroid.AsteroidSize == AsteroidSize.Medium ? Options.SmallAsteroidOptions : Options.MediumAsteroidOptions;

                        for (var ii = 0; ii < 2; ii++)
                        {
                            var asteroidPosition = MathExtensions.GenerateRandomPointInRing(0, 5) + asteroid.Position;
                            var asteroidVelocity = VectorExtensions.FromAngle(Random.Shared.NextSingle(angleStart, angleEnd)) * asteroidOptions.Speed;
                            var newAsteroid = CreateAsteroid(asteroidOptions, asteroidPosition, asteroidVelocity);

                            _asteroids.Add(newAsteroid);
                        }
                    }

                    _asteroids.RemoveBySwap(asteroidIndex);
                    _bullets.RemoveBySwap(bulletIndex);

                    break;
                }
            }
        }

        if (_asteroids.Count == 0)
        {
            //GameEnded?.Invoke(this, EventArgs.Empty);
            throw new NotImplementedException();
        }
        else if(isDead)
        {
            _gameEndedCommand.Execute(new GameEnded()
            {
                Points = int.MaxValue
            });
        }

        // Update Ship
        if (input.HasFlag(Input.RotateLeft))
        {
            _player.Rotation -= MathF.Tau * elapsedSeconds;
        }
        if (input.HasFlag(Input.RotateRight))
        {
            _player.Rotation += MathF.Tau * elapsedSeconds;
        }
        if (input.HasFlag(Input.Shoot) && CanShoot())
        {
            var bullet = CreateBullett(Options.BulletOptions);
            bullet.Position = _player.Position;
            bullet.Velocity = VectorExtensions.FromAngle(_player.Rotation) * Options.BulletOptions.Speed;

            _bullets.Add(bullet);

            _canShoot = 0; // Reset timer so the player cant shot every frame.
        }
        if (input.HasFlag(Input.Accelerate))
        {
            var direction = VectorExtensions.FromAngle(_player.Rotation);
            _player.Velocity += direction * Options.PlayerOptions.Speed * elapsedSeconds;
        }
        else
        {
            _player.Velocity *= .99f;
        }

        // Update position based on velocity
        _player.Velocity = VectorExtensions.MaxLength(_player.Velocity, Options.PlayerOptions.MaxSpeed);
        _player.Position += _player.Velocity * elapsedSeconds;

        foreach (var asteroid in _asteroids)
            asteroid.Position += asteroid.Velocity * elapsedSeconds;
        foreach (var bullet in _bullets)
            bullet.Position += bullet.Velocity * elapsedSeconds;

        // Wrap-around entities if they leave the map.
        var width = _window.Width;
        var height = _window.Height;
        _player.WrapPosition(width, height);
        foreach (var asteroid in _asteroids)
            asteroid.WrapPosition(width, height);
        foreach (var bullet in _bullets)
        {
            bullet.WrapPosition(width, height);
            bullet.LifeTime -= elapsedSeconds;
        }

    }

    /// <inheritdoc cref="Game.Draw{T}(T)"/>
    public void Draw<TRenderer>(TRenderer renderer) where TRenderer : IRenderer
    {
        ThrowIfNotInitialized();

        foreach (var asteroid in _asteroids)
            renderer.DrawEntity(asteroid);
        foreach (var bullet in _bullets)
            renderer.DrawEntity(bullet);
        renderer.DrawEntity(_player);
    }

    /// <summary>
    /// Determines if the player can shoot.
    /// </summary>
    /// <returns><see langword="true"/> if the player can shoot otherwise <see langword="false"/></returns>
    protected virtual bool CanShoot() => _canShoot >= Options.ShootColdown;

    /// <summary>
    /// Creates the Player.
    /// </summary>
    /// <returns></returns>
    protected virtual Player CreatePlayer(PlayerOptions options)
    {
        var player = new Player()
        {
            Scale = options.Scale,
            CollisionRadius = options.CollisionRadius,
            Color = options.Color,
            Vertices = new Vector2[]
            {
                new(2, 0),
                new(-1, -1),
                new(-.5f, -.8f),
                new(-.5f, .8f),
                new(-1f, 1f),
            }
        };

        return player;
    }

    /// <summary>
    /// Creates a bullet.
    /// </summary>
    /// <returns></returns>>
    protected virtual Bullet CreateBullett(BulletOptions options)
    {
        var bullet = new Bullet
        {
            Scale = options.Scale,
            CollisionRadius = options.CollisionRadius,
            Color = options.Color,
            Vertices = new Vector2[]
            {
                new(-.5f, -.5f),
                new(.5f, -.5f),
                new(.5f, .5f),
                new(-.5f, .5f),
            },
            LifeTime = options.LifeTime
        };

        return bullet;
    }

    /// <summary>
    /// Creates a asteroid.
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    protected virtual Asteroid CreateAsteroid(AsteroidOptions options, Vector2 position, Vector2 velocity)
    {
        var asteroid = new Asteroid
        {
            Scale = options.Scale,
            Position = position,
            Velocity = velocity,
            CollisionRadius = options.CollisionRadius,
            Color = options.Color,
            Vertices = CreateAsteroidShape(),
            AsteroidSize = options.AsteroidSize
        };

        return asteroid;
    }

    /// <summary>
    /// Createas a shape for the asteroids.
    /// </summary>
    /// <returns></returns>
    protected virtual Vector2[] CreateAsteroidShape()
    {
        var vertices = new Vector2[20];
        for (var i = 0; i < vertices.Length; i++)
        {
            var radius = Random.Shared.NextSingle() * .8f + .7f;
            var angle = ((float)i / vertices.Length) * MathF.Tau;

            vertices[i] = new Vector2(MathF.Sin(angle), MathF.Cos(angle)) * radius;
        }

        return vertices;
    }

    [MemberNotNull(nameof(Options), nameof(_player))]
    [Conditional("DEBUG")]
    private void ThrowIfNotInitialized()
    {
        Debug.Assert(Options != null);
        Debug.Assert(_player != null);
        Debug.Assert(_initialized, $"Call {nameof(Initialize)}() before calling any other method.");
    }
}