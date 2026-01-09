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
        private TextureRegion _snake;
        private TextureRegion _apple;

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

            _snake = atlas.GetRegion("snake-head-right");
            _apple = atlas.GetRegion("apple");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin the sprite batch to prepare for rendering.
            SpriteBatch.Begin();

            // Draw the slime texture region at a scale of 4.0
            _snake.Draw(SpriteBatch, Vector2.Zero, Color.White, 0.0f, Vector2.One, 1.0f, SpriteEffects.None, 0.0f);

            // Draw the bat texture region 10px to the right of the slime at a scale of 4.0
            _apple.Draw(SpriteBatch, new Vector2(_snake.Width * 4.0f + 10, 0), Color.White, 0.0f, Vector2.One, 1.0f, SpriteEffects.None, 1.0f);

            // Always end the sprite batch when finished.
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
