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
    public enum ButtonStatus
    {
        Normal, // button's regular look, no interaction
        MouseOver, // when a mouse is on the button
        Pressed, // when the button is clicked
    }
    public class Button : UiObject
    {
        private Texture2D _texture;
        private bool _visible = true;
        private Vector2 _position;
        // stores the last mouse state
        private MouseState _previousState;

        // different textures
        private Texture2D _hoverTexture;
        private Texture2D _pressedTexture;

        // rectangle that covers the button
        private Rectangle _bounds;

        // current button state
        private ButtonStatus _state = ButtonStatus.Normal;

        private float _scale = 1.0f;

        public ButtonStatus GetButtonStatus
        {
            get { return _state; }
        }

        // event upon being pressed
        public event EventHandler Clicked;

        // event upon being held down
        public event EventHandler OnPress;

        // button constructor
        public Button(Texture2D texture, Texture2D hoverTexture, Texture2D pressedTexture, Vector2 position, float scale = 1.0f)
            : base ()
        {
            this._texture = texture;
            this._hoverTexture = hoverTexture;
            this._pressedTexture = pressedTexture;
            this._position = position;

            // draw bounds around the button
            this._bounds = new Rectangle((int)position.X - texture.Width/2, (int)position.Y-texture.Height/2, texture.Width, texture.Height);
            this._scale = scale;
        }

        public override void ToggleDebugMode()
        {
            //base.toggleDebugMode();
        }

        public override void ChangeFont(SpriteFont f)
        {

        }

        private void DebugUpdate(GameTime gt)
        {

        }
        private void DebugDraw(SpriteBatch sb)
        {

        }

        // update button state/fire events as necessary
        public override void Update(GameTime gameTime)
        {
            if (_visible)
            {
                // tracks mouse position
                MouseState mouseState = Mouse.GetState();

                int mouseX = mouseState.X; // sets mouse x position
                int mouseY = mouseState.Y; // sets mouse y position

                bool isMouseOver = _bounds.Contains(mouseX, mouseY); // check if the mouse is touching the button

                if (isMouseOver)
                {
                    // update the button state
                    if (_state != ButtonStatus.Pressed)
                    {
                        _state = ButtonStatus.MouseOver; // button uses the mouseover state
                    }                    

                    // check if player begins to hold the button
                    if (mouseState.LeftButton == ButtonState.Pressed && _previousState.LeftButton == ButtonState.Released)
                    {
                        // update the button state
                        _state = ButtonStatus.Pressed;

                        if (OnPress != null)
                        {
                            // player has begun holding the button down, fire press event
                            OnPress(this, EventArgs.Empty);
                        }
                    }

                    // check if the player releases the click on the button
                    else if (mouseState.LeftButton == ButtonState.Released && _previousState.LeftButton == ButtonState.Pressed)
                    {
                        // update the button state
                        _state = ButtonStatus.MouseOver;

                        if (Clicked != null)
                        {
                            // layer has stopped holding down the button, fire click event
                            Clicked(this, EventArgs.Empty);
                        }

                        // if the button has been clicked
                        else if (_state == ButtonStatus.Pressed)
                        {
                            _state = ButtonStatus.Normal;
                        }
                    }
                }
                // mouse is not on the button
                else // !isMouseOver
                {
                    _state = ButtonStatus.Normal;
                }

                _previousState = mouseState;
            }
        } // end update method
        public override void Draw(SpriteBatch spriteBatch)
        {   
            if (_visible)
            {
                // draw the button using a switch on the status of the button
                switch (_state)
                {
                    // draw the normal state of the button
                    case ButtonStatus.Normal:
                        spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(_texture.Width / 2, _texture.Height / 2), _scale, SpriteEffects.None, 0);
                        //spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), Color.White);
                        break;
                    // draw the mouseover state of the button
                    case ButtonStatus.MouseOver:
                        spriteBatch.Draw(_hoverTexture, _position, null, Color.White, 0, new Vector2(_texture.Width / 2, _texture.Height / 2), _scale, SpriteEffects.None, 0);
                        break;
                    // draw the pressed state of the button
                    case ButtonStatus.Pressed:
                        spriteBatch.Draw(_hoverTexture, _position, null, Color.White, 0, new Vector2(_texture.Width/2, _texture.Height/2), _scale + .2f, SpriteEffects.None, 0);
                        break;
                    // impossible case
                    default:
                        break;
                }
            }
        }

        public override void ChangeContext(int id)
        {

        }

        public void ChangeTexture(Texture2D texture, Texture2D hoverTexture, Texture2D pressedTexture)
        {
            this._texture = texture;
            this._hoverTexture = hoverTexture;
            this._pressedTexture = pressedTexture;
        }
        public void ToggleVisibility()
        {
            if (_visible)
            {
                _visible = false;
            }
            else
            {
                _visible = true;
            }
        }
    }
}
