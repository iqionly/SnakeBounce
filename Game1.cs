using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SnakeBounce
{
    public class Game1 : Core
    {
        private AnimatedSprite _snake;
        private AnimatedSprite _apple;

        // Tracks the position of the slime.
        private Vector2 _snakePosition;

        // Speed multiplier when moving.
        private const float MOVEMENT_SPEED = 5.0f;

        // Use a queue directly for input buffering
        private Queue<Vector2> _inputBuffer;
        private const int MAX_BUFFER_SIZE = 2;

        public Game1() : base("SnakeBounce", 1280, 720, false)
        {

        }

        protected override void Initialize()
        {
            // In initialization code:
            _inputBuffer = new Queue<Vector2>(MAX_BUFFER_SIZE);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create the texture atlas from the XML configuration file
            TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

            _snake = atlas.CreateAnimatedSprite("snake-animation");

            _apple = atlas.CreateAnimatedSprite("apple-animation");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update snake animations
            _snake.Update(gameTime);

            // Update apple animations
            _apple.Update(gameTime);

            CheckKeyboardInputBuffer();

            base.Update(gameTime);
        }

        private void CheckKeyboardInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            float speed = MOVEMENT_SPEED;
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                speed *= 1.5f;
            }

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                _snakePosition.Y -= speed;
            }
            else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                _snakePosition.X += speed;
            }
            else if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                _snakePosition.Y += speed;
            }
            else if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                _snakePosition.X -= speed;
            }
        }

        private void CheckGamePadInput()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            // If the A button is held down, the movement speed increases by 1.5
            // and the gamepad vibrates as feedback to the player.
            float speed = MOVEMENT_SPEED;
            if (gamePadState.IsButtonDown(Buttons.A))
            {
                speed *= 1.5f;
                GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
            }
            else
            {
                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
            }

            // Check thumbstick first since it has priority over which gamepad input
            // is movement.  It has priority since the thumbstick values provide a
            // more granular analog value that can be used for movement.
            if (gamePadState.ThumbSticks.Left != Vector2.Zero)
            {
                _snakePosition.X += gamePadState.ThumbSticks.Left.X * speed;
                _snakePosition.Y -= gamePadState.ThumbSticks.Left.Y * speed;
            }
            else
            {
                // If DPadUp is down, move the slime up on the screen.
                if (gamePadState.IsButtonDown(Buttons.DPadUp))
                {
                    _snakePosition.Y -= speed;
                }

                // If DPadDown is down, move the slime down on the screen.
                if (gamePadState.IsButtonDown(Buttons.DPadDown))
                {
                    _snakePosition.Y += speed;
                }

                // If DPapLeft is down, move the slime left on the screen.
                if (gamePadState.IsButtonDown(Buttons.DPadLeft))
                {
                    _snakePosition.X -= speed;
                }

                // If DPadRight is down, move the slime right on the screen.
                if (gamePadState.IsButtonDown(Buttons.DPadRight))
                {
                    _snakePosition.X += speed;
                }
            }
        }

        private void CheckKeyboardInputBuffer()
        {
            // In the input handling code:
            KeyboardState keyboard = Keyboard.GetState();
            Vector2 newDirection = Vector2.Zero;

            float speed = MOVEMENT_SPEED;
            if (keyboard.IsKeyDown(Keys.Space))
            {
                speed *= 1.5f;
            }

            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
            {
                newDirection = -Vector2.UnitY;
            }
            else if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
            {
                newDirection = Vector2.UnitY;
            }
            else if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
            {
                newDirection = -Vector2.UnitX;
            }
            else if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
            {
                newDirection = Vector2.UnitX;
            }

            // Only add if a valid direction and does not exceed the buffer size.
            if (newDirection != Vector2.Zero && _inputBuffer.Count < MAX_BUFFER_SIZE)
            {
                _inputBuffer.Enqueue(newDirection);
            }

            // In movement update code.
            if (_inputBuffer.Count > 0)
            {
                Vector2 nextDirection = _inputBuffer.Dequeue();
                _snakePosition += nextDirection * speed;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin the sprite batch to prepare for rendering.
            SpriteBatch.Begin();

            // Draw the slime sprite.
            _snake.Draw(SpriteBatch, _snakePosition);

            // Draw the bat sprite 10px to the right of the slime.
            _apple.Draw(SpriteBatch, new Vector2(_snake.Width + 10, 0));

            // Always end the sprite batch when finished.
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
