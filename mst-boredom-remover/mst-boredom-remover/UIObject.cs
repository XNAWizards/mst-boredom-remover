using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mst_boredom_remover
{
    public class UiObject
    {
        // Master class to contain all UI objects
        //private Texture2D texture;
        //private Vector2 position;
        protected bool debugMode = false;
        protected SpriteFont font;
        public string debugText = "";

        public virtual void ChangeFont(SpriteFont f)
        {
            //this.font = f;
        }

        public virtual void ToggleDebugMode()
        {
            //debugMode = !debugMode;
        }
        public virtual void ChangeContext(int id)
        {

        }

        public UiObject(SpriteFont font = null)
        {
            this.font = font;
        }

        public virtual void Update(GameTime gt)
        {
            // no default behavior
        }
        public virtual void Draw(SpriteBatch sb)
        {
            // no default draw
        }
    }
}
