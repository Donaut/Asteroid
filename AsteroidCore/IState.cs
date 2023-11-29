namespace AsteroidCore;

public interface IState
{
    /// <inheritdoc cref="Game.Initialize"/>
    void Initialize();

    /// <inheritdoc cref="Game.Update(float, Input)"/>
    void Update(float elapsedSeconds, Input action);

    /// <inheritdoc cref="Game.Draw{T}(T)"/>
    void Draw<TRenderer>(TRenderer renderer) where TRenderer : IRenderer;
}
