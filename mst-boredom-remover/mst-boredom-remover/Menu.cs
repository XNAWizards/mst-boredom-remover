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
    public class Menu : UIObject
    {
        private Texture2D texture;
        private Vector2 position;
        private List<UIObject> controls;
        private Color color;
        private int thisId;

        private bool active = false;

        public Menu(Texture2D texture, Vector2 position, List<UIObject> controls, Color color, int thisId)
            : base()
        {
            this.texture = texture;
            this.position = position;
            this.controls = controls;
            this.color = color;
            this.thisId = thisId;
        }

        public void activate()
        {
            active = true;
        }

        public void deactivate()
        {
            active = false;
            // reset?
        }

        public override void changeContext(int id)
        {
            if (id != thisId)
            {
                deactivate();
            }
            else if (id == thisId)
            {
                activate();
            }
        }

        public override void toggleDebugMode()
        {
            foreach (UIObject u in controls)
            {
                u.toggleDebugMode();
            }
            //base.toggleDebugMode();
        }

        public override void changeFont(SpriteFont f)
        {
            foreach (UIObject u in controls)
            {
                u.changeFont(f);
            }
        }

        public override void mapMove(int deltaX, int deltaY)
        {

            //base.mapMove(deltaX, deltaY);
        }

        private void debugUpdate(GameTime gt)
        {

        }
        private void debugDraw(SpriteBatch sb)
        {

        }

        public override void Update(GameTime gt)
        {
            if (active)
            {
                foreach (UIObject x in controls)
                {
                    x.Update(gt);
                }
                //base.Update(gt);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (active)
            {
                sb.Draw(texture, position, color);
                foreach (UIObject x in controls)
                {
                    x.Draw(sb);
                }
                //base.Draw(sb);
            }
        }
    }
}
