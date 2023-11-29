
using System;
using System.Drawing;
using System.Numerics;

namespace AsteroidCore;

public interface IRenderer
{
    void BeginDraw();
     
    void EndDraw();

    void FillRectangle(Rectangle rectangle, Color color);

    void DrawLines(ReadOnlySpan<Vector2> vertices, float scale, float rotation, Vector2 position, Color color);
}