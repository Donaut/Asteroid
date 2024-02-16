using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidCore
{
    internal static class VectorExtensions
    {
        /// <summary>
        /// Creates a vector pointing towards the angle
        /// </summary>
        /// <param name="angle">The angle measured in radians.</param>
        /// <returns>No</returns>
        public static Vector2 FromAngle(float angle)
        {
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }

        public static Vector2 MaxLength(Vector2 vector, float maxLength)
        {
            var length = vector.Length();
            if (length > maxLength)
            {
                return Vector2.Normalize(vector) * maxLength;
            }

            return vector;
        }
    }
}
