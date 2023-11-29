using System;
using System.Numerics;

namespace AsteroidCore
{
    [Flags]
    public enum Input
    {
        /// <summary>
        /// Does nothing
        /// </summary>
        None = 0,

        /// <summary>
        /// Accelerate the ship
        /// </summary>
        Accelerate = 1,

        /// <summary>
        /// Rotate the ship clockwise
        /// </summary>
        RotateRight = 2,

        /// <summary>
        /// Rotate the ship counter clockwise
        /// </summary>
        RotateLeft = 4,

        /// <summary>
        /// Shot a bullett
        /// </summary>
        Shoot = 8,
    }
}
