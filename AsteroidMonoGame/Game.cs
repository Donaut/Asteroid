using AsteroidCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Vector2 = System.Numerics.Vector2;

namespace AsteroidMonoGame
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private ExtendedSpriteBatch? _spriteBatch;
        private MonoGameRenderer? _renderer;
        private AsteroidCore.Game? _game;

        public Game() : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true; // Just to flex on people, we can resize the window.

            
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);

        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
        }

        protected override void Initialize()
        {   
            
            base.Initialize();
        }

        /// <summary>
        /// The <see cref="LoadContent"/> method is called by the <see cref="Initialize"/> method internally if it manages to find a <see cref="GraphicsDevice"/> instance.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new ExtendedSpriteBatch(GraphicsDevice);
            
            _game = new AsteroidCore.Game();
            _game.Initialize();


            _renderer = new MonoGameRenderer(_game, Window, GraphicsDevice, _spriteBatch);
            _renderer.Initialize();
            
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
                Exit();

            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var action = Input.None;
            if (keyboard.IsKeyDown(Keys.W))
                action |= Input.Accelerate;
            if (keyboard.IsKeyDown(Keys.A))
                action |= Input.RotateLeft;
            if (keyboard.IsKeyDown(Keys.D))
                action |= Input.RotateRight;
            if (keyboard.IsKeyDown(Keys.Space))
                action |= Input.Shoot;

            _game?.Update(elapsedTime, action);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(90, 62, 81));

            _game?.Draw(_renderer!);

            base.Draw(gameTime);
        }
    }
}