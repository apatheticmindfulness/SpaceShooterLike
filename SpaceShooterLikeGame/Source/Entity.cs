using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterLikeGame.Source
{
    public abstract class Entity
    {
        protected Texture2D texture;
        protected Rectangle rectangle;
        protected Vector2 position;

        public Vector2 GetPosition() { return position; }
        public virtual void Update(float dt  = 0) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
