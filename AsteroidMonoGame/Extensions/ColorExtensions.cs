using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidMonoGame.Extensions
{
    internal static class ColorExtensions
    {
        public static Microsoft.Xna.Framework.Color ToXnaColor(this System.Drawing.Color @this)
        {
            return new(@this.R, @this.G, @this.B, @this.A);
        }
    }
}
