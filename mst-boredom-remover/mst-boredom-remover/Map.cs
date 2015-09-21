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
        private char[,] map;

        public const int MAP_X = 1280 / 2;
        public const int MAP_Y = 720 / 2;
        private const int TILE_PX_SIZE = 28;
        private const int TILE_PX_SMALL = 2;
        private const int RES_X = 1280;
        private const int RES_Y = 720;
        private bool smallMode = false;
        private Vector2 tileIndex;
        private bool gDisable = false;
        private List<string> tileNames;
        //private string debugText = "";
        private List<Unit> units;
        private List<Texture2D> unitTextures;

        public int width;
        public int height;

        public Tile[,] tiles;

        public enum Directions
        {
            North,
            South,
            East,
            West
        }

        private enum UnitTypeTextures
        {
            Default,        // 0
            Swordman,       // 1
            Archer,         // 2
            Mage            // 3
        };

        struct BiomeInfo
        {
            public char Type;
            public int X;
            public int Y;
        };
        public Map(Vector2 position, List<Texture2D> tileTextures, ref List<Unit> units, List<Texture2D> unitTextures, int width = 0, int height = 0)
        {
            this.width = width;
            this.height = height;
            this.tiles = new Tile[width, height];

            this.position = position;
            this.tileTextures = tileTextures;
            this.unitTextures = unitTextures;
            //this.map = map;
            //this.texture = texture;

            tileIndex = Vector2.Zero;

            generator();

            tileNames = new List<string>();

            tileNames.Add("null");
            tileNames.Add("Plains");
            tileNames.Add("Mountain");
            tileNames.Add("Desert");
            tileNames.Add("Ocean");
            tileNames.Add("Dreadland");
            tileNames.Add("Tundra");
            tileNames.Add("Forest");

            this.units = units;
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
                    if (tileIndex.X + deltaX < MAP_X - ((RES_X / TILE_PX_SIZE)))
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
                    if (tileIndex.Y + deltaY < MAP_Y - ((RES_Y / TILE_PX_SIZE)))
                    {
                        tileIndex.Y += deltaY;
                    }
                }
            }
            //base.mapMove(deltaX, deltaY);
        }

        private void generator()
        {
            // chance out of 1000
            const int MAX_CHANCE = 1000;
            const int GOLD_CHANCE = 5;
            const int IRON_CHANCE = 2;
            const int MANA_CHANCE = 2;
            const int NumBio = 700;
            Random r = new Random();
            BiomeInfo[] bio = new BiomeInfo[NumBio];
            for (int i = 0; i < bio.Length; i++)
            {
                bio[i] = new BiomeInfo();
            }
            for (int a = 0; a < (NumBio / 7); a++)
            {
                bio[a * 7].Type = '~'; //Ocean
                bio[a * 7].X = r.Next(0, MAP_X);
                bio[a * 7].Y = r.Next(0, MAP_Y);

                bio[a * 7 + 1].Type = '+';//Plain
                bio[a * 7 + 1].X = r.Next(0, MAP_X);
                bio[a * 7 + 1].Y = r.Next(0, MAP_Y);

                bio[a * 7 + 2].Type = 'M';//Mountain
                bio[a * 7 + 2].X = r.Next(0, MAP_X);
                bio[a * 7 + 2].Y = r.Next(0, MAP_Y);

                bio[a * 7 + 3].Type = 'F';//Forest
                bio[a * 7 + 3].X = r.Next(0, MAP_X);
                bio[a * 7 + 3].Y = r.Next(0, MAP_Y);

                bio[a * 7 + 4].Type = '%';//Dreadlands
                bio[a * 7 + 4].X = r.Next(0, MAP_X);
                bio[a * 7 + 4].Y = r.Next(0, MAP_Y);

                bio[a * 7 + 5].Type = 'D';//Desert
                bio[a * 7 + 5].X = r.Next(0, MAP_X);
                bio[a * 7 + 5].Y = r.Next(0, MAP_Y);

                bio[a * 7 + 6].Type = 'T';//Tundra
                bio[a * 7 + 6].X = r.Next(0, MAP_X);
                bio[a * 7 + 6].Y = r.Next(0, MAP_Y);
            }

            char[,] field = new char[MAP_X, MAP_Y];
            // i = y
            // j = x
            for (int i = 0; i < MAP_Y; i++)
            {
                for (int j = 0; j < MAP_X; j++)
                {
                    char nearest = '~';
                    int dist = 5000;
                    for (int z = 0; z < NumBio; z++)
                    {
                        int Xdiff = bio[z].X - i;
                        int Ydiff = bio[z].Y - j;
                        int Cdist = Xdiff * Xdiff + Ydiff * Ydiff;
                        if (Cdist < dist)
                        {
                            nearest = bio[z].Type;
                            dist = Cdist;
                        }

                    }
                    field[j, i] = nearest;
                }
            }
            for (int i = 0; i < MAP_Y; i++)
            {
                field[0, i] = '~';          // left side
                field[MAP_X - 1, i] = '~';  // right side
            }
            for (int j = 0; j < MAP_X; j++)
            {
                field[j, 0] = '~';          // top
                field[j, MAP_Y - 1] = '~';  // bottom
            }
            //Adding Resources.
            for (int i = 0; i < MAP_Y; i++)
            {
                for (int j = 0; j < MAP_X; j++)
                {
                    if (field[j, i] == 'M')
                    {
                        if (r.Next(0, MAX_CHANCE) <= GOLD_CHANCE)
                            field[j, i] = 'G';//inserts gold mine resource
                    }
                    else if (field[j, i] == 'F')
                    {
                        if (r.Next(0, MAX_CHANCE) <= IRON_CHANCE)
                            field[j, i] = 'L';//sawmill for lumber
                    }
                    else if (field[j, i] == '%')
                    {
                        if (r.Next(0, MAX_CHANCE) <= MANA_CHANCE)
                            field[j, i] = '*';//magic crystal resource
                    }

                }
            }
            map = field;

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

            if (mouseIndex.X + tileIndex.X < 0 || mouseIndex.X + tileIndex.X > MAP_X)
            {
                fail = true;
            }
            if (mouseIndex.Y + tileIndex.Y < 0 || mouseIndex.Y + tileIndex.Y > MAP_Y)
            {
                fail = true;
            }
            if (fail == false)
            {
                c = map[(int)(mouseIndex.X + tileIndex.X), (int)(mouseIndex.Y + tileIndex.Y)];
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
                generator();
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
                    if (tileIndex.X < MAP_X)
                    {
                        for (int y = (int)position.Y; y < ((RES_Y / TILE_PX_SIZE)); y++)
                        {
                            if (tileIndex.Y < MAP_Y)
                            {
                                // 2D array draw tiles at their designated spots, in TILE_PX x TILE_PX squares
                                sb.Draw(tileTextures[charToInt(map[(int)tileIndex.X + x, (int)tileIndex.Y + y])],
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
                    if (tileIndex.X < MAP_X)
                    {
                        for (int y = (int)position.Y; y < ((RES_Y / TILE_PX_SMALL)); y++)
                        {
                            if (tileIndex.Y < MAP_Y)
                            {
                                // 2D array draw tiles at their designated spots, in TILE_PX x TILE_PX squares
                                sb.Draw(tileTextures[charToInt(map[(int)tileIndex.X + x, (int)tileIndex.Y + y])],
                                    new Rectangle((int)position.X + TILE_PX_SMALL * x, (int)position.Y + TILE_PX_SMALL * y, TILE_PX_SMALL, TILE_PX_SMALL), Color.White);
                            }
                        }
                    }
                }
            }

            int unitIndex = 0;
            Vector2 unitPos = new Vector2();
            Vector2 drawPos = new Vector2();

            foreach (Unit x in units)
            {
                // determine texture to draw
                if (x.type.movement_type == UnitType.MovementType.Walker)
                {
                    if (x.type.attack_type == UnitType.AttackType.Melee)
                    {
                        // sword
                        unitIndex = Convert.ToInt32(UnitTypeTextures.Swordman);
                    }
                    else if (x.type.attack_type == UnitType.AttackType.Arrow)
                    {
                        // archer
                        unitIndex = Convert.ToInt32(UnitTypeTextures.Archer);
                    }
                    else if (x.type.attack_type == UnitType.AttackType.Fireball)
                    {
                        // mage
                        unitIndex = Convert.ToInt32(UnitTypeTextures.Mage);
                    }
                }

                // map coordinate of unit
                unitPos.X = x.position.x;
                unitPos.Y = x.position.y;

                // calculate screen space based on map coordinates
                // (coordinate of the unit - coordinate of the camera) * tile_pixel_size
                drawPos = (unitPos - tileIndex) *TILE_PX_SIZE;

                // finally draw the unit
                sb.Draw(unitTextures[1], new Rectangle((int)drawPos.X, (int)drawPos.Y, TILE_PX_SIZE, TILE_PX_SIZE), Color.White);
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
