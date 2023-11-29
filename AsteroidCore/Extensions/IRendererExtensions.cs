using AsteroidCore.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidCore.Extensions;

public static class IRendererExtensions
{
    private static readonly Dictionary<char, Vector2[]> _letters = new Dictionary<char, Vector2[]>();

    public static void DrawEntity<TRenderer, TEntity>(this TRenderer renderer, TEntity entity)
        where TRenderer : IRenderer
        where TEntity : IEntity
    {
        renderer.DrawLines(entity.Vertices, entity.Scale, entity.Rotation, entity.Position, entity.Color);
    }

    public static void DrawString<TRenderer>(this TRenderer renderer, string text, Vector2 position, int scale)
        where TRenderer : IRenderer
    {
        var characterSize = scale * 2;
        var spacing = 5;

        var offset = (text.Length / 2) * (characterSize + spacing);
        var start = position - new Vector2(offset, 0);
        foreach (var character in text)
        {
            if(!_letters.TryGetValue(character, out var vertices))
                continue;

            renderer.DrawLines(vertices, scale, 0, start, System.Drawing.Color.White);
            start.X += characterSize + spacing;

        }
    }

    static IRendererExtensions()
    {
        _letters['P'] = new[]
        {
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(-1, 1),
        };
        _letters['R'] = new[]
        {
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(1, 1),
            new Vector2(-1, 0),
            new Vector2(-1, 1),
        };
        _letters['E'] = new[]
        {
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, 0),
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(-1, 1),
            new Vector2(1, 1),
            new Vector2(-1, 1),
        };
        _letters['S'] = new[]
        {
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(-1, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, 0),
        };
        _letters['A'] = new[]
        {
            new Vector2(-1, 0),
            new Vector2(-1, 1),
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(1, 1),
            new Vector2(1, 0),
        };
        _letters['C'] = new[]
        {
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, 1),
            new Vector2(1, 1),
            new Vector2(-1, 1)
        };
        _letters['-'] = new[]
        {
            new Vector2(-1, 0),
            new Vector2(1, 0),
        };
        //_letters[' '] = Array.Empty<Vector2>();
        //_letters['\n'] = Array.Empty<Vector2>();
        //foreach (var key in _letters.Keys)
        //{
        //    _letters[key] = _letters[key].Select(x => x / 2).ToArray();
        //}
    }
}
