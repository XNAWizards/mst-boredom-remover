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
    public class TextDisplay : UiObject
    {
        private Texture2D _texture;      // background texture
        private Vector2 _position;       // top left corner of the background
        private Color _color;            // color of the text

        private string _text = "";       // the text
        private Vector2 _textPosition;   // calculated position of the text

        private int _test = 1;

        public TextDisplay(Texture2D texture, Vector2 position, SpriteFont font, Color color)
            : base()
        {
            this._texture = texture;
            this._position = position;
            this.font = font;
            this._color = color;
            CalculateCenter();
        }

        // Calculates where to draw the text
        private void CalculateCenter()
        {
            // if string is not empty
            if (_text != "")
            {
                // centers the string inside the texture
                // top left corner of background + half the texture pixel size - half the pixel size of the string
                _textPosition = new Vector2((_position.X + _texture.Width / 2) - font.MeasureString(_text).X / 2, 
                    (_position.Y + _texture.Height / 2) - font.MeasureString(_text).Y / 2);
            }
        }

        // Changes the text to be displayed
        // Recenters the text
        public void ChangeText(string newText)
        {
            _text = newText;
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
            _test++;
            ChangeText(_test.ToString());
            //base.Update(gt);
        }

        // Draws the background and text
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_texture, _position, Color.White);
            sb.DrawString(font, _text, _textPosition, _color);
            //base.Draw(sb);
        }
    }
}
