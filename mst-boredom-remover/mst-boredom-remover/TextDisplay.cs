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
    public class TextDisplay : UIObject
    {
        private Texture2D texture;      // background texture
        private Vector2 position;       // top left corner of the background
        private SpriteFont font;        // font of the text
        private Rectangle bounds;       // rectangle bounds containing the background
        private Color color;            // color of the text

        private string text = "";       // the text
        private Vector2 textPosition;   // calculated position of the text

        private int test = 1;

        public TextDisplay(Texture2D texture, Vector2 position, SpriteFont font, Color color)
            : base(texture, position)
        {
            this.texture = texture;
            this.position = position;
            this.font = font;
            this.color = color;
            calculateCenter();
        }

        // Calculates where to draw the text
        private void calculateCenter()
        {
            // if string is not empty
            if (text != "")
            {
                // centers the string inside the texture
                // top left corner of background + half the texture pixel size - half the pixel size of the string
                textPosition = new Vector2((position.X + texture.Width / 2) - font.MeasureString(text).X / 2, 
                    (position.Y + texture.Height / 2) - font.MeasureString(text).Y / 2);
            }
        }

        // Changes the text to be displayed
        // Recenters the text
        public void changeText(string newText)
        {
            text = newText;
            calculateCenter();
        }

        public override void changeContext(int id)
        {

        }

        // ?
        public override void Update(GameTime gt)
        {
            test++;
            changeText(test.ToString());
            //base.Update(gt);
        }

        // Draws the background and text
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
            sb.DrawString(font, text, textPosition, color);
            //base.Draw(sb);
        }
    }
}
