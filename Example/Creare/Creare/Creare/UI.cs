using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Creare
{
    public class UI
    {
        Texture2D craftingBook;
        Texture2D inventoryBackground;
        List<Texture2D> items; // array of item textures
        List<Texture2D> itemsGray; // array of grayed item textures
        Texture2D hoverTexture;

        List<Vector2> itemCoordsInv = new List<Vector2>()
        {
            // 80x80
            new Vector2(347, 96), // 0
            new Vector2(542, 276), // 1
            new Vector2(343, 434), // 2
            new Vector2(173, 265), // 3
            // 52x52
            new Vector2(495, 20), // 4
            new Vector2(593, 61), // 5
            new Vector2(641, 161), // 6
            new Vector2(664, 265), // 7
            new Vector2(654, 372), // 8
            new Vector2(602, 457), // 9
            new Vector2(524, 520), // 10
            new Vector2(200, 519), // 11
            new Vector2(123, 447), // 12
            new Vector2(69, 354), // 13
            new Vector2(48, 249), // 14
            new Vector2(73, 146), // 15
            new Vector2(140, 63), // 16
            new Vector2(230, 16), // 17
        };

        List<Texture2D> equipment = new List<Texture2D>();
        
        Texture2D[] healthPotion;
        int healthBarIndex = 0; // status of the player's HP

        Vector2 inventoryPos = new Vector2(20, 0); // position of the inventory button
        Vector2 healthBarPos = new Vector2(700, 15); // position of the HP thing

        public UI(Texture2D[] healthPotion, Texture2D inventoryBackground, List<Texture2D> equipment)
        {
            this.healthPotion = healthPotion;
            this.inventoryBackground = inventoryBackground;
            this.equipment = equipment;
        }
        public void Update(GameTime gameTime, Player player)
        {
            
        }
        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.Draw(healthPotion[healthBarIndex], healthBarPos, Color.White);
            if (Global.displayInv)
            {
                Global.displayBook = false;
                spriteBatch.Draw(inventoryBackground, inventoryPos, Color.White);
                for (int x = 0; x < 18; x++)
                {
                    if (player.Inv[x, 0] != 0)
                    {
                        if (player.Inv[x, 0] < 100)
                        {
                            if (x < 4)
                            {
                                spriteBatch.Draw(items[player.Inv[x, 0]], new Rectangle((int)itemCoordsInv[x].X + 20, (int)itemCoordsInv[x].Y, 80, 80), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(items[player.Inv[x, 0]], new Rectangle((int)itemCoordsInv[x].X + 20, (int)itemCoordsInv[x].Y, 52, 52), Color.White);
                            }
                            spriteBatch.DrawString(Global.font, player.Inv[x, 1].ToString(), new Vector2(itemCoordsInv[x].X + 25, itemCoordsInv[x].Y + 5), Color.White);
                        }
                        else
                        {
                            if (x < 4)
                            {
                                spriteBatch.Draw(equipment[player.Inv[x, 0] - 100], new Rectangle((int)itemCoordsInv[x].X + 20, (int)itemCoordsInv[x].Y, 80, 80), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(equipment[player.Inv[x, 0] - 100], new Rectangle((int)itemCoordsInv[x].X + 20, (int)itemCoordsInv[x].Y, 52, 52), Color.White);
                            }
                            spriteBatch.DrawString(Global.font, player.Inv[x, 1].ToString(), new Vector2(itemCoordsInv[x].X + 25, itemCoordsInv[x].Y + 5), Color.White);
                        }
                    }
                }
                //spriteBatch.Draw(items[hoverTexture], new Vector2(Global.GetMouse().X, Global.GetMouse().Y),
                if (Global.hover && hoverTexture != null)
                {
                    Rectangle mouse = new Rectangle(Global.GetMouse().X, Global.GetMouse().Y, 42, 42);
                    spriteBatch.Draw(hoverTexture, mouse, Color.White);
                }
            }
            
        }
        public void AddItemTextures(List<Texture2D> tempItems)
        {
            this.items = tempItems;
        }
        public void AddItemGrayTextures(List<Texture2D> tempItems)
        {
            this.itemsGray = tempItems;
        }
        public int GetSlot(MouseState mouse)
        {
            Rectangle rectangle;

            for (int x = 0; x < 18; x++)
            {
                if (x < 4)
                {
                    rectangle = new Rectangle((int)itemCoordsInv[x].X, (int)itemCoordsInv[x].Y, 80, 80);
                }
                else
                {
                    rectangle = new Rectangle((int)itemCoordsInv[x].X, (int)itemCoordsInv[x].Y, 52, 52);
                }
                if (rectangle.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)))
                {
                    return x;
                }
            }

            return -1;
        }
        public void SetHover(int index)
        {
            if (index >= 100)
            {
                hoverTexture = equipment[index - 100];
            }
            else if (index != 0)
            {
                hoverTexture = items[index];
            }
            else
            {
                hoverTexture = null;
            }
        }
        public void RecipeAdd()
        {

        }
    }
}
