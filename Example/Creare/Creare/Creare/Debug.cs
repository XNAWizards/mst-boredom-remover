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

namespace Creare
{
    public class Debug
    {
        Color color = Color.Crimson;

        string difference = "";
        string smallDifference = "";
        string areaAtZero = "";
        string colliding = "";
        string playerBounds = "";
        string mousePos = "";
        string selectedTile = "";
        int X;
        int Y;
        string inventory = "";
        string invSlot = "";

        public Debug()
        {
            
        }
        public void Update(GameTime gameTime, Area area, Player player)
        {
            MouseState mouse = Mouse.GetState();
            difference = "Difference: (" + area.difference.X.ToString() + ", " + area.difference.Y.ToString() + ")";
            smallDifference = "Small Difference: (" + area.smallDifference.X.ToString() + ", " + area.smallDifference.Y.ToString() + ")";
            areaAtZero = "Area(x,y) at (0,0): " + area.area[0, 0, 0].ToString();
            if (area.colliding)
            {
                colliding = "Colliding with Object: True";
            }
            else
            {
                colliding = "Colliding with Object: False";
            }
            //playerBounds = "Player Bounds:\nX: " + player.bounds.X + "\nY: " + player.bounds.Y + "\nWidth/Height: " + player.bounds.Width;
            mousePos = "Mouse Position = " + mouse.X + ", " + mouse.Y;
            
            X = ((mouse.X + 86) / 16) - 1;
            Y = ((mouse.Y + 86) / 16) - 1;

            selectedTile = "" + X + ", " + Y;

            inventory = "";
            for (int x = 0; x < 3; x++)
            {
                inventory += player.Inv[x, 0] + ", " + player.Inv[x,1];
                inventory += "\n";
            }
            invSlot = "Slot: " + player.selectedInvSlot;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Global.font, difference, new Vector2(15, 15), color);
            spriteBatch.DrawString(Global.font, smallDifference, new Vector2(15, 30), color);
            spriteBatch.DrawString(Global.font, areaAtZero, new Vector2(15, 45), color);
            spriteBatch.DrawString(Global.font, colliding, new Vector2(15, 60), color);
            //spriteBatch.DrawString(Global.font, playerBounds, new Vector2(15, 75), color);
            spriteBatch.DrawString(Global.font, mousePos, new Vector2(15, 75), color);
            spriteBatch.DrawString(Global.font, selectedTile, new Vector2(15, 90), color);
            spriteBatch.DrawString(Global.font, inventory, new Vector2(15, 105), color);
            spriteBatch.DrawString(Global.font, invSlot, new Vector2(15, 150), color);
        }
        
    }
}
