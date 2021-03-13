using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterLikeGame.Source
{
    public class Goal : Entity
    {
        private float m_Speed = 3.0f * 60.0f;
        private int m_Width = 25;
        private int m_Height = 25;
        private Vector2 m_Origin;
        private Rect m_CollBox;
        private bool m_IsCollected = false;
        private GraphicsDevice m_GraphicsDevice;
        private Random m_Random;

        public void Init(GraphicsDevice graphicsDevice, Vector2 new_position, Color color, ref Random random)
        {
            m_GraphicsDevice = graphicsDevice;

            position = new_position;
            m_Origin = new Vector2((float)m_Width / 2.0f, (float)m_Height / 2.0f);

            m_CollBox = new Rect(graphicsDevice, m_Width, m_Height, m_Origin, color);
            m_CollBox.position = position;

            m_Random = random;
        }

        public override void Update(float dt = 0)
        {
            m_CollBox.ConfigureSide(position);

            position += new Vector2(-1.0f, 0.0f) * m_Speed * dt;
            if(m_CollBox.right < 0)
            {
                Reset();
            }

            m_CollBox.BindPosition(position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!m_IsCollected)
            {
                m_CollBox.Draw(spriteBatch);
            }
        }

        public void Reset()
        {
            Vector2 random_position = new Vector2(
               m_Random.Next(GameConfig.Window.Width + (int)m_Width, GameConfig.Window.Width + (int)m_Width * 7),
               m_Random.Next(m_Height, GameConfig.Window.Height - m_Height * 7)
            );

            Init(m_GraphicsDevice, random_position, Color.Blue, ref m_Random);
        }

        public Rect GetRect()
        {
            return m_CollBox;
        }

        public bool IsCollected() { return m_IsCollected; }
    }
}
