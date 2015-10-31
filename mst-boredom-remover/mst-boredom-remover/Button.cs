using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private string text = "";
        private Vector2 textPosition;
        private Texture2D texture;
        private bool visible = true;
        private Vector2 position;
        private Vector2 forceBounds;
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

        public Button(Texture2D texture, Texture2D hoverTexture, Texture2D pressedTexture, Vector2 position, float scale, string text, SpriteFont f, Vector2 forceBounds = default(Vector2))
        {
            this.texture = texture;
            this.hoverTexture = hoverTexture;
            this.pressedTexture = pressedTexture;
            this.position = position;

            if (forceBounds == default(Vector2))
            {
                forceBounds = new Vector2(-1, -1);
                this.bounds = new Rectangle((int)position.X - texture.Width / 2, (int)position.Y - texture.Height / 2, texture.Width, texture.Height);
            }
            else
            {
                this.forceBounds = forceBounds;
                this.bounds = new Rectangle((int)position.X - texture.Width / 2, (int)position.Y - texture.Height / 2, (int)forceBounds.X, (int)forceBounds.Y);
            }

            // draw bounds around the button
            
            this.scale = scale;
            this.text = text;
            this.font = f;

            CalculateCenter();
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

        private void CalculateCenter()
        {
            // if string is not empty
            if (text != "")
            {
                // centers the string inside the texture
                // top left corner of background + half the texture pixel size - half the pixel size of the string
                textPosition = new Vector2((bounds.X + (bounds.Width) / 2) - font.MeasureString(text).X / 2,
                    (bounds.Y + (bounds.Height) / 2) - font.MeasureString(text).Y / 2);
            }
        }

        // update button state/fire events as necessary
        public override void Update(GameTime gameTime)
        {
            if (visible)
            {
                // tracks mouse position
                MouseState mouseState = Mouse.GetState();

                int mouseX = mouseState.X; // sets mouse x position
                int mouseY = mouseState.Y; // sets mouse y position

                bool isMouseOver = bounds.Contains(mouseX, mouseY); // check if the mouse is touching the button

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
                        spriteBatch.Draw(texture, bounds, Color.White);
                        //spriteBatch.Draw(texture, new Vector2(bounds.X, bounds.Y), null, Color.White, 0, new Vector2(bounds.Width, bounds.Height), scale, SpriteEffects.None, 0);
                        break;
                    // draw the mouseover state of the button
                    case ButtonStatus.MouseOver:
                        spriteBatch.Draw(hoverTexture, bounds, Color.White);
                        //spriteBatch.Draw(hoverTexture, position, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0);
                        break;
                    // draw the pressed state of the button
                    case ButtonStatus.Pressed:
                        spriteBatch.Draw(pressedTexture, bounds, Color.White);
                        //spriteBatch.Draw(pressedTexture, position, null, Color.White, 0, new Vector2(texture.Width/2, texture.Height/2), scale + .2f, SpriteEffects.None, 0);
                        break;
                    // impossible case
                    default:
                        break;
                }
                if (state == ButtonStatus.Pressed)
                {
                    spriteBatch.DrawString(font, text, textPosition, Color.Red, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.DrawString(font, text, textPosition, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }
                
                //spriteBatch.DrawString(font, text, textPosition, Color.White);
            }
        }

        public override void ChangeContext(int id)
        {

        }

        public void ChangeTexture(Texture2D texture, Texture2D hoverTexture, Texture2D pressedTexture)
        {
            this.texture = texture;
            this.hoverTexture = hoverTexture;
            this.pressedTexture = pressedTexture;
        }
        public void changeText(string newText)
        {
            text = newText;
            CalculateCenter();
        }
        public void ToggleVisibility()
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
