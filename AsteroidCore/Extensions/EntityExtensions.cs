using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidCore.Extensions;

internal static class EntityExtensions
{

    public static bool CirclesIntersects<TEntity1, TEntity2>(TEntity1 entity1, TEntity2 entity2)
        where TEntity1 : IEntity
        where TEntity2 : IEntity
    {
        return MathExtensions.CirclesIntersects(entity1.Position, entity1.CollisionRadius, entity2.Position, entity2.CollisionRadius);
    }

    /// <summary>
    /// Makes the entity appear at the other side if it leaves the map.
    /// </summary>
    /// <param name="entity"></param>
    public static void WrapPosition<T>(this T @this, int width, int height) where T : IEntity
    {
        var position = @this.Position;
        var radius = @this.CollisionRadius;

        var wrappedX = @this.Position.X;
        var wrappedY = @this.Position.Y;

        if (position.X + radius < 0)
            wrappedX = width + radius;
        else if (position.X - radius > width)
            wrappedX = 0 - radius;


        if (position.Y + radius < 0)
            wrappedY = height + radius;
        else if (position.Y - radius > height)
            wrappedY = 0 - radius;

        @this.Position = new Vector2(wrappedX, wrappedY);
    }
}
