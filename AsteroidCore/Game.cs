using System;
using System.Diagnostics;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using AsteroidCore.States;

namespace AsteroidCore;

interface ICommand<T>
{
    void Execute(T command);
}

public class GameEnded
{
    public int Points { get; set; }
}

public class GameWon
{
    public int Points { get; set; }
}

public class GameStart
{
    public GameOptions StartOptions { get; }

    public GameStart(GameOptions startOptions)
    {
        StartOptions = startOptions;
    }
}

/// <summary>
/// The size which the game is designed in. Used for scaling when we scale the content to the real size.
/// </summary>
interface IWindowSize
{
    int Width { get; }

    int Height { get; }
}

public class Game : IWindowSize, ICommand<GameEnded>, ICommand<GameStart>
{
    private bool _initialized;

    private IState? _activeState;
    private IState? _nextState;

    private MenuState? _menuState;
    private GameCore? _gameState;

    private bool _transitioning;
    private float _duration;
    private float _decrementAmount;
    private float _opacity;
    private float _changeDelay;

    public bool IsInitalized => _initialized;

    /// <inheritdoc />
    public int Width => 300;

    /// <inheritdoc />
    public int Height => 300;

    public Game()
    {
    }

    /// <summary>
    /// Just like the constructor initalizes values that are readonly for the object lifetime. 
    /// But calling virutal methods in the constructor is not recommended so we do initialization logic inside this method.
    /// </summary>
    public virtual void Initialize()
    {
        if (IsInitalized)
            return;

        _gameState = new GameCore(this, this);
        _menuState = new MenuState(this, this);

        _gameState.Initialize();
        _menuState.Initialize();

        _initialized = true;

        LoadState(_menuState);
    }

    /// <summary>
    /// Updates the game
    /// </summary>
    /// <param name="elapsedSeconds">The elapsed seconds since the last update</param>
    /// <param name="action">The action that the user made</param>
    public virtual void Update(float elapsedSeconds, Input action)
    {
        ThrowIfNotInitialized();

        if(_transitioning)
        {
            if(_duration <= _changeDelay)
            {
                _activeState = _nextState;

                _nextState = null;
                _transitioning = false;
            }
            _duration -= elapsedSeconds;
            _opacity -= _decrementAmount * elapsedSeconds;
            return;
        }

        _activeState.Update(elapsedSeconds, action);
    }

    /// <summary>
    /// Draws the game
    /// </summary>
    /// <param name="renderer">The renderer to use, for drawing.</param>
    public void Draw<T>(T renderer) where T : IRenderer
    {
        ThrowIfNotInitialized();

        renderer.BeginDraw();
        try
        {
            if (_transitioning)
            {
                var newRenderer = new OpcaityRenderer<T>(renderer, _opacity);
                DrawBackGround(newRenderer);
                _activeState.Draw(newRenderer);

                return;
            }

            DrawBackGround(renderer);
            _activeState.Draw(renderer);
        }
        finally
        {
            renderer.EndDraw();
        }
    }

    protected virtual void DrawBackGround<T>(T renderer) where T : IRenderer
    {
        var rectangle = new Rectangle(0, 0, Width, Height);
        renderer.FillRectangle(rectangle, Color.Black);
    }

    void ICommand<GameEnded>.Execute(GameEnded command)
    {
        ThrowIfNotInitialized();

        LoadState(_menuState);
    }

    void ICommand<GameStart>.Execute(GameStart command)
    {
        ThrowIfNotInitialized();

        _gameState.Reset();
        LoadState(_gameState);
    }

    /// <inheritdoc />
    public void LoadState(IState state)
    {
        ThrowIfNotInitialized();

        // This will happen when the game loads.
        if (_activeState == null)
        {
            _activeState = state;
            return;
        }

        // If a transition is already playing we will log it.
        if (_transitioning)
        {
            Debug.Fail("TODO: Something bad happened!");
            return;
        }
        else
        {
            _duration = .5f;
            _opacity = 1;
            _decrementAmount = _opacity / _duration;
            _changeDelay = -.2f;
            _transitioning = true;
        }

        _nextState = state;
    }

    internal void LoadNextState()
    {
        Debug.Fail("Here!");
        if (_nextState == null)
        {

        }
        _activeState = _nextState;
        _nextState = null;
    }

    [MemberNotNull(nameof(_activeState), nameof(_gameState), nameof(_menuState))]
    [Conditional("DEBUG")]
    private void ThrowIfNotInitialized()
    {
        if (_initialized)
        {
            return;
        }

        throw new InvalidOperationException($"Call {nameof(Initialize)}() before calling any other method.");
    }

    readonly struct OpcaityRenderer<T> : IRenderer
        where T : IRenderer
    {
        private readonly T _renderer;
        private readonly float _opacity;

        public OpcaityRenderer(T renderer, float opacity)
        {
            _renderer = renderer;
            _opacity = Math.Clamp(opacity, 0, 1);
        }

        public void BeginDraw() { }

        public void EndDraw() { }

        public void DrawLines(ReadOnlySpan<Vector2> vertices, float scale, float rotation, Vector2 position, Color color)
        {
            var A = (int)(color.A * _opacity);
            var R = (int)(color.R * _opacity);
            var G = (int)(color.G * _opacity);
            var B = (int)(color.B * _opacity);
            var newColor = Color.FromArgb(A, R, G, B);
            _renderer.DrawLines(vertices, scale, rotation, position, newColor);
        }

        public void FillRectangle(Rectangle rectangle, Color color)
        {
            _renderer.FillRectangle(rectangle, color); // Background stays the same!
        }
    }
}

