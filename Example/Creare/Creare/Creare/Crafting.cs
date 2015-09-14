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
using System.IO;

namespace Creare
{
    class Crafting
    {
        Texture2D craftingBook;
        public int page = 0;

        Recipe[,] recipes = new Recipe[10, 2];
        List<Texture2D> items; // array of item textures
        List<Texture2D> itemsGray; // array of grayed item textures
        List<Texture2D> equipment;

        bool page1 = false;
        bool page2 = false;

        List<Vector2> itemCoordsBook = new List<Vector2>() // coordinates for the book slots
        {
            new Vector2(40,206), // 0
            new Vector2(40,277), // 1
            new Vector2(40,348), // 2
            new Vector2(40,419), // 3
            new Vector2(199,206), // 4
            new Vector2(199,277), // 5
            new Vector2(199,348), // 6
            new Vector2(199,419), // 7
            new Vector2(431,206), // 8
            new Vector2(431,277), // 9
            new Vector2(481,348), // 10
            new Vector2(481,419), // 11
            new Vector2(626,206), // 12
            new Vector2(626,277), // 13
            new Vector2(626,348), // 14
            new Vector2(626,419), // 15
            new Vector2(238, 55), // completed slot 16
            new Vector2(653, 55), // completed slot 17
        };

        Rectangle completedSlot;
        Rectangle completedSlot2;
        Rectangle notCompletedSlot;
        Rectangle notCompletedSlot2;

        public Crafting(Texture2D craftingBook, List<Texture2D> items, List<Texture2D> itemsGray, List<Texture2D> equipment)
        {
            completedSlot = new Rectangle((int)itemCoordsBook[16].X, (int)itemCoordsBook[16].Y, 100, 100);
            completedSlot2 = new Rectangle((int)itemCoordsBook[17].X, (int)itemCoordsBook[17].Y, 100, 100);
            notCompletedSlot = new Rectangle((int)itemCoordsBook[16].X, (int)itemCoordsBook[16].Y, 50, 50);
            notCompletedSlot2 = new Rectangle((int)itemCoordsBook[17].X, (int)itemCoordsBook[17].Y, 50, 50);

            this.craftingBook = craftingBook;
            this.items = items;
            this.itemsGray = itemsGray;
            this.equipment = equipment;

            int[,] copperPick = new int[2, 2]
            {
                {11, 3},
                {7, 3},
            };
            int[,] copperAxe = new int[2, 2]
            {
                {11, 2},
                {7, 4},
            };
            int[,] ironPick = new int[2, 2]
            {
                {11, 2},
                {8, 3},
            };
            int[,] ironAxe = new int[2, 2]
            {
                {11, 2},
                {8, 4},
            };
            int[,] steel = new int[2, 2]
            {
                {7, 2},
                {8, 1},
            };
            int[,] steelPick = new int[2, 2]
            {
                {11, 2},
                {12, 3},
            };
            int[,] steelAxe = new int[2, 2]
            {
                {11, 2},
                {12, 4},
            };
            int[,] silverPick = new int[2, 2]
            {
                {11, 2},
                {9, 3},
            };
            int[,] silverAxe = new int[2, 2]
            {
                {11, 2},
                {9, 4},
            };
            int[,] goldPick = new int[2, 2]
            {
                {11, 2},
                {10, 3},
            };
            int[,] goldAxe = new int[2, 2]
            {
                {11, 2},
                {10, 4},
            };

            recipes[0, 0] = new Recipe("Copper Pickaxe", copperPick, 0);
            recipes[0, 1] = new Recipe("Copper Axe", copperAxe, 1);
            recipes[1, 0] = new Recipe("Iron Pickaxe", ironPick, 2);
            recipes[1, 1] = new Recipe("Iron Axe", ironAxe, 3);
            recipes[2, 0] = new Recipe("Steel Ingot", steel, 12);
            recipes[2, 1] = new Recipe("Steel Pickaxe", steelPick, 4);
            recipes[3, 0] = new Recipe("Steel Axe", steelAxe, 5);
            recipes[3, 1] = new Recipe("Silver Pickaxe", silverPick, 6);
            recipes[4, 0] = new Recipe("Silver Axe", silverAxe, 7);
            recipes[4, 1] = new Recipe("Gold Pickaxe", goldPick, 8);
            recipes[5, 0] = new Recipe("Gold Axe", goldAxe, 9);
        }
        public void Update(GameTime gameTime, Player player)
        {
            Global.displayInv = false;

            page1 = CheckInvSlots(recipes[page, 0].requirements, player.Inv);
            page2 = CheckInvSlots(recipes[page, 1].requirements, player.Inv);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(craftingBook, new Vector2(0, 0), Color.White);
            for (int x = 0; x < 2; x++) // page
            {
                for (int z = 0; z < 2; z++) // item quantity and index
                {
                    if (x == 1) // if second page
                    {
                        DrawRequirements2(spriteBatch, recipes[page, x].requirements);
                    }
                    else
                    {
                        DrawRequirements(spriteBatch, recipes[page, x].requirements);
                    }
                    //spriteBatch.Draw(items[recipes[page, x].itemIndex], itemCoordsBook[y], Color.White);
                }
                
            }
            if (page1)
            {
                spriteBatch.Draw(equipment[recipes[page, 0].itemIndex], completedSlot, Color.White);
            }
            else
            {
                spriteBatch.Draw(equipment[recipes[page, 0].itemIndex], completedSlot, Color.White);
            }
            if (page2)
            {
                spriteBatch.Draw(equipment[recipes[page, 1].itemIndex], completedSlot2, Color.White);
            }
            else
            {
                spriteBatch.Draw(equipment[recipes[page, 1].itemIndex], completedSlot2, Color.White);
            }
        }
        public void DrawRequirements(SpriteBatch spriteBatch, int[,] requirements)
        {
            for (int x = 0; x < 2; x++)
            {
                spriteBatch.Draw(items[requirements[x, 0]], itemCoordsBook[x], Color.White);
                spriteBatch.DrawString(Global.bigFont, requirements[x, 1].ToString(), new Vector2(itemCoordsBook[x].X + 100, itemCoordsBook[x].Y), Color.Black);
            }
            
        }
        public void DrawRequirements2(SpriteBatch spriteBatch, int[,] requirements)
        {
            for (int x = 0; x < 2; x++)
            {
                spriteBatch.Draw(items[requirements[x, 0]], new Vector2(itemCoordsBook[x + 8].X + 50, itemCoordsBook[x + 8].Y), Color.White);
                spriteBatch.DrawString(Global.bigFont, requirements[x, 1].ToString(), new Vector2(itemCoordsBook[x + 8].X + 150, itemCoordsBook[x + 8].Y), Color.Black);
            }
        }
        public bool CheckInvSlots(int[,] recipe, int[,] playerInv)
        {
            bool req1 = false;
            bool req2 = false;

            for (int x = 0; x < 18; x++)
            {
                if (playerInv[x, 0] == recipe[0, 0])
                {
                    if (playerInv[x, 1] >= recipe[0, 1])
                    {
                        req1 = true;
                    }
                }
            }
            for (int x = 0; x < 18; x++)
            {
                if (playerInv[x, 0] == recipe[1, 0])
                {
                    if (playerInv[x, 1] >= recipe[1, 1])
                    {
                        req2 = true;
                    }
                }
            }
            return (req1 && req2);
        }
        public void CreateSlot1(Player player)
        {
            bool first = false;
            bool second = false;
            if (page1)
            {
                for (int x = 0; x < 18; x++)
                {
                    if (player.Inv[x, 0] == recipes[page, 0].requirements[0, 0] && player.Inv[x, 1] >= recipes[page, 0].requirements[0, 1] && !first)
                    {
                        first = true;        
                    }
                    else if (player.Inv[x, 0] == recipes[page, 0].requirements[1, 0] && player.Inv[x, 1] >= recipes[page, 0].requirements[1, 1])
                    {
                        second = true;
                        break;

                    }
                    else
                    {
                        second = false;
                    }
                }
                if (first && second)
                {
                    for (int num = recipes[page, 0].requirements[0, 1]; num > 0; num--)
                    {
                        player.InvDel(recipes[page, 0].requirements[0, 0]);
                    }
                    for (int num = recipes[page, 0].requirements[1, 1]; num > 0; num--)
                    {
                        player.InvDel(recipes[page, 0].requirements[1, 0]);
                    }

                    player.InvAddItem(recipes[page, 0].itemIndex);
                }
            }
        }
        public void CreateSlot2(Player player)
        {
            bool first = false;
            bool second = false;
            if (page2)
            {
                for (int x = 0; x < 18; x++)
                {
                    if (player.Inv[x, 0] == recipes[page, 1].requirements[0, 0] && player.Inv[x, 1] >= recipes[page, 1].requirements[0, 1] && !first)
                    {
                        first = true;

                    }
                    else
                    {
                        first = false;
                    }
                    if (player.Inv[x, 0] == recipes[page, 1].requirements[1, 0] && player.Inv[x, 1] >= recipes[page, 1].requirements[1, 1])
                    {
                        second = true;
                        break;
                    }
                    else
                    {
                        second = false;
                    }
                }
                if (first && second)
                {
                    for (int num = recipes[page, 1].requirements[0, 1]; num > 0; num--)
                    {
                        player.InvDel(recipes[page, 1].requirements[0, 0]);
                    }
                    for (int num = recipes[page, 1].requirements[1, 1]; num > 0; num--)
                    {
                        player.InvDel(recipes[page, 1].requirements[1, 0]);
                    }

                    player.InvAddItem(recipes[page, 1].itemIndex);
                }
            }
        }
    }
}
