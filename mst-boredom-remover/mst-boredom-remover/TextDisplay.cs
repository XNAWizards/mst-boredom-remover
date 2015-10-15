using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mst_boredom_remover
{
    public class TextDisplay : UiObject
    {
        private Texture2D texture;      // background texture
        private Vector2 position;       // top left corner of the background
        private Color color;            // color of the text

        private string text = "";       // the text
        private Vector2 textPosition;   // calculated position of the text

        private int test = 1;

        public TextDisplay(Texture2D texture, Vector2 position, SpriteFont font, Color color)
        {
            this.texture = texture;
            this.position = position;
            this.font = font;
            this.color = color;
            CalculateCenter();
        }

        // Calculates where to draw the text
        private void CalculateCenter()
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
        public void ChangeText(string newText)
        {
            text = newText;
            CalculateCenter();
        }

        public override void ChangeFont(SpriteFont f)
        {
            //font = f;
        }

        public override void ChangeContext(int id)
        {

        }

        public override void ToggleDebugMode()
        {
            //base.toggleDebugMode();
        }

        private void DebugUpdate(GameTime gt)
        {

        }
        private void DebugDraw(SpriteBatch sb)
        {

        }

        // ?
        public override void Update(GameTime gt)
        {
            test++;
            ChangeText(test.ToString());
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
