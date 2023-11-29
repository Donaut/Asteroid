using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace AsteroidMonoGame
{
    internal class ExtendedSpriteBatch : SpriteBatch
    {
        private readonly Texture2D _texture;

        public ExtendedSpriteBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            _texture = new Texture2D(GraphicsDevice, 1, 1);
            _texture.SetData(new[] { Color.White });
        }

        public void DrawLine(Vector2 start, Vector2 end, Color color, int thickness = 1)
        {
            var edge = end - start;
            var angle = MathF.Atan2(edge.Y, edge.X);
            var origin = new Vector2(0f, .5f);
            var length = Vector2.Distance(start, end) + .5f;
            var scale = new Vector2(length, thickness);

            this.Draw(_texture, start, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }

        public void FillRectangle(Rectangle destinationRecrangle, Color color)
        {
            Draw(_texture, destinationRecrangle, color);
        }
    }
}
