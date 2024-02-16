using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidCore.Extensions;

internal static class RandomExtensions
{

    public static Vector2 GenerateRandomPointInRing(float minRadius, float maxRadius, Random random)
    {
        var randomRadius = minRadius + (maxRadius - minRadius) * random.NextSingle();
        var randomAngle = 2 * MathF.PI * random.NextSingle();

        var x = randomRadius * MathF.Cos(randomAngle);
        var y = randomRadius * MathF.Sin(randomAngle);

        return new Vector2(x, y);
    }
    public static float NextSingle(this Random @this, float minimum, float maximum) => @this.NextSingle() * (maximum - minimum) + minimum;
}
