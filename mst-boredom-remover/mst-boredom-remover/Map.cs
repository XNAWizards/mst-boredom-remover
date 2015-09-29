using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;

namespace mst_boredom_remover
{
    class Map : UIObject
    {
        private Vector2 position;
        private List<Texture2D> tileTextures;
        private char[,] charmap;

        public int width;
        public int height;
        private const int TILE_PX_SIZE = 8;
        private const int TILE_PX_SMALL = 2;
        private const int RES_X = 1280;
        private const int RES_Y = 720;
        private bool smallMode = false;
        private Vector2 tileIndex;
        private bool gDisable = false;
        private List<string> tileNames;
        //private string debugText = "";
        private Engine game;

        private enum UnitTypeTextures
        {
            Default,        // 0
            Swordman,       // 1
            Archer,         // 2
            Mage            // 3
        };

        public struct BiomeInfo
        {
            public char Type;
            public int X;
            public int Y;
        };
        public Map(Vector2 position, List<Texture2D> tileTextures, int width, int height, ref Engine game)
        {
            this.width = width;
            this.height = height;

            this.position = position;
            this.tileTextures = tileTextures;
            //this.map = map;
            //this.texture = texture;

            tileIndex = Vector2.Zero;

            this.charmap = Generator.generate(width, height);

            tileNames = new List<string>();

            tileNames.Add("null");
            tileNames.Add("Plains");
            tileNames.Add("Mountain");
            tileNames.Add("Desert");
            tileNames.Add("Ocean");
            tileNames.Add("Dreadland");
            tileNames.Add("Tundra");
            tileNames.Add("Forest");
            
            this.game = game;
        }

        public override void changeContext(int id)
        {
            //base.changeContext(id);

            if (id == 2)
            {
                // small mode
                smallMode = true;
            }
            else
            {
                // regular mode
                smallMode = false;
            }
        }

        public override void mapMove(int deltaX, int deltaY)
        {
            gDisable = false;
            if (deltaX != 0)
            {
                if (tileIndex.X + deltaX >= 0)
                {
                    if (tileIndex.X + deltaX < width - ((RES_X / TILE_PX_SIZE)))
                    {
                        tileIndex.X += deltaX;
                    }
                }
            }
            if (deltaY != 0)
            {
                if (tileIndex.Y + deltaY >= 0)
                {
                    // res_y/px_size = number of tiles that fit on screen
                    // map_Y - #tiles on screen = maximum tileIndex.Y to allow
                    if (tileIndex.Y + deltaY < height - ((RES_Y / TILE_PX_SIZE)))
                    {
                        tileIndex.Y += deltaY;
                    }
                }
            }
            //base.mapMove(deltaX, deltaY);
        }

        public override void toggleDebugMode()
        {
            debugMode = !debugMode;
            //base.toggleDebugMode();
        }

        private void debugUpdate(GameTime gt)
        {
            MouseState m = Mouse.GetState();
            char c = ' ';
            bool fail = false;

            // find out what tile is at tileIndex + MousePos / tile_px_size

            Vector2 mouseIndex = new Vector2(m.X / TILE_PX_SIZE, m.Y / TILE_PX_SIZE);

            if (mouseIndex.X + tileIndex.X < 0 || mouseIndex.X + tileIndex.X > width)
            {
                fail = true;
            }
            if (mouseIndex.Y + tileIndex.Y < 0 || mouseIndex.Y + tileIndex.Y > height)
            {
                fail = true;
            }
            if (fail == false)
            {
                c = charmap[(int)(mouseIndex.X + tileIndex.X), (int)(mouseIndex.Y + tileIndex.Y)];
            }

            debugText = "";
            debugText += "Mouse Position: (" + m.X + ", " + m.Y + ")\n";
            debugText += "TileIndex: (" + tileIndex.X + ", " + tileIndex.Y + ")\n";
            debugText += tileNames[charToInt(c)];
            
        }
        private void debugDraw(SpriteBatch sb)
        {
            // menu draws all of its controls' debug texts
            //sb.DrawString(font, debugText, new Vector2(0, 0), Color.Black);
        }

        public override void changeFont(SpriteFont f)
        {
            //this.font = f;
            //base.changeFont(f);
        }

        public override void Update(GameTime gt)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.G) && gDisable == false)
            {
                // generate a new map quickly
                charmap = Generator.generate(width, height);
                gDisable = true;
            }
            if (keyboard.IsKeyDown(Keys.W))
            {
                mapMove(0, -1);
                
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                mapMove(-1, 0);

            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                mapMove(0, 1);

            }
            else if (keyboard.IsKeyDown(Keys.D))
            {
                mapMove(1, 0);

            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                int i = 0;
                foreach (Unit unit in game.units)
                {
                    unit.orders.Add(Order.CreateMoveOrder(new Position((Mouse.GetState().X / TILE_PX_SIZE) + (int)tileIndex.X, (Mouse.GetState().Y / TILE_PX_SIZE) + (int)tileIndex.Y)));
                    game.ScheduleUpdate(10, unit);
                    i++;
                }
            }

            if (debugMode)
            {
                debugUpdate(gt);
            }
            //base.Update(gt);
        }

        public override void Draw(SpriteBatch sb)
        {
            // not small mode
            if (!smallMode)
            {
                // keep drawing until we run out of screen space, x and y
                for (int x = (int)position.X; x < ((RES_X / TILE_PX_SIZE)); x++)
                {
                    if (tileIndex.X < width)
                    {
                        for (int y = (int)position.Y; y < ((RES_Y / TILE_PX_SIZE)); y++)
                        {
                            if (tileIndex.Y < height)
                            {
                                // 2D array draw tiles at their designated spots, in TILE_PX x TILE_PX squares
                                sb.Draw(tileTextures[charToInt(charmap[(int)tileIndex.X + x, (int)tileIndex.Y + y])],
                                    new Rectangle((int)position.X + TILE_PX_SIZE * x, (int)position.Y + TILE_PX_SIZE * y, TILE_PX_SIZE + 4, TILE_PX_SIZE + 4), Color.White);
                            }
                        }
                    }
                }
            }
            // small mode for zoomed out view
            else
            {
                // keep drawing until we run out of screen space, x and y
                for (int x = (int)position.X; x < ((RES_X / TILE_PX_SMALL)); x++)
                {
                    if (tileIndex.X < width)
                    {
                        for (int y = (int)position.Y; y < ((RES_Y / TILE_PX_SMALL)); y++)
                        {
                            if (tileIndex.Y < height)
                            {
                                // 2D array draw tiles at their designated spots, in TILE_PX x TILE_PX squares
                                sb.Draw(tileTextures[charToInt(charmap[(int)tileIndex.X + x, (int)tileIndex.Y + y])],
                                    new Rectangle((int)position.X + TILE_PX_SMALL * x, (int)position.Y + TILE_PX_SMALL * y, TILE_PX_SMALL, TILE_PX_SMALL), Color.White);
                            }
                        }
                    }
                }
            }

            // Draw units
            // TODO: Only do this for units on screen, probably with the help of a quad-tree
            foreach (Unit unit in game.units)
            {
                // TODO: Move current_textures outside of this loop and into unit logic
                Texture2D[] current_textures = null;
                switch (unit.status)
                {
                    case Unit.Status.Idle:
                        current_textures = unit.type.idle_textures;
                        break;
                    case Unit.Status.Moving:
                        current_textures = unit.type.move_textures;
                        break;
                    case Unit.Status.Attacking:
                        current_textures = unit.type.attack_textures;
                        break;
                }

                // calculate screen space based on map coordinates
                // (coordinate of the unit - coordinate of the camera) * tile_pixel_size
                Vector2 draw_position = (unit.position.ToVector2() - tileIndex) * TILE_PX_SIZE;
          
                // finally draw the unit
                sb.Draw(current_textures[(game.current_tick - unit.animation_start_tick) % current_textures.Length],
                    new Rectangle((int)draw_position.X, (int)draw_position.Y, TILE_PX_SIZE, TILE_PX_SIZE), Color.White);
            }

            if (debugMode)
            {
                debugDraw(sb);
            }
            //base.Draw(sb);
        }

        private int charToInt(char c)
        {
            switch (c)
            {
                case '+':
                    return 1;
                case 'M':
                    return 2;
                case 'D':
                    return 3;
                case '~':
                    return 4;
                case '%':
                    return 5;
                case 'T':
                    return 6;
                case 'F':
                    return 7;
                default:
                    // 0 is not a tile, more efficient than try-catch index out of bounds
                    return 0;
            }
        }
    }
}
