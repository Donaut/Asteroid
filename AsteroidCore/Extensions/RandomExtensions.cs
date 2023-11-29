using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidCore.Extensions;

internal static class RandomExtensions
{
    public static float NextSingle(this Random @this, float minimum, float maximum) => @this.NextSingle() * (maximum - minimum) + minimum;
}
