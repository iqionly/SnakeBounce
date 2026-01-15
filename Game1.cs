using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System;
using System.Diagnostics;

namespace SnakeBounce
{
    public class Game1 : Core
    {
        private AnimatedSprite _snake;
        private AnimatedSprite _apple;

        public Game1() : base("SnakeBounce", 1280, 720, false)
        {

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin the sprite batch to prepare for rendering.
            SpriteBatch.Begin();

            // Draw the slime sprite.
            _snake.Draw(SpriteBatch, Vector2.Zero);

            // Draw the bat sprite 10px to the right of the slime.
            _apple.Draw(SpriteBatch, new Vector2(_snake.Width + 10, 0));

            // Always end the sprite batch when finished.
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
