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
    class TextInput : UiObject
    {
        private Texture2D _texture;
        private Texture2D _activeTexture;
        private Vector2 _position;
        private SpriteFont _font;

        private string _currentText = "";
        private bool _active = true;

        // stores the last mouse state
        private MouseState _previousState;
        private KeyboardState _previousKeys;
        private Rectangle _bounds;

        //public event EventHandler newInput;

        public TextInput(Texture2D texture, Texture2D activeTexture, Vector2 position, SpriteFont font)
            : base()
        {
            this._texture = texture;
            this._activeTexture = activeTexture;
            this._position = position;
            this._font = font;

            _bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        

        public override void ToggleDebugMode()
        {
            //base.toggleDebugMode();
        }

        public override void ChangeContext(int id)
        {

            //base.changeContext(id);
        }

        public override void ChangeFont(SpriteFont f)
        {
            base.ChangeFont(f);
        }

        private void DebugUpdate(GameTime gt)
        {

        }
        private void DebugDraw(SpriteBatch sb)
        {

        }

        public override void Update(GameTime gt)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keys = Keyboard.GetState();

            bool isMouseOver = _bounds.Contains(mouse.X, mouse.Y); // check if the mouse is touching the button
            // first determine whether the text field needs to be activated or deactivated
            if (isMouseOver)
            {
                // if clicked on the field
                if (_previousState.LeftButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Released)
                {
                    // activate the field
                    _active = true;
                }
            }
            else
            {
                // if clicked anywhere but the field
                if (_previousState.LeftButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Released)
                {
                    // deactivate the field
                    _active = false;
                }
            }

            // only update the string if the field is active
            if (_active)
            {
                // key input from user
                char c = ' ';
                foreach (Keys k in _previousKeys.GetPressedKeys())
                {
                    if (keys.IsKeyUp(k))
                    {
                        if (k == Keys.Back)
                        {
                            c = '<';
                        }
                        else
                        {
                            try
                            {
                                c = Convert.ToChar(k);
                            }
                            catch (Exception e)
                            {
                                // k is not a key convertable to a char
                            }
                        }
                    }
                }

                // if key was backspace
                if (c == '<')
                {
                    if (_currentText.Length > 0)
                    {
                        _currentText = _currentText.Remove(_currentText.Length - 1);
                    }
                }
                else
                {
                    // verify string length
                    if (_font.MeasureString((_currentText + "W")).X < _texture.Width - 10)
                    {
                        // verify string contents
                        if (c != ' ' && (c > 65 && c <= 90) || (c > 96 && c <= 122))
                        {
                            _currentText += c;
                        }
                    }
                }
            }
            
            _previousState = mouse;
            _previousKeys = keys;
            //base.Update(gt);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!_active)
            {
                sb.Draw(_texture, _position, Color.White);
            }
            else
            {
                sb.Draw(_activeTexture, _position, Color.White);
            }
            sb.DrawString(_font, _currentText, _position + new Vector2(10, 12), Color.Black);
            //base.Draw(sb);
        }
    }
}
