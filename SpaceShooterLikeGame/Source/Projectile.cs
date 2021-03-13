using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterLikeGame.Source
{
    public class Projectile : Entity
    {
        private float m_Width = 75.0f;
        private float m_Height = 75.0f;
        private float m_Speed = 10.0f * 60.0f;
        private bool m_IsActive = false;

        private Vector2 m_Origin;
        private Rect m_CollBox;

        public Projectile(GraphicsDevice graphicsDevice, Texture2D new_texture, Vector2 new_position, int texture_index)
        {
            texture = new_texture;
            position = new_position;
            rectangle = new Rectangle(texture_index * (int)m_Width, 1, (int)m_Width, (int)m_Height);
            m_Origin = new Vector2(m_Width / 2.0f, m_Height / 2.0f);

            m_CollBox = new Rect(graphicsDevice, (int)m_Width - 50, (int)m_Height - 60, m_Origin, Color.White);
            m_CollBox.position = position + new Vector2(27.0f, 27.0f);

            m_IsActive = true;
        }

        public override void Update(float dt = 0)
        {
            m_CollBox.ConfigureSide(position);

            position += new Vector2(1, 0) * m_Speed * dt;
            if(m_CollBox.left + m_CollBox.right > GameConfig.Window.Width + m_CollBox.left)
            {
                m_IsActive = false;
            }

            m_CollBox.BindPosition(position + new Vector2(27.0f, 27.0f));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //m_CollBox.Draw(spriteBatch);
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

        public bool IsActive() { return m_IsActive; }

        public Rect GetRect() { return m_CollBox; }

        public bool CollideWithMeteor(Rect meteor)
        {
            return m_CollBox.CollideWith(meteor);
        }

    }

}
