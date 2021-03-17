using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooterLikeGame.Source;
using System;

namespace SpaceShooterLikeGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameState
        {
            GAME_RUN,
            GAME_OVER,
            GAME_START,
            GAME_DO_NOTHING
        };

        private Texture2D m_Texture;
        private Spaceship m_Spaceship;
        private Meteoroid[] m_Meteoroid = new Meteoroid[20];
        private Goal m_Goal;
        private int m_Score = 0;
        private Random m_Random;

        private Texture2D m_StartScreenTexture;
        private Texture2D m_GameOverTexture;

        private GameState m_GameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameConfig.Window.Width;
            graphics.PreferredBackBufferHeight = GameConfig.Window.Height;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        private void InitializeGame()
        {
            m_Random = new Random();
            m_Goal = new Goal();
            m_Goal.Init(graphics.GraphicsDevice, new Vector2((float)GameConfig.Window.Width / 2.0f, 100.0f), Color.Blue, ref m_Random);

            for (int i = 0; i < m_Meteoroid.Length; i++)
            {
                Vector2 random_position = new Vector2(
                    m_Random.Next(GameConfig.Window.Width + 75, GameConfig.Window.Width + 75 * 7),
                    m_Random.Next(75 * 5, GameConfig.Window.Height - 75)
                );
                float speed = (float)m_Random.Next(3, 5);

                m_Meteoroid[i] = new Meteoroid();
                m_Meteoroid[i].Init(graphics.GraphicsDevice, m_Texture, random_position, speed * 60.0f, 1, ref m_Random);
            };
        }

        private void DrawStartScreen()
        {
            spriteBatch.Draw(
                    m_StartScreenTexture,
                    new Vector2(GameConfig.Window.Width / 2.0f, GameConfig.Window.Height / 2.0f),
                    new Rectangle(0, 0, m_StartScreenTexture.Width, m_StartScreenTexture.Height),
                    Color.White,
                    0.0f,
                    new Vector2(m_StartScreenTexture.Width / 2.0f, m_StartScreenTexture.Height / 2.0f),
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
        }

        private void DrawGameOverScreen()
        {
            spriteBatch.Draw(
                m_GameOverTexture,
                new Vector2(GameConfig.Window.Width / 2.0f, GameConfig.Window.Height / 2.0f),
                new Rectangle(0, 0, m_GameOverTexture.Width, m_GameOverTexture.Height),
                Color.White,
                0.0f,
                new Vector2(m_GameOverTexture.Width / 2.0f, m_GameOverTexture.Height / 2.0f),
                1.0f,
                SpriteEffects.None,
                0.0f);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            m_GameState = GameState.GAME_DO_NOTHING;

            m_Random = new Random();

            m_Goal = new Goal();
            m_Goal.Init(graphics.GraphicsDevice, new Vector2((float)GameConfig.Window.Width / 2.0f, 100.0f), Color.Blue, ref m_Random);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            m_StartScreenTexture = Content.Load<Texture2D>("Resources/Assets/Start");
            m_GameOverTexture = Content.Load<Texture2D>("Resources/Assets/GameOver");

            m_Texture = Content.Load<Texture2D>("Resources/Assets/Sprite");
            m_Spaceship = new Spaceship(graphics.GraphicsDevice, m_Texture, new Vector2((float)GameConfig.Window.Width / 2.0f, (float)GameConfig.Window.Height / 2.0f), 0);

            for (int i = 0; i < m_Meteoroid.Length; i++)
            {
                Vector2 random_position = new Vector2(
                    m_Random.Next(GameConfig.Window.Width + 75, GameConfig.Window.Width + 75 * 7),
                    m_Random.Next(75 * 5, GameConfig.Window.Height - 75)
                );
                float speed = (float)m_Random.Next(3, 5);

                m_Meteoroid[i] = new Meteoroid();
                m_Meteoroid[i].Init(graphics.GraphicsDevice, m_Texture, random_position, speed * 60.0f, 1, ref m_Random);
            };

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(m_GameState == GameState.GAME_DO_NOTHING || m_GameState == GameState.GAME_OVER)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    m_GameState = GameState.GAME_RUN;
                    InitializeGame();
                }
            }

            if (m_GameState == GameState.GAME_RUN)
            {
                m_Spaceship.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                // Meteoroids
                for (int i = 0; i < m_Meteoroid.Length; i++)
                {
                    m_Meteoroid[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (m_Spaceship.CollideWithMeteor(m_Meteoroid[i].GetRect()))
                    {
                        // Player dead
                        m_GameState = GameState.GAME_OVER;
                    }


                    if (m_Spaceship.ProjectileHitMeteor(m_Meteoroid[i]))
                    {
                        // Terserah 
                    }
                }

                // Goal
                m_Goal.Update(dt);
                if (m_Spaceship.CollideWithGoal(m_Goal))
                {
                    // Tambah poin
                    m_Goal.Reset();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(GameConfig.Window.BackgroundColor);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if(m_GameState == GameState.GAME_DO_NOTHING)
            {
                DrawStartScreen();
            }
            else
            {
                m_Spaceship.Draw(spriteBatch);
                for (int i = 0; i < m_Meteoroid.Length; i++)
                {
                    m_Meteoroid[i].Draw(spriteBatch);
                }

                m_Goal.Draw(spriteBatch);
            }

            if (m_GameState == GameState.GAME_OVER)
            {
                DrawGameOverScreen();
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
