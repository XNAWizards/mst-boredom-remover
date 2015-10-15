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
    public class Player
    {
        Texture2D[] texture = new Texture2D[6]; // array of textures for animation
        public int hitPoints; // how many hits the player can take till death
        float fallSpeed = 300f; // how fast the player will move
        int animationFrame = 1; // which frame of the animation is active
        public Vector2 playerPosition; // pixel position
        double AnimTimer; // saves the time since last animation change
        public bool PlayingAnimation = false; // is the animation running
        public bool flip = false; // is the texture flipped
        Vector2 absolutePosition = new Vector2(0, 0); // world position in x,y        
        public float movementSpeed = 400f; // movement speed through the world
        public bool isFalling = false; // is gravity taking effect
        public bool canMoveLeft = true; // can the player move left
        public bool canMoveRight = true; // can the player move right
        public double jumpTimer = 1; // how long will the jump last
        public float jumpSpeed = 0; // stores the calculated jump speed based on a quadratic function of time
        public bool jumping = false; // is the player jumping
        public int[,] Inv = new int[18, 2]; // inventory slots, (item id, quantity)
        public int selectedInvSlot = 0;
        List<Texture2D> miniInvSlots = new List<Texture2D>();
        List<Texture2D> equipment = new List<Texture2D>();
        
        
        Texture2D fallTexture; // texture used when not standing on the ground

        public Rectangle bounds;
        
        public Player(Texture2D[] texture, Texture2D fallTexture, List<Texture2D> equipment)
        {
            this.texture = texture;
            playerPosition = new Vector2(400 - texture[0].Width / 2, 305 - texture[0].Height / 2);
            this.fallTexture = fallTexture;

        }
        public void Update(GameTime gameTime, Area area, Button invButton)
        {
            invButton.ChangeTexture(miniInvSlots[selectedInvSlot]);
            if (jumpTimer <= .3)
            {
                jumping = false;
            }
            if (jumping)
            {
                jumpSpeed = 300 * (float)Math.Pow((jumpTimer), 2) + (float)jumpTimer + 2;
                if (jumpTimer == 1.5 && area.colliding || !area.colliding)
                {
                    area.smallDifference.Y += (jumpSpeed * (1 / Global.ScreenHeight));
                }
                jumpTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            }

            absolutePosition.X = area.difference.X + 30;
            absolutePosition.Y = area.difference.Y + 26;

            AnimTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (PlayingAnimation)
            {
                switch (animationFrame)
                {
                    case 1:
                        if (AnimTimer > .1)
                        {
                            animationFrame = 2;
                            AnimTimer = 0;
                        }
                        break;
                    case 2:
                        if (AnimTimer > .1)
                        {
                            animationFrame = 3;
                            AnimTimer = 0;
                        }
                        break;
                    case 3:
                        if (AnimTimer > .1)
                        {
                            animationFrame = 4;
                            AnimTimer = 0;
                        }
                        break;
                    case 4:
                        if (AnimTimer > .1)
                        {
                            animationFrame = 5;
                            AnimTimer = 0;
                        }
                        break;
                    case 5:
                        if (AnimTimer > .1)
                        {
                            animationFrame = 6;
                            AnimTimer = 0;
                        }
                        break;
                    case 6:
                        if (AnimTimer > .1)
                        {
                            animationFrame = 1;
                            AnimTimer = 0;
                        }
                        break;
                }//end of switch
            }//End of if
            else
            {
                animationFrame = 1;
            }
            if (isFalling && !Global.debugMode)
            {
                area.smallDifference.Y -= fallSpeed * (1 / Global.ScreenHeight);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isFalling && flip)
            {
                spriteBatch.Draw(fallTexture, playerPosition, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 0);
            }
            else if (isFalling && !flip)
            {
                spriteBatch.Draw(fallTexture, playerPosition, Color.White);
            }
            else if (flip && !isFalling)
            {
                spriteBatch.Draw(texture[animationFrame - 1], playerPosition, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                spriteBatch.Draw(texture[animationFrame - 1], playerPosition, Color.White);
            }
        }
        public void AddMiniInv(List<Texture2D> miniInvSlots)
        {
            this.miniInvSlots = miniInvSlots;
        }
        public void ChangeInvSlot(int slot)
        {
            this.selectedInvSlot = slot;
        }
        public void InvAdd(int BlockID)
        {
            if (BlockID != 0)
            {
                if (BlockID == 1 || BlockID == 2 || BlockID == 3)//Dirt
                {
                    BlockID = 1;
                }
                else if (BlockID == 4 || BlockID == 5 || BlockID == 6)// stone
                {
                    BlockID = 4;
                }

                bool IfFound = false;
                for (int x = 0; x < 18; x++)
                {
                    if (Inv[x, 0] == BlockID)
                    {
                        Inv[x, 1] += 1;
                        IfFound = true;
                        break;
                    }
                }
                if (!IfFound)
                {
                    for (int x = 0; x < 18; x++)
                    {
                        if (Inv[x, 0] == 0)
                        {
                            Inv[x, 0] = BlockID;
                            Inv[x, 1] += 1;
                            break;
                        }
                    }
                }
            }
        }
        public void InvAddItem(int ItemID)
        {
            for (int x = 0; x < 18; x++)
            {
                if (Inv[x, 0] == 0)
                {
                    Inv[x, 0] = ItemID + 100;
                    Inv[x, 1]++;
                    break;
                }
            }
        }
        public bool InvDel(int BlockID)
        {
            for (int x = 0; x < 18; x++)
            {
                if (Inv[x, 0] == BlockID)
                {
                    if (Inv[x, 1] != 0)
                    {
                        Inv[x, 1] -= 1;
                    }
                    if (Inv[x, 1] == 0)
                    {
                        Inv[x, 0] = 0;
                    }
                    return true;
                }
            }
            return false;

        }
        public void LoadData(int hitPoints, int[,] inventory)
        {
            this.hitPoints = hitPoints;
            this.Inv = inventory;
        }
        public void Reset()
        {
            canMoveLeft = true;
            canMoveRight = true;
            this.AnimTimer = 0;
            this.fallSpeed = 300;
            this.flip = false;
            this.isFalling = false;
            this.PlayingAnimation = false;
            this.selectedInvSlot = 0;
            this.jumpTimer = 0;
            for (int x = 0; x < 18; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    Inv[x,y] = 0;
                }
            }
        }


        public int GetSlotTexture(int selectedInvSlot)
        {
            return Inv[selectedInvSlot, 0];
        }
        public void MoveSlots(int oldSlot, int newSlot)
        {
            int save0 = Inv[newSlot, 0];
            int save1 = Inv[newSlot, 1];

            Inv[newSlot, 0] = Inv[oldSlot, 0];
            Inv[newSlot, 1] = Inv[oldSlot, 1];

            Inv[oldSlot, 0] = save0;
            Inv[oldSlot, 1] = save1;
        }
    }
}
