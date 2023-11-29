using AsteroidCore;
using System.Diagnostics;
using System.Numerics;
namespace AsteroidWinform;

public partial class MainForm : Form
{
    private Matrix3x2 _transform;
    private readonly Game _game;
    private float _currentTime = (float)Stopwatch.GetTimestamp() / Stopwatch.Frequency;
    private Input _input;

    public MainForm()
    {
        InitializeComponent();

        _game = new Game();
        //_game.Initialize();

        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.UserPaint, true);

        SizeChanged += MainForm_SizeChanged;
        Application.Idle += GameLoop;
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        var newInput = Input.None;
        switch (e.KeyCode)
        {
            case Keys.W:
                newInput |= Input.Accelerate;
                break;
            case Keys.A:
                newInput |= Input.RotateLeft;
                break;
            case Keys.D:
                newInput |= Input.RotateRight;
                break;
            case Keys.Space:
                newInput |= Input.Shoot;
                break;
            default:
                break;
        }
        _input &= ~newInput; // If the user releases a key we remove it from the current inputs.
        base.OnKeyUp(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        var newInput = Input.None;
        switch (e.KeyCode)
        {
            case Keys.W:
                newInput |= Input.Accelerate;
                break;
            case Keys.A:
                newInput |= Input.RotateLeft;
                break;
            case Keys.D:
                newInput |= Input.RotateRight;
                break;
            case Keys.Space:
                newInput |= Input.Shoot;
                break;
            default:
                break;
        }

        _input |= newInput;
        base.OnKeyDown(e);
    }

    private void GameLoop(object? sender, EventArgs e)
    {
        float newTime = (float)Stopwatch.GetTimestamp() / Stopwatch.Frequency;
        float frameTime = newTime - _currentTime;
        _currentTime = newTime;

        while (frameTime > 0.0f)
        {
            float deltaTime = MathF.Min(frameTime, 1f / 60.0f);
            _game.Update(deltaTime, _input);
            frameTime -= deltaTime;
        }
        Invalidate(false);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        _game.Initialize();
        MainForm_SizeChanged(this, EventArgs.Empty);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var originalClip = e.Graphics.Clip;

        var x = (int)_transform.M31;
        var y = (int)_transform.M32;

        var width = (int)_transform.M11 * _game.Width;
        var height = (int)_transform.M22 * _game.Height;
        
        e.Graphics.SetClip(new Rectangle(x, y, width, height));
        _game.Draw(new WinformRenderer(e.Graphics, _transform));
        e.Graphics.Clip = originalClip;
    }

    private void MainForm_SizeChanged(object? sender, EventArgs e)
    {
        // Both these values must be your real window size, so of course these values can't be static
        int screen_width = Width;
        int screen_height = Height;

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

        var newTrasnform = Matrix3x2.CreateScale(scale_x, scale_y);
        newTrasnform *= Matrix3x2.CreateTranslation(vp_x, vp_y);

        _transform = newTrasnform;
    }
}
