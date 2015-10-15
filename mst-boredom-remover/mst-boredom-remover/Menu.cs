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
    public class Menu : UiObject
    {
        private Texture2D _texture;
        private Vector2 _position;
        private List<UiObject> _controls;
        private Color _color;
        private int _thisId;

        private bool _active = false;

        public Menu(Texture2D texture, Vector2 position, List<UiObject> controls, Color color, int thisId)
            : base()
        {
            this._texture = texture;
            this._position = position;
            this._controls = controls;
            this._color = color;
            this._thisId = thisId;
        }

        public void Activate()
        {
            _active = true;
        }

        public void Deactivate()
        {
            _active = false;
            // reset?
        }

        public override void ChangeContext(int id)
        {
            if (id != _thisId)
            {
                Deactivate();
            }
            else if (id == _thisId)
            {
                Activate();
            }
        }

        public override void ToggleDebugMode()
        {
            foreach (UiObject u in _controls)
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
            foreach (UiObject x in _controls)
            {
                debugText += x.debugText;
            }
        }
        private void DebugDraw(SpriteBatch sb)
        {
            sb.DrawString(font, debugText, Vector2.Zero, Color.White);
        }

        public override void Update(GameTime gt)
        {
            if (_active)
            {
                foreach (UiObject x in _controls)
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
            if (_active)
            {
                sb.Draw(_texture, _position, _color);
                foreach (UiObject x in _controls)
                {
                    x.Draw(sb);
                }
                //base.Draw(sb);
                if (debugMode)
                {
                    DebugDraw(sb);
                }
            }
        }
    }
}
