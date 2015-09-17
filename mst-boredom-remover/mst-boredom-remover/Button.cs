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
    public class Button : UIObject
    {
        private Texture2D texture;
        private bool visible = true;
        private Vector2 position;
        // stores the last mouse state
        private MouseState previousState;

        // different textures
        private Texture2D hoverTexture;
        private Texture2D pressedTexture;

        // rectangle that covers the button
        private Rectangle bounds;

        // current button state
        private ButtonStatus state = ButtonStatus.Normal;

        private float scale = 1.0f;

        public ButtonStatus GetButtonStatus
        {
            get { return state; }
        }

        // event upon being pressed
        public event EventHandler Clicked;

        // event upon being held down
        public event EventHandler OnPress;

        // button constructor
        public Button(Texture2D texture, Texture2D hoverTexture, Texture2D pressedTexture, Vector2 position, float scale = 1.0f)
            : base ()
        {
            this.texture = texture;
            this.hoverTexture = hoverTexture;
            this.pressedTexture = pressedTexture;
            this.position = position;

            // draw bounds around the button
            this.bounds = new Rectangle((int)position.X - texture.Width/2, (int)position.Y-texture.Height/2, texture.Width, texture.Height);
            this.scale = scale;
        }

        public override void toggleDebugMode()
        {
            base.toggleDebugMode();
        }

        public override void mapMove(int deltaX, int deltaY)
        {

            //base.mapMove(deltaX, deltaY);
        }

        public override void changeFont(SpriteFont f)
        {
            base.changeFont(f);
        }

        private void debugUpdate(GameTime gt)
        {

        }
        private void debugDraw(SpriteBatch sb)
        {

        }

        // update button state/fire events as necessary
        public override void Update(GameTime gameTime)
        {
            if (visible)
            {
                // tracks mouse position
                MouseState mouseState = Mouse.GetState();

                int MouseX = mouseState.X; // sets mouse x position
                int MouseY = mouseState.Y; // sets mouse y position

                bool isMouseOver = bounds.Contains(MouseX, MouseY); // check if the mouse is touching the button

                if (isMouseOver)
                {
                    // update the button state
                    if (state != ButtonStatus.Pressed)
                    {
                        state = ButtonStatus.MouseOver; // button uses the mouseover state
                    }                    

                    // check if player begins to hold the button
                    if (mouseState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released)
                    {
                        // update the button state
                        state = ButtonStatus.Pressed;

                        if (OnPress != null)
                        {
                            // player has begun holding the button down, fire press event
                            OnPress(this, EventArgs.Empty);
                        }
                    }

                    // check if the player releases the click on the button
                    else if (mouseState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                    {
                        // update the button state
                        state = ButtonStatus.MouseOver;

                        if (Clicked != null)
                        {
                            // layer has stopped holding down the button, fire click event
                            Clicked(this, EventArgs.Empty);
                        }

                        // if the button has been clicked
                        else if (state == ButtonStatus.Pressed)
                        {
                            state = ButtonStatus.Normal;
                        }
                    }
                }
                // mouse is not on the button
                else // !isMouseOver
                {
                    state = ButtonStatus.Normal;
                }

                previousState = mouseState;
            }
        } // end update method
        public override void Draw(SpriteBatch spriteBatch)
        {   
            if (visible)
            {
                // draw the button using a switch on the status of the button
                switch (state)
                {
                    // draw the normal state of the button
                    case ButtonStatus.Normal:
                        spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0);
                        //spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), Color.White);
                        break;
                    // draw the mouseover state of the button
                    case ButtonStatus.MouseOver:
                        spriteBatch.Draw(hoverTexture, position, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0);
                        break;
                    // draw the pressed state of the button
                    case ButtonStatus.Pressed:
                        spriteBatch.Draw(hoverTexture, position, null, Color.White, 0, new Vector2(texture.Width/2, texture.Height/2), scale + .2f, SpriteEffects.None, 0);
                        break;
                    // impossible case
                    default:
                        break;
                }
            }
        }

        public override void changeContext(int id)
        {

        }

        public void ChangeTexture(Texture2D texture, Texture2D hoverTexture, Texture2D pressedTexture)
        {
            this.texture = texture;
            this.hoverTexture = hoverTexture;
            this.pressedTexture = pressedTexture;
        }
        public void toggleVisibility()
        {
            if (visible)
            {
                visible = false;
            }
            else
            {
                visible = true;
            }
        }
    }
}
