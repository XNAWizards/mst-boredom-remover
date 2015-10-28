using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mst_boredom_remover
{
    public class Menu : UiObject
    {
        private Texture2D texture;
        private Vector2 position;
        private List<UiObject> controls;
        private Color color;
        private int thisId;

        private bool active = false;

        public Menu(Texture2D texture, Vector2 position, List<UiObject> controls, Color color, int thisId)
            : base()
        {
            this.texture = texture;
            this.position = position;
            this.controls = controls;
            this.color = color;
            this.thisId = thisId;

            foreach (UiObject u in controls)
            {
                u.parentReference = this;
            }
        }

        public void Activate()
        {
            active = true;
        }

        public void Deactivate()
        {
            active = false;
            // reset?
        }

        public override void ChangeContext(int id)
        {
            if (id != thisId)
            {
                Deactivate();
            }
            else if (id == thisId)
            {
                Activate();
            }
        }

        public override void ToggleDebugMode()
        {
            foreach (UiObject u in controls)
            {
                u.ToggleDebugMode();
            }

            debugMode = !debugMode;
            //base.toggleDebugMode();
        }

        public override void ChangeFont(SpriteFont f)
        {
            font = f;
        }

        private void DebugUpdate(GameTime gt)
        {
            // reset the debug text
            debugText = "";
            // compile each subobject's debug text
            foreach (UiObject x in controls)
            {
                debugText += x.debugText;
                debugText += "\n";
            }
        }
        private void DebugDraw(SpriteBatch sb)
        {
            sb.DrawString(font, debugText, Vector2.Zero, Color.White);
        }

        public override void IssueOrder(string order)
        {
            foreach (UiObject u in controls)
            {
                if (u.GetType() == typeof(Hud))
                {
                    u.IssueOrder(order);
                }
            }
        }

        public override void Update(GameTime gt)
        {
            if (active)
            {
                foreach (UiObject x in controls)
                {
                    x.Update(gt);
                }
                //base.Update(gt);

                if (debugMode)
                {
                    DebugUpdate(gt);
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (active)
            {
                
                foreach (UiObject x in controls)
                {
                    x.Draw(sb);
                }
                //base.Draw(sb);
                if (debugMode)
                {
                    DebugDraw(sb);
                }
                //sb.Draw(texture, position, color);
            }
        }
    }
}
