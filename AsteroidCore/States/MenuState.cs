using AsteroidCore.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AsteroidCore.States;

internal class MenuState : IState
{
    private readonly Vector2[] _boxVertices = new Vector2[]
    {
        new(-.5f, -.5f),
        new(.5f, -.5f),
        new(.5f, .5f),
        new(-.5f, .5f),
    };

    private IList<Vector2> _stars = Array.Empty<Vector2>();

    private readonly IWindowSize _window;
    private readonly ICommand<GameStart> _gameStartCommand;

    public MenuState(IWindowSize window, ICommand<GameStart> gameStartCommand)
    {
        _window = window;
        _gameStartCommand = gameStartCommand;
    }

    public void Initialize()
    {
        _stars = new List<Vector2>(25);

        var width = _window.Width;
        var height = _window.Height;
        for (int i = 0; i < 25; i++)
        {
            var x = Random.Shared.Next(0, width * height);
            var position = new Vector2(x % width, x / height);
            _stars.Add(position);
        }

        
    }

    public void Update(float elapsedSeconds, Input action)
    {
        for (int i = 0; i < _stars.Count; i++)
        {
            var star = _stars[i];
            star.Y = (star.Y + 10 * elapsedSeconds) % 300;
            _stars[i] = star;
        }

        if (action.HasFlag(Input.Shoot))
        {
            _gameStartCommand.Execute(new GameStart(new GameOptions()));
        }
    }

    public void Draw<TRenderer>(TRenderer renderer) where TRenderer : IRenderer
    {
        for (var i = 0; i < _stars.Count; i++)
        {
            var starPosition = _stars[i];
            renderer.DrawLines(_boxVertices, 1, 0, starPosition, System.Drawing.Color.White);
        }

        renderer.DrawString("PRESS", new Vector2(150, 150), 10);
        renderer.DrawString("-SPACE-", new Vector2(150, 175), 6);
    }
}
