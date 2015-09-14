using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace mst_boredom_remover
{
    public class UIObject
    {
        // Master class to contain all UI objects
        private Texture2D texture;
        private Vector2 position;

        public virtual void changeContext(int id)
        {

        }

        public UIObject(Texture2D texture, Vector2 position)
        {
            //this.texture = texture;
            //this.position = position;
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
