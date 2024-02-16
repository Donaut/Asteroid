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

    /// <summary>
    /// Checks if two circles are intersecting.
    /// </summary>
    /// <param name="p1">Position of the first circle</param>
    /// <param name="r1">Radius of the first circle</param>
    /// <param name="p2">Position of the second circle</param>
    /// <param name="r2">Radius of the second circle</param>
    /// <returns><see langword="true"/> if the two circle intersects otherwise <see langword="false"/></returns>
    public static bool CirclesIntersects(Vector2 p1, float r1, Vector2 p2, float r2)
    {
        var dx = p1.X - p2.X;
        var dy = p1.Y - p2.Y;
        var radiusSum = r1 + r2;

        return radiusSum * radiusSum >= (dx * dx) + (dy * dy);
    }

    /// <summary>
    /// Its like generating points inside a circle but the circle center (minRadius) is forbidden.
    /// </summary>
    /// <param name="minRadius"></param>
    /// <param name="maxRadius"></param>
    /// <returns></returns>
    [Obsolete("This method uses Random.Shared, which makes this ")]
    public static Vector2 GenerateRandomPointInRing(float minRadius, float maxRadius)
    {
        return RandomExtensions.GenerateRandomPointInRing(minRadius, maxRadius, Random.Shared);
    }
}
