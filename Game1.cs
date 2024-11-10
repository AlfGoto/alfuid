using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Player;
using System;
using System.IO;

namespace alfuid
{
    public class Game1 : Game
    {


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KnightClass Knight = new();


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            Knight.Initialize(_graphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load knight sprite sheet
            Knight.knightSpriteSheet = Content.Load<Texture2D>("Knight SpriteSheet/Hero-idle-Sheet");
            Knight.knightWalkingSpriteSheet = Content.Load<Texture2D>("Knight SpriteSheet/Hero-walk-Sheet");
            Knight.knightAttackingSpriteSheet = Content.Load<Texture2D>("Knight SpriteSheet/Hero-attack-Sheet");
            Knight.knightDyingSpriteSheet = Content.Load<Texture2D>("Knight SpriteSheet/Hero-die-Sheet");
        }

        protected override void Update(GameTime gameTime)
        {
            Knight.Update(Exit, gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            Knight.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}