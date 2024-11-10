
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace Player
{

  class KnightClass
  {
    Vector2 knightPosition;
    float knightSpeed;
    private readonly string positionFilePath = "knightPosition.txt";
    public Texture2D knightSpriteSheet;
    public Texture2D knightWalkingSpriteSheet;
    public Texture2D knightAttackingSpriteSheet;
    public Texture2D knightDyingSpriteSheet;
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

    int dyingFrameWidth = 24;
    int dyingFrameHeight = 24;
    int dyingTotalFrames = 12;
    int dyingCurrentFrame = 0;
    float dyingTimeElapsed;
    float dyingTimePerFrame = 0.05f;
    public bool isDying = false;
    public bool isAttacking = false;
    public bool isWalking = false;
    public bool isFacingLeft = false;

    public Rectangle sourceRectangle;
    public Texture2D spriteSheetToUse;
    public float spriteDecal;
    SpriteEffects spriteEffects;

    public void Initialize(GraphicsDeviceManager _graphics)
    {
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
    }
    public void Update(Action Exit, double Seconds)
    {
      if (isDying)
      {
        dyingTimeElapsed += (float)Seconds;
        if (dyingTimeElapsed >= dyingTimePerFrame)
        {
          dyingTimeElapsed -= dyingTimePerFrame;
          dyingCurrentFrame++;

          if (dyingCurrentFrame >= dyingTotalFrames)
          {
            isDying = false;
            dyingCurrentFrame = 0;
            Exit();
          }
        }
      }
      else if (isAttacking)
      {
        attackTimeElapsed += (float)Seconds;
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
        walkTimeElapsed += (float)Seconds;
        if (walkTimeElapsed >= walkTimePerFrame)
        {
          walkTimeElapsed -= walkTimePerFrame;
          walkCurrentFrame = (walkCurrentFrame + 1) % walkTotalFrames;
        }
      }
      else
      {
        idleTimeElapsed += (float)Seconds;
        if (idleTimeElapsed >= idleTimePerFrame)
        {
          idleTimeElapsed -= idleTimePerFrame;
          idleCurrentFrame = (idleCurrentFrame + 1) % idleTotalFrames;
        }
      }

      var kstate = Keyboard.GetState();
      if ((kstate.IsKeyDown(Keys.Enter) || Mouse.GetState().LeftButton == ButtonState.Pressed) && !isAttacking)
      {
        isAttacking = true;
        attackCurrentFrame = 0;
        attackTimeElapsed = 0f;
      }

      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        using (StreamWriter writer = new StreamWriter(positionFilePath))
        {
          writer.WriteLine(knightPosition.X);
          writer.WriteLine(knightPosition.Y);
        }
        // Exit();
        isDying = true;
      }

      float updatedKnightSpeed = knightSpeed * (float)Seconds;
      if (isAttacking) updatedKnightSpeed /= 4f;


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
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
      if (isDying)
      {
        sourceRectangle = new Rectangle(dyingCurrentFrame * dyingFrameWidth, 0, dyingFrameWidth, dyingFrameHeight);
        spriteSheetToUse = knightDyingSpriteSheet;
        spriteDecal = dyingFrameWidth / 2;
      }
      else if (isAttacking)
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

      spriteEffects = isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

      _spriteBatch.Draw(spriteSheetToUse, knightPosition, sourceRectangle, Color.White, 0f, new Vector2(spriteDecal, idleFrameHeight / 2), scale, spriteEffects, 0f);
    }
  }
}