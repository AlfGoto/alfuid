using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace alfuid
{
    public class Game1 : Game
    {
        Texture2D knightSpriteSheet;
        Texture2D knightWalkingSpriteSheet;
        Texture2D knightAttackingSpriteSheet;
        Vector2 knightPosition;
        float knightSpeed;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly string positionFilePath = "knightPosition.txt";
        // Idle animation variables
        int idleFrameWidth = 24;
        int idleFrameHeight = 24;
        int idleTotalFrames = 2;
        int idleCurrentFrame = 0;
        float idleTimeElapsed;
        float idleTimePerFrame = 0.3f;

        int walkFrameWidth = 48;
        int walkFrameHeight = 24;
        int walkTotalFrames = 6;
        int walkCurrentFrame = 0;
        float walkTimeElapsed;
        float walkTimePerFrame = 0.1f;

        int attackFrameWidth = 48;
        int attackFrameHeight = 24;
        int attackTotalFrames = 8;
        int attackCurrentFrame = 0;
        float attackTimeElapsed;
        float attackTimePerFrame = 0.05f;
        bool isAttacking = false;
        bool isWalking = false;
        bool isFacingLeft = false;

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

            if (File.Exists(positionFilePath))
            {
                using (StreamReader reader = new StreamReader(positionFilePath))
                {
                    float.TryParse(reader.ReadLine(), out float x);
                    float.TryParse(reader.ReadLine(), out float y);
                    knightPosition = new Vector2(x, y);
                }
            }
            else
            {
                knightPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            }
            knightSpeed = 300f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load knight sprite sheet
            knightSpriteSheet = Content.Load<Texture2D>("Knight SpriteSheet/Hero-idle-Sheet");
            knightWalkingSpriteSheet = Content.Load<Texture2D>("Knight SpriteSheet/Hero-walk-Sheet");
            knightAttackingSpriteSheet = Content.Load<Texture2D>("Knight SpriteSheet/Hero-attack-Sheet");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                using (StreamWriter writer = new StreamWriter(positionFilePath))
                {
                    writer.WriteLine(knightPosition.X);
                    writer.WriteLine(knightPosition.Y);
                }
                Exit();
            }



            if (isAttacking)
            {
                attackTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (attackTimeElapsed >= attackTimePerFrame)
                {
                    attackTimeElapsed -= attackTimePerFrame;
                    attackCurrentFrame++;

                    if (attackCurrentFrame >= attackTotalFrames)
                    {
                        isAttacking = false;
                        attackCurrentFrame = 0;
                    }
                }
            }
            else if (isWalking)
            {
                walkTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (walkTimeElapsed >= walkTimePerFrame)
                {
                    walkTimeElapsed -= walkTimePerFrame;
                    walkCurrentFrame = (walkCurrentFrame + 1) % walkTotalFrames;
                }
            }
            else
            {
                idleTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (idleTimeElapsed >= idleTimePerFrame)
                {
                    idleTimeElapsed -= idleTimePerFrame;
                    idleCurrentFrame = (idleCurrentFrame + 1) % idleTotalFrames;
                }
            }

            float updatedKnightSpeed = knightSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (isAttacking) updatedKnightSpeed /= 4f;
            var kstate = Keyboard.GetState();

            if (Mouse.GetState().LeftButton == ButtonState.Pressed) { }

            if ((kstate.IsKeyDown(Keys.Enter) || Mouse.GetState().LeftButton == ButtonState.Pressed) && !isAttacking)
            {
                isAttacking = true;
                attackCurrentFrame = 0;
                attackTimeElapsed = 0f;
            }

            if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.Z))
            {
                knightPosition.Y -= kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.Q) ||
                                    kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D)
                                    ? updatedKnightSpeed * 0.70f : updatedKnightSpeed;
            }

            if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S))
            {
                knightPosition.Y += kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.Q) ||
                                    kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D)
                                    ? updatedKnightSpeed * 0.70f : updatedKnightSpeed;
            }

            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.Q))
            {
                isFacingLeft = true;
                knightPosition.X -= kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.Z) ||
                                    kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S)
                                    ? updatedKnightSpeed * 0.70f : updatedKnightSpeed;
            }

            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
            {
                isFacingLeft = false;
                knightPosition.X += kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.Z) ||
                                    kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S)
                                    ? updatedKnightSpeed * 0.70f : updatedKnightSpeed;
            }


            if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.Z)
            || kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S)
            || kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.Q)
            || kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D)) { isWalking = true; }
            else if (kstate.IsKeyDown(Keys.Right)) { isWalking = true; }
            else { isWalking = false; }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Rectangle sourceRectangle;
            Texture2D spriteSheetToUse;
            float spriteDecal;

            if (isAttacking)
            {
                sourceRectangle = new Rectangle(attackCurrentFrame * attackFrameWidth, 0, attackFrameWidth, attackFrameHeight);
                spriteSheetToUse = knightAttackingSpriteSheet;
                spriteDecal = walkFrameWidth / 2;
            }
            else if (isWalking)
            {
                sourceRectangle = new Rectangle(walkCurrentFrame * walkFrameWidth, 0, walkFrameWidth, walkFrameHeight);
                spriteSheetToUse = knightWalkingSpriteSheet;
                spriteDecal = walkFrameWidth / 2;
            }
            else
            {
                sourceRectangle = new Rectangle(idleCurrentFrame * idleFrameWidth, 0, idleFrameWidth, idleFrameHeight);
                spriteSheetToUse = knightSpriteSheet;
                spriteDecal = idleFrameWidth / 2;
            }
            Vector2 scale = new Vector2(2.0f, 2.0f);

            SpriteEffects spriteEffects = isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            _spriteBatch.Begin();

            _spriteBatch.Draw(
                spriteSheetToUse,
                knightPosition,
                sourceRectangle,
                Color.White,
                0f,
                new Vector2(spriteDecal, idleFrameHeight / 2),
                scale,
                spriteEffects,
                0f
            );

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}