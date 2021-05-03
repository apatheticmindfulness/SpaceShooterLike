using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceShooterLikeGame.Source
{
    public class Meteoroid :  Entity
    {
        private float m_Width = 75.0f;
        private float m_Height = 75.0f;
        private float m_Speed = 5.0f * 60.0f;
        private bool m_IsActive = false;
        private Vector2 m_Origin;
        private Rect m_CollBox;

        private GraphicsDevice m_GraphicsDevice;
        private Random m_Random;

        public void Init(GraphicsDevice graphicsDevice, Texture2D new_texture, Vector2 new_position,  float speed,  int texture_index, ref Random random)
        {
            m_GraphicsDevice = graphicsDevice;
            texture = new_texture;
            position = new_position;
            m_Speed = speed;
            rectangle = new Rectangle(texture_index * (int)m_Width, 1, (int)m_Width, (int)m_Height);
            m_Random = random;
            m_Origin = new Vector2(m_Width / 2.0f, m_Height / 2.0f);

            // Collision box
            m_CollBox = new Rect(graphicsDevice, (int)m_Width - 30, (int)m_Height - 30, m_Origin, Color.White);
            m_CollBox.position = position + new Vector2(15.0f, 15.0f);

            m_IsActive = true;
        }


        public override void Update(float dt = 0)
        {
            m_CollBox.ConfigureSide(position);

            position += new Vector2(-1.0f, 0.0f) * m_Speed * dt;
            if(m_CollBox.left + m_CollBox.right < 0)
            {
                Reset();
            }

            m_CollBox.BindPosition(position + new Vector2(15.0f, 15.0f));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //m_CollBox.Draw(spriteBatch);
            if (m_IsActive)
            {
                spriteBatch.Draw(
                    texture,
                    position,
                    rectangle,
                    Color.White,
                    0.0f,
                    m_Origin,
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
            }
        }

        public void Reset()
        {
            Vector2 random_position = new Vector2(
               m_Random.Next(GameConfig.Window.Width + (int)m_Width, GameConfig.Window.Width + (int)m_Width * 7),
               m_Random.Next(75, GameConfig.Window.Height - 75)
            );
            float speed = (float)m_Random.Next(1, 10);

            Init(m_GraphicsDevice, texture, random_position, speed * 60.0f, 1, ref m_Random);
        }

        public Rect GetRect()
        {
            return m_CollBox;
        }

        public bool CollideWithProjectile(Rect projectile)
        {
            return m_CollBox.CollideWith(projectile);
        }
    }
}
