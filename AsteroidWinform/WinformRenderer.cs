using AsteroidCore;
using System.Numerics;



namespace AsteroidWinform;

/// <summary>
/// Please dont store this struct inside a field. Its uses the <see cref="Graphics"/> objects which becomes invalid after the drawing has finished.
/// </summary>
readonly struct WinformRenderer : IRenderer
{
    private readonly Graphics _graphics;
    private readonly Matrix3x2 _transform;

    public WinformRenderer(Graphics graphics, Matrix3x2 transform)
    {
        _graphics = graphics;
        _transform = transform;
    }

    public void BeginDraw()
    {
    }

    public void EndDraw()
    { 
    }

    public void FillRectangle(Rectangle rectangle, Color color)
    {
        // The _transform matrix contains both the offset and the scaling of the view.
        var xOffset = (int)_transform.M31;
        var yOffset = (int)_transform.M32;
        rectangle.X += xOffset;
        rectangle.Y += yOffset;

        var xScale = (int)_transform.M11;
        var yScale = (int)_transform.M22;
        rectangle.Width *= xScale;
        rectangle.Height *= yScale;

        using var brush = new SolidBrush(color);
        _graphics.FillRectangle(brush, rectangle);
        brush.Dispose();
    }

    public void DrawLines(ReadOnlySpan<Vector2> vertices, float scale, float rotation, Vector2 position, Color color)
    {
        if (vertices.Length == 0)
            return;
        
        var model = Matrix3x2.Identity;
        model *= Matrix3x2.CreateScale(scale);
        model *= Matrix3x2.CreateRotation(rotation);
        model *= Matrix3x2.CreateTranslation(position);
        model *= _transform;

        using var pen = new Pen(color);

        var first = Vector2.Transform(vertices[0], model);
        var previous = first;

        for (int i = 1; i < vertices.Length; i++)
        {
            var current = Vector2.Transform(vertices[i], model);
            _graphics.DrawLine(pen, previous.X, previous.Y, current.X, current.Y);
            //_spriteBatch.DrawLine(previous, current, color);
            previous = current;
        }

        // Finish connecting the lines, connect the first and last line.
        _graphics.DrawLine(pen, previous.X, previous.Y, first.X, first.Y);
    }
}