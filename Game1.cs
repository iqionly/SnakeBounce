using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using System;
using System.Collections.Generic;

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

        private Vector2 _lastDirection = Vector2.UnitY;

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
            float speed = MOVEMENT_SPEED;
            if (Input.Keyboard.IsKeyDown(Keys.Space))
            {
                speed *= 1.5f;
            }

            if (Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up))
            {
                _snakePosition.Y -= speed;
            }
            else if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
            {
                _snakePosition.X += speed;
            }
            else if (Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down))
            {
                _snakePosition.Y += speed;
            }
            else if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
            {
                _snakePosition.X -= speed;
            }
        }

        private void CheckGamePadInput()
        {
            GamePadInfo gamePadOne = Input.GamePads[(int)PlayerIndex.One];

            // If the A button is held down, the movement speed increases by 1.5
            // and the gamepad vibrates as feedback to the player.
            float speed = MOVEMENT_SPEED;
            if (gamePadOne.IsButtonDown(Buttons.A))
            {
                speed *= 1.5f;
                gamePadOne.SetVibration(1.0f, TimeSpan.FromSeconds(1));
            }
            else
            {
                gamePadOne.StopVibration();
            }

            // Check thumbstick first since it has priority over which gamepad input
            // is movement.  It has priority since the thumbstick values provide a
            // more granular analog value that can be used for movement.
            if (gamePadOne.LeftThumbStick != Vector2.Zero)
            {
                _snakePosition.X += gamePadOne.LeftThumbStick.X * speed;
                _snakePosition.Y -= gamePadOne.LeftThumbStick.Y * speed;
            }
            else
            {
                // If DPadUp is down, move the slime up on the screen.
                if (gamePadOne.IsButtonDown(Buttons.DPadUp))
                {
                    _snakePosition.Y -= speed;
                }

                // If DPadDown is down, move the slime down on the screen.
                if (gamePadOne.IsButtonDown(Buttons.DPadDown))
                {
                    _snakePosition.Y += speed;
                }

                // If DPapLeft is down, move the slime left on the screen.
                if (gamePadOne.IsButtonDown(Buttons.DPadLeft))
                {
                    _snakePosition.X -= speed;
                }

                // If DPadRight is down, move the slime right on the screen.
                if (gamePadOne.IsButtonDown(Buttons.DPadRight))
                {
                    _snakePosition.X += speed;
                }
            }
        }

        private void CheckKeyboardInputBuffer()
        {
            // In the input handling code:
            Vector2 newDirection = Vector2.Zero;

            float speed = MOVEMENT_SPEED;
            if (Input.Keyboard.IsKeyDown(Keys.Space))
            {
                speed *= 1.5f;
            }

            if (_lastDirection != Vector2.UnitY && (Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up)))
            {
                newDirection = -Vector2.UnitY;
            }
            else if (_lastDirection != -Vector2.UnitY && (Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down)))
            {
                newDirection = Vector2.UnitY;
            }
            else if (_lastDirection != Vector2.UnitX && (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left)))
            {
                newDirection = -Vector2.UnitX;
            }
            else if (_lastDirection != -Vector2.UnitX && (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right)))
            {
                newDirection = Vector2.UnitX;
            }

            // Only add if a valid direction and does not exceed the buffer size.
            if (newDirection != Vector2.Zero && _inputBuffer.Count < MAX_BUFFER_SIZE)
            {
                _inputBuffer.Enqueue(newDirection);
            }

            // In movement update code.
            if (_inputBuffer.Count > 1)
            {
                _lastDirection = _inputBuffer.Dequeue();
            }
            _snakePosition += _lastDirection * speed;
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
