using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidCore.Extensions;

/// <summary>
/// Common methods that are missing from the <see cref="Math"/> class.
/// </summary>
public static class MathExtensions
{
    /// <summary>
    /// Converts the specified degree into radian
    /// </summary>
    /// <returns>The degree converted into radian.</returns>
    public static float ToRadian(this float degree) => degree * ((float)Math.PI / 180f);

    public static bool CirclesIntersects<TEntity1, TEntity2>(TEntity1 entity1, TEntity2 entity2)
        where TEntity1 : IEntity
        where TEntity2 : IEntity
    {
        return CirclesIntersects(entity1.Position, entity1.CollisionRadius, entity2.Position, entity2.CollisionRadius);
    }

    public static bool CirclesIntersects(Vector2 p1, float r1, Vector2 p2, float r2)
    {
        var dx = p1.X - p2.X;
        var dy = p1.Y - p2.Y;
        var radiusSum = r1 + r2;

        return radiusSum * radiusSum >= (dx * dx) + (dy * dy);
    }

    public static Vector2 GenerateRandomPointInRing(float minRadius, float maxRadius)
    {
        return GenerateRandomPointInRing(minRadius, maxRadius, Random.Shared);
    }

    public static Vector2 GenerateRandomPointInRing(float minRadius, float maxRadius, Random random)
    {
        var randomRadius = minRadius + (maxRadius - minRadius) * random.NextSingle();
        var randomAngle = 2 * MathF.PI * random.NextSingle();

        var x = randomRadius * MathF.Cos(randomAngle);
        var y = randomRadius * MathF.Sin(randomAngle);

        return new Vector2(x, y);
    }

}
