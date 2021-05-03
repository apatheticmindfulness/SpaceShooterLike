using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterLikeGame.Source
{
    // Rizky Maulana
    // :)
    // 
    public class Rect
    {
        public float width { set; get; }
        public float height { set; get; }
        public float left { get; set; }
        public float right { get; set; }
        public float top { get; set; }
        public float bottom { get; set; }

        public Vector2 position { set; get; }
        public Vector2 origin { set; get; }
        private Texture2D texture { set; get; }

        public Rect() { }
        public Rect(GraphicsDevice graphicsDevice, int new_width, int new_height, Vector2 new_origin, Color new_color)
        {
            width = new_width;
            height = new_height;
            origin = new_origin;

            //left = position.X - width / 2.0f;
            //right = position.X + width / 2.0f;

            Color[] color = new Color[new_width * new_height];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = new_color;
            }

            texture = new Texture2D(graphicsDevice, new_width, new_height);
            texture.SetData<Color>(color);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                position,
                new Rectangle((int)width, (int)height, (int)width, (int)height),
                Color.White,
                0.0f,
                origin,
                1.0f,
                SpriteEffects.None,
                0.0f);
        }

        public void ConfigureSide(Vector2 new_position)
        {
            left = new_position.X - width / 2.0f;
            right = new_position.X + width / 2.0f;
            bottom = new_position.Y + height / 2.0f;
            top = new_position.Y - height / 2.0f;
        }

        public void BindPosition(Vector2 position_bind)
        {
            position = position_bind;
        }

        public bool CollideWith(Rect other)
        {
            return right > other.left && left < other.right &&
                top < other.bottom && bottom > other.top;
        }
    }
}
