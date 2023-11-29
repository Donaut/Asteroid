using AsteroidCore;
using AsteroidMonoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Numerics;

using XnaVector2 = Microsoft.Xna.Framework.Vector2;
using NumericsVector2 = System.Numerics.Vector2;

using XnaColor = Microsoft.Xna.Framework.Color;
using SystemColor = System.Drawing.Color;
using SystemRectangle = System.Drawing.Rectangle;

using XnaMatrix = Microsoft.Xna.Framework.Matrix;
using System;
using System.Diagnostics;
namespace AsteroidMonoGame;

internal class MonoGameRenderer : IRenderer
{
    private readonly AsteroidCore.Game _game;
    private readonly GameWindow _window;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly ExtendedSpriteBatch _spriteBatch;

    private XnaMatrix _transform = XnaMatrix.Identity;

    public MonoGameRenderer(AsteroidCore.Game game, GameWindow window, GraphicsDevice graphicsDevice, ExtendedSpriteBatch spriteBatch)
    {
        _game = game;
        _window = window;
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;

        window.ClientSizeChanged += (s, args) => { OnScreensizeChanged(); };
    }

    public void Initialize()
    {
        OnScreensizeChanged();
    }

    public void BeginDraw()
    {
        _spriteBatch.Begin(transformMatrix: _transform, samplerState: SamplerState.PointClamp);
    }

    public void FillRectangle(SystemRectangle rectangle, SystemColor color)
    {
        var xnaRectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        _spriteBatch.FillRectangle(xnaRectangle, color.ToXnaColor());
    }

    public void DrawLines(ReadOnlySpan<NumericsVector2> vertices, float scale, float rotation, NumericsVector2 position, SystemColor color)
    {
        if(vertices.Length == 0)
            return;

        var model = Matrix3x2.Identity;
        model *= Matrix3x2.CreateScale(scale);
        model *= Matrix3x2.CreateRotation(rotation);
        model *= Matrix3x2.CreateTranslation(position);
        
        var first = NumericsVector2.Transform(vertices[0], model);
        var previous = first;

        for (int i = 1; i < vertices.Length; i++)
        {
            var current = NumericsVector2.Transform(vertices[i], model);
            _spriteBatch.DrawLine(previous, current, color.ToXnaColor());
            previous = current;
        }

        // Finish connecting the lines, connect the first and last line.
        _spriteBatch.DrawLine(previous, first, color.ToXnaColor());
    }

    public void EndDraw()
    {
        _spriteBatch.End();
    }

    protected void OnScreensizeChanged()
    {
        var windowSize = _window.ClientBounds;

        // Both these values must be your real window size, so of course these values can't be static
        int screen_width = windowSize.Width;
        int screen_height = windowSize.Height;

        // This is your target virtual resolution for the game, the size you built your game to
        int virtual_width = _game.Width;
        int virtual_height = _game.Height;

        float targetAspectRatio = virtual_width / virtual_height;

        // figure out the largest area that fits in this resolution at the desired aspect ratio
        int width = screen_width;
        int height = (int)(width / targetAspectRatio + 0.5f);

        if (height > screen_height)
        {
            //It doesn't fit our height, we must switch to pillarbox then
            height = screen_height;
            width = (int)(height * targetAspectRatio + 0.5f);
        }

        var scale_x = width / virtual_width;
        var scale_y = height / virtual_height;

        width = virtual_width * scale_x;
        height = virtual_height * scale_y;

        // set up the new viewport centered in the backbuffer
        int vp_x = (screen_width / 2) - (width / 2);
        int vp_y = (screen_height / 2) - (height / 2);

        var newTrasnform = XnaMatrix.Identity;
        newTrasnform *= XnaMatrix.CreateScale(scale_x, scale_y, 1);
        // newTrasnform *= XnaMatrix.CreateTranslation(vp_x, vp_y, 1); Enable scissors also!

        _transform = newTrasnform;
        _graphicsDevice.Viewport = new Viewport(vp_x, vp_y, width, height);
    }

    
}
