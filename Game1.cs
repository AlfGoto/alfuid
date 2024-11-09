using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace alfuid;

public class Game1 : Game
{
    Texture2D ballTexture;
    Vector2 ballPosition;
    float ballSpeed;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
        ballSpeed = 300f;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        ballTexture = Content.Load<Texture2D>("ball");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        // The time since Update was called last.
        float updatedBallSpeed = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        var kstate = Keyboard.GetState();

        if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.Z))
        {
            ballPosition.Y -= updatedBallSpeed;
        }

        if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S))
        {
            ballPosition.Y += updatedBallSpeed;
        }

        if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.Q))
        {
            ballPosition.X -= updatedBallSpeed;
        }

        if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
        {
            ballPosition.X += updatedBallSpeed;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        // _spriteBatch.Draw(ballTexture, ballPosition, Color.White);
        _spriteBatch.Draw(
            ballTexture,
            ballPosition,
            null,
            Color.White,
            0f,
            new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
        _spriteBatch.End();

        base.Draw(gameTime);

        base.Draw(gameTime);
    }
}
