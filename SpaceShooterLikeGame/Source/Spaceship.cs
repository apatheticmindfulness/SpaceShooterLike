using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooterLikeGame.Source
{
    public class Spaceship : Entity
    {
        private float m_Width = 75.0f;
        private float m_Height = 75.0f;
        private float m_Speed = 5.0f * 60.0f;
        private Vector2 m_Origin;
        private Vector2 direction;

        Rect m_CollBox;
        private Vector2 m_CollisionPosition;

        public Spaceship(GraphicsDevice graphicsDevice, Texture2D new_texture, Vector2 new_position, int texture_index)
        {
            texture = new_texture;
            position = new_position;
            rectangle = new Rectangle(texture_index * (int)m_Width, 1, (int)m_Width, (int)m_Height);
            m_Origin = new Vector2(m_Width / 2.0f, m_Height / 2.0f);

            m_CollBox = new Rect(graphicsDevice, (int)m_Width, (int)m_Height - 40, m_Origin, Color.White);
            m_CollBox.position = m_CollisionPosition;
        }

        public void KeyboardInput(float dt)
        {
            CollideWithWindow();
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position += new Vector2(1.0f, 0.0f) * m_Speed * dt;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position += new Vector2(-1.0f, 0.0f) * m_Speed * dt;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                position += new Vector2(0.0f, -1.0f) * m_Speed * dt;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                position += new Vector2(0.0f, 1.0f) * m_Speed * dt;
            }
        }

        public void CollideWithWindow()
        {
            if (m_CollBox.left < 0)
            {
                position.X += 0.0f - m_CollBox.left;
            }
            else if (m_CollBox.right > GameConfig.Window.Width)
            {
                position.X -= m_CollBox.right - GameConfig.Window.Width;
            }

            if (m_CollBox.bottom > GameConfig.Window.Height)
            {
                position.Y -= m_CollBox.bottom - GameConfig.Window.Height;
            }
            else if (m_CollBox.top < 0)
            {
                position.Y += 0 - m_CollBox.top;
            }
        }

        public override void Update(float dt = 0)
        {
            m_CollBox.ConfigureSide(position);
            KeyboardInput(dt);
            m_CollBox.BindPosition(position + new Vector2(0.0f, 15.0f));

            base.Update(dt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            m_CollBox.Draw(spriteBatch);
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
}
