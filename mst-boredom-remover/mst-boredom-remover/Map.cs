using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
using mst_boredom_remover.engine;

namespace mst_boredom_remover
{
    class Map : UiObject
    {
        private Vector2 position;
        private List<Texture2D> tileTextures;
        private char[,] charmap;
        GraphicsDevice graphicsDevice;
        bool buildMapCache = true;

        // max x = 11520
        // max y = 6480

        // 4096 6 px tiles
        // 8192 12 px tiles
        // 12288 18 px tiles
        // 3x2 array of textures for the whole map
        // outer textures are shorter.
        //  768 extra on right side
        //  1712 extra on bottom
        // =
        /*
         *  4096x4096, 4096x4096, 3328x4096
         *  4096x2384, 4096x2384, 3328x2384
         * 
         *  create 6 megatextures by copying data of tiles rendered at 18x18 pixels.
         * */

        public int width;
        public int height;
        private const int tilePxSize = 18;
        private int pxMod = 0;
        private int savePxMod = 0;
        private float zoom = 1.0f;
        private const int resX = 1280;
        private const int resY = 720;
        private bool smallMode = false;
        private Vector2 tileIndex;
        private bool gDisable = false;
        private List<string> tileNames;
        //private string debugText = "";
        private Engine engine;
        private int lastScrollValue;
        private List<Unit> selectedUnits;
        
        private ButtonState previousLeftMouseState = ButtonState.Released;
        private ButtonState previousRightMouseState = ButtonState.Released;

        RenderTarget2D[,] mapCaches;

        private Vector2 mouseTile = new Vector2();

        private enum UnitTypeTextures
        {
            Default,        // 0
            Swordman,       // 1
            Archer,         // 2
            Mage            // 3
        };

        public Map(Vector2 position, List<Texture2D> tileTextures, int width, int height, ref Engine engine, GraphicsDevice graphicsDevice)
        {
            this.width = width;
            this.height = height;

            this.position = position;
            this.tileTextures = tileTextures;
            //this.map = map;
            //this.texture = texture;

            tileIndex = Vector2.Zero;

            this.charmap = Generator.Generate(width, height);

            tileNames = new List<string>();

            tileNames.Add("null");
            tileNames.Add("Plains");
            tileNames.Add("Mountain");
            tileNames.Add("Desert");
            tileNames.Add("Ocean");
            tileNames.Add("Dreadland");
            tileNames.Add("Tundra");
            tileNames.Add("Forest");
            
            this.engine = engine;

            this.graphicsDevice = graphicsDevice;
            mapCaches = new RenderTarget2D[3, 2];
            RenderTarget2D r1 = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
            RenderTarget2D r2 = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
            RenderTarget2D r3 = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
            RenderTarget2D r4 = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
            RenderTarget2D r5 = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
            RenderTarget2D r6 = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
            mapCaches[0, 0] = r1;
            mapCaches[1, 0] = r2;
            mapCaches[2, 0] = r3;
            mapCaches[0, 1] = r4;
            mapCaches[1, 1] = r5;
            mapCaches[2, 1] = r6;

            selectedUnits = new List<Unit>();
            /*
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    RenderTarget2D r = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
                    mapCaches[x, y] = r;
                }
            }*/
        }

        public override void ChangeContext(int id)
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

        public void MapMove(int deltaX, int deltaY)
        {
            gDisable = false;
            
            if (deltaX != 0)
            {
                if (tileIndex.X + deltaX >= 0)
                {
                    if (tileIndex.X + deltaX < width - (resX / (tilePxSize + pxMod)))
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
                    // height - #tiles on screen = maximum tileIndex.Y to allow
                    if (tileIndex.Y + deltaY < height - (resY / (tilePxSize + pxMod)))
                    {
                        tileIndex.Y += deltaY;
                    }
                }
            }
        }

        // returns a list of units in the bounds provided.
        public List<Unit> select(Rectangle bounds)
        {
            int startTileX;
            int startTileY;
            int tileWidth;
            int tileHeight;

            // determine which tile the selection box is starting at
            startTileX = bounds.X / (int)((tilePxSize + pxMod) + tileIndex.X);
            startTileY = bounds.Y / (int)((tilePxSize + pxMod) + tileIndex.Y);
            tileWidth = Math.Max(bounds.Width / (int)((tilePxSize + pxMod) + tileIndex.X), 1);
            tileHeight = Math.Max(bounds.Height / (int)((tilePxSize + pxMod) + tileIndex.Y), 1);

            selectedUnits.Clear();

            int masterX = startTileX;
            int masterY = startTileY;
            // search only the grid. hopefully small n^24
            for (int y = 0; y < tileHeight; y++)
            {
                for (int x = 0; x < tileWidth; x++)
                {
                    if (engine.unitGrid[masterX + x, masterY + y] != null)
                    {
                        engine.unitGrid[masterX + x, masterY + y].selected = true;
                        selectedUnits.Add(engine.unitGrid[masterX+ x, masterY + y]);
                    }
                }
            }

            return selectedUnits;
        }
        
        public override void ToggleDebugMode()
        {
            debugMode = !debugMode;
            //base.toggleDebugMode();
        }

        private void DebugUpdate(GameTime gt)
        {
            MouseState m = Mouse.GetState();
            char c = ' ';
            bool fail = false;

            // find out what tile is at tileIndex + MousePos / tilePxSize

            Vector2 mouseIndex = new Vector2(m.X / tilePxSize, m.Y / tilePxSize);

            if (mouseTile.X < 0 || mouseTile.X > width)
            {
                fail = true;
            }
            if (mouseTile.Y < 0 || mouseTile.Y > height)
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
            debugText += "Mouse Tile: (" + mouseTile.X + ", " + mouseTile.Y + ")\n";
            debugText += "TileIndex: (" + tileIndex.X + ", " + tileIndex.Y + ")\n";
            debugText += tileNames[CharToInt(c)];
            
        }
        private void DebugDraw(SpriteBatch sb)
        {
            // menu draws all of its controls' debug texts
        }

        public override void ChangeFont(SpriteFont f)
        {
            //this.font = f;
            //base.changeFont(f);
        }

        private void ForceBounds()
        {
            // min bounds
            if (tileIndex.X < 0)
            {
                tileIndex.X = 0;
            }
            if (tileIndex.Y < 0)
            {
                tileIndex.Y = 0;
            }
            // max bounds
            if (tileIndex.X + (resX / (tilePxSize + pxMod)) > width)
            {
                tileIndex.X = width - (resX / (tilePxSize + pxMod));
            }
            if (tileIndex.Y + (resY / (tilePxSize + pxMod)) > height)
            {
                tileIndex.Y = height - (resY / (tilePxSize + pxMod));
            }
        }

        private void ZoomIn()
        {
            if (pxMod < 18)
            {
                pxMod += 2;
                // calculate zoom factor for drawing.

                zoom = (float)(tilePxSize + pxMod) / (float)tilePxSize;
                
                // calculate number of tiles after zoom
                int tilesX = resX / (tilePxSize + pxMod);
                int tilesY = resY / (tilePxSize + pxMod);
                // new tileIndex =  mouseTile - tiles after/2
                tileIndex.X = mouseTile.X - tilesX / 2;
                tileIndex.Y = mouseTile.Y - tilesY / 2;

                ForceBounds();
            }
        }
        private void ZoomOut()
        {
            // zoom out
            if (pxMod > -16)
            {
                pxMod -= 2;
                
                zoom = (float)(tilePxSize + pxMod) / (float)tilePxSize;
                
                // calculate number of tiles after zoom
                int tilesX = resX / (tilePxSize + pxMod);
                int tilesY = resY / (tilePxSize + pxMod);
                // new tileIndex =  mouseTile - tiles after/2
                tileIndex.X = mouseTile.X - tilesX / 2;
                tileIndex.Y = mouseTile.Y - tilesY / 2;

                ForceBounds();
            }
        }

        private void RebuildMapCache(SpriteBatch sb, RenderTarget2D cache, int numX, int numY)
        {
            int startX = numX * 214;
            int startY = numY * 214;
            sb.End();
            graphicsDevice.SetRenderTarget(cache);
            sb.Begin();

            for (int x = startX; x < startX + 214 && x < width; x++)
            {
                for (int y = startY; y < startY + 214 && y < height; y++)
                {
                    sb.Draw(tileTextures[CharToInt(charmap[x, y])], new Rectangle((x - (numX * 214)) * (tilePxSize + pxMod), (y - (numY * 214)) * (tilePxSize + pxMod), (tilePxSize + pxMod), (tilePxSize + pxMod)), Color.White);
                }
             }
                
            sb.End();
            graphicsDevice.SetRenderTarget(null);
            sb.Begin();
        }

        public void unitGroupMove(List<Unit> selected_units)
        {
            Position mouse_game_tile_position = new Position((int)mouseTile.X, (int)mouseTile.Y);

            var enumerator = engine.map.BreadthFirst(mouse_game_tile_position).GetEnumerator();
            enumerator.MoveNext();
            foreach (Unit unit in selected_units)
            {
                if (engine.unitGrid[mouse_game_tile_position.x, mouse_game_tile_position.y] == unit) // Produce units
                {
                    engine.OrderProduce(unit, engine.unitTypes[0]);
                    break;
                }
                else // Move units
                {
                    while (engine.unitGrid[enumerator.Current.x, enumerator.Current.y] != null)
                    {
                        enumerator.MoveNext();
                    }
                    engine.OrderMove(unit, enumerator.Current);
                    enumerator.MoveNext();
                }
            }
        }

        public override void Update(GameTime gt)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.G) && gDisable == false)
            {
                // generate a new map, reconstruct cache
                charmap = Generator.Generate(width, height);
                gDisable = true;
                buildMapCache = true;
                savePxMod = pxMod;
                pxMod = 0;
            }

            if (keyboard.IsKeyDown(Keys.W))
            {
                MapMove(0, -1);
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                MapMove(-1, 0);
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                MapMove(0, 1);
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                MapMove(1, 0);
            }

            MouseState m = Mouse.GetState();

            mouseTile.X = m.X / (tilePxSize + pxMod) + tileIndex.X;
            mouseTile.Y = m.Y / (tilePxSize + pxMod) + tileIndex.Y;

            // scroll up
            if (m.ScrollWheelValue > lastScrollValue)
            {
                ZoomIn();
            }
            // scroll down
            else if (m.ScrollWheelValue < lastScrollValue)
            {
                ZoomOut();
            }

            lastScrollValue = m.ScrollWheelValue;
            
            if (debugMode)
            {
                DebugUpdate(gt);
            }
            
            // Update mouse states
            previousLeftMouseState = m.LeftButton;
            previousRightMouseState = m.RightButton;
        }

        public override void Draw(SpriteBatch sb)
        {
            // if the cache was erased for whatever reason, reconstruct
            if (mapCaches[0, 0].IsContentLost)
            {
                buildMapCache = true;
                savePxMod = pxMod;
                pxMod = 0;
            }

            // build the cache texture. takes a bit of time, avoid as much as possible
            if (buildMapCache)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        RebuildMapCache(sb, mapCaches[x, y], x, y);
                    }
                }

                buildMapCache = false;
                pxMod = savePxMod;
            }

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    // base position
                    int xPos = x * mapCaches[x, y].Width;
                    int yPos = y * mapCaches[x, y].Height;

                    // up/down/left/right translation
                    xPos += (int)-tileIndex.X * (tilePxSize + pxMod);
                    yPos += (int)-tileIndex.Y * (tilePxSize + pxMod);

                    // zoom offset
                    xPos -= x * (int)(mapCaches[x, y].Width - ((float)mapCaches[x, y].Width * zoom));
                    yPos -= y * (int)(mapCaches[x, y].Height - ((float)mapCaches[x, y].Height * zoom));

                    // extra offset to account for float inaccuracy, does not occur when zoomed in
                    if (zoom < 1.0f)
                    {
                        xPos -= x;
                        yPos -= y;
                    }

                    // finally draw the cache
                    sb.Draw(mapCaches[x, y], new Vector2(xPos, yPos), null, Color.White, 0f, Vector2.Zero, zoom, SpriteEffects.None, 1f);
                }
            }

            // Draw units
            // TODO: Only do this for units on screen, probably with the help of a quad-tree
            foreach (Unit unit in engine.units)
            {
                // TODO: Move current_textures outside of this loop and into unit logic
                Texture2D[] currentTextures = null;
                switch (unit.status)
                {
                    case Unit.Status.Idle:
                        currentTextures = unit.type.idleTextures;
                        break;
                    case Unit.Status.Moving:
                        currentTextures = unit.type.moveTextures;
                        break;
                    case Unit.Status.Attacking:
                        currentTextures = unit.type.attackTextures;
                        break;
                }

                // calculate screen space based on map coordinates
                // (coordinate of the unit - coordinate of the camera) * tile_pixel_size
                Vector2 drawPosition = (unit.position.ToVector2() - tileIndex) * (tilePxSize + pxMod);

                Color c = Color.White;

                if (unit.selected)
                {
                    c = Color.Red;
                }

                // finally draw the unit
                sb.Draw(currentTextures[(engine.currentTick- unit.animationStartTick) % currentTextures.Length],
                    new Rectangle((int)drawPosition.X, (int)drawPosition.Y, (tilePxSize + pxMod), (tilePxSize + pxMod)), c);
            }

            if (debugMode)
            {
                DebugDraw(sb);
            }
            //base.Draw(sb);
        }

        private int CharToInt(char c)
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
