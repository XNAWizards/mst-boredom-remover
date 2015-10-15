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
        private const int TILE_PX_SIZE = 18;
        private int px_mod = 0;
        private int savePx_mod = 0;
        private float zoom = 1.0f;
        private const int RES_X = 1280;
        private const int RES_Y = 720;
        private bool smallMode = false;
        private Vector2 tileIndex;
        private bool gDisable = false;
        private List<string> tileNames;
        //private string debugText = "";
        private Engine game;
        private List<Unit> selected_units = new List<Unit>();
        private int lastScrollValue;
        
        private ButtonState previous_left_mouse_state = ButtonState.Released;
        private ButtonState previous_right_mouse_state = ButtonState.Released;

        RenderTarget2D[,] mapCaches;

        private Vector2 mouseTile = new Vector2();

        private enum UnitTypeTextures
        {
            Default,        // 0
            Swordman,       // 1
            Archer,         // 2
            Mage            // 3
        };

        public Map(Vector2 position, List<Texture2D> tileTextures, int width, int height, ref Engine game, GraphicsDevice graphicsDevice)
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

            this.graphicsDevice = graphicsDevice;
            mapCaches = new RenderTarget2D[3, 2];
            RenderTarget2D r1 = new RenderTarget2D(graphicsDevice, 214 * TILE_PX_SIZE, 214 * TILE_PX_SIZE);
            RenderTarget2D r2 = new RenderTarget2D(graphicsDevice, 214 * TILE_PX_SIZE, 214 * TILE_PX_SIZE);
            RenderTarget2D r3 = new RenderTarget2D(graphicsDevice, 214 * TILE_PX_SIZE, 214 * TILE_PX_SIZE);
            RenderTarget2D r4 = new RenderTarget2D(graphicsDevice, 214 * TILE_PX_SIZE, 214 * TILE_PX_SIZE);
            RenderTarget2D r5 = new RenderTarget2D(graphicsDevice, 214 * TILE_PX_SIZE, 214 * TILE_PX_SIZE);
            RenderTarget2D r6 = new RenderTarget2D(graphicsDevice, 214 * TILE_PX_SIZE, 214 * TILE_PX_SIZE);
            mapCaches[0, 0] = r1;
            mapCaches[1, 0] = r2;
            mapCaches[2, 0] = r3;
            mapCaches[0, 1] = r4;
            mapCaches[1, 1] = r5;
            mapCaches[2, 1] = r6;
            /*
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    RenderTarget2D r = new RenderTarget2D(graphicsDevice, 214 * TILE_PX_SIZE, 214 * TILE_PX_SIZE);
                    mapCaches[x, y] = r;
                }
            }*/
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

        public void mapMove(int deltaX, int deltaY)
        {
            gDisable = false;
            
            if (deltaX != 0)
            {
                if (tileIndex.X + deltaX >= 0)
                {
                    if (tileIndex.X + deltaX < width - (RES_X / (TILE_PX_SIZE + px_mod)))
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
                    if (tileIndex.Y + deltaY < height - (RES_Y / (TILE_PX_SIZE + px_mod)))
                    {
                        tileIndex.Y += deltaY;
                    }
                }
            }
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
                try
                {
                    c = charmap[(int)(mouseTile.X), (int)(mouseTile.Y)];
                }
                catch (Exception)
                {
                    c = '!';
                }
                c = charmap[(int)(mouseIndex.X + tileIndex.X), (int)(mouseIndex.Y + tileIndex.Y)];
            }

            debugText = "";
            debugText += "Mouse Position: (" + m.X + ", " + m.Y + ")\n";
            debugText += "Mouse Tile: (" + mouseTile.X + ", " + mouseTile.Y + ")\n";
            debugText += "TileIndex: (" + tileIndex.X + ", " + tileIndex.Y + ")\n";
            debugText += tileNames[charToInt(c)];
            
        }
        private void debugDraw(SpriteBatch sb)
        {
            // menu draws all of its controls' debug texts
        }

        public override void changeFont(SpriteFont f)
        {
            //this.font = f;
            //base.changeFont(f);
        }

        private void forceBounds()
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
            if (tileIndex.X + (RES_X / (TILE_PX_SIZE + px_mod)) > width)
            {
                tileIndex.X = width - (RES_X / (TILE_PX_SIZE + px_mod));
            }
            if (tileIndex.Y + (RES_Y / (TILE_PX_SIZE + px_mod)) > height)
            {
                tileIndex.Y = height - (RES_Y / (TILE_PX_SIZE + px_mod));
            }
        }

        private void zoomIn()
        {
            if (px_mod < 18)
            {
                px_mod += 2;
                // calculate zoom factor for drawing.

                zoom = (float)(TILE_PX_SIZE + px_mod) / (float)TILE_PX_SIZE;
                
                // calculate number of tiles after zoom
                int tilesX = RES_X / (TILE_PX_SIZE + px_mod);
                int tilesY = RES_Y / (TILE_PX_SIZE + px_mod);
                // new tileIndex =  mouseTile - tiles after/2
                tileIndex.X = mouseTile.X - tilesX / 2;
                tileIndex.Y = mouseTile.Y - tilesY / 2;

                forceBounds();
            }
        }
        private void zoomOut()
        {
            // zoom out
            if (px_mod > -16)
            {
                px_mod -= 2;
                
                zoom = (float)(TILE_PX_SIZE + px_mod) / (float)TILE_PX_SIZE;
                
                // calculate number of tiles after zoom
                int tilesX = RES_X / (TILE_PX_SIZE + px_mod);
                int tilesY = RES_Y / (TILE_PX_SIZE + px_mod);
                // new tileIndex =  mouseTile - tiles after/2
                tileIndex.X = mouseTile.X - tilesX / 2;
                tileIndex.Y = mouseTile.Y - tilesY / 2;

                forceBounds();
            }
        }

        private void rebuildMapCache(SpriteBatch sb, RenderTarget2D cache, int numX, int numY)
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
                    sb.Draw(tileTextures[charToInt(charmap[x, y])], new Rectangle((x - (numX * 214)) * (TILE_PX_SIZE + px_mod), (y - (numY * 214)) * (TILE_PX_SIZE + px_mod), (TILE_PX_SIZE + px_mod), (TILE_PX_SIZE + px_mod)), Color.White);
                }
             }
                
            sb.End();
            graphicsDevice.SetRenderTarget(null);
            sb.Begin();
        }

        public override void Update(GameTime gt)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.G) && gDisable == false)
            {
                // generate a new map, reconstruct cache
                charmap = Generator.generate(width, height);
                gDisable = true;
                buildMapCache = true;
                savePx_mod = px_mod;
                px_mod = 0;
            }

            if (keyboard.IsKeyDown(Keys.W))
            {
                mapMove(0, -1);
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                mapMove(-1, 0);
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                mapMove(0, 1);
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                mapMove(1, 0);
            }

            MouseState m = Mouse.GetState();

            mouseTile.X = m.X / (TILE_PX_SIZE + px_mod) + tileIndex.X;
            mouseTile.Y = m.Y / (TILE_PX_SIZE + px_mod) + tileIndex.Y;

            // scroll up
            if (m.ScrollWheelValue > lastScrollValue)
            {
                zoomIn();
            }
            // scroll down
            else if (m.ScrollWheelValue < lastScrollValue)
            {
                zoomOut();
            }

            lastScrollValue = m.ScrollWheelValue;

            Position mouse_game_tile_position = new Position((int)mouseTile.X, (int)mouseTile.Y);
            
            // Select units
            if (m.RightButton == ButtonState.Pressed && previous_right_mouse_state == ButtonState.Released)
            {
                if (game.unit_grid[mouse_game_tile_position.x, mouse_game_tile_position.y] != null)
                {
                    Unit target_unit = game.unit_grid[mouse_game_tile_position.x, mouse_game_tile_position.y];
                    if (selected_units.Contains(target_unit))
                    {
                        selected_units.Remove(target_unit);
                    }
                    else
                    {
                        selected_units.Add(target_unit);
                    }
                }
            }

            
            if (m.LeftButton == ButtonState.Pressed && previous_left_mouse_state == ButtonState.Released)
            {
                var enumerator = game.map.BreadthFirst(mouse_game_tile_position).GetEnumerator();
                enumerator.MoveNext();
                Unit clickedSpot = game.unit_grid[mouse_game_tile_position.x, mouse_game_tile_position.y];
                foreach (Unit unit in selected_units)
                {
                    if (clickedSpot == unit) // Produce units
                    {
                        game.OrderProduce(unit, game.unit_types[0]);
                        break;
                    }
                    else if ( clickedSpot != null ) //Clicked a different unit TODO:make it so you don't attack your buddies
                    {
                        if ( selected_units.Contains(clickedSpot) ) //this check makes it so that you can produce without have the other selected units attack
                        {
                            continue;
                        }
                        game.OrderAttack(unit, clickedSpot);
                    }
                    else // Move units
                    {
                        while (game.unit_grid[enumerator.Current.x, enumerator.Current.y] != null)
                        {
                            enumerator.MoveNext();
                        }
                        game.OrderMove(unit, enumerator.Current);
                        enumerator.MoveNext();
                    }
                }
            }
            
            if (debugMode)
            {
                debugUpdate(gt);
            }
            
            // Update mouse states
            previous_left_mouse_state = m.LeftButton;
            previous_right_mouse_state = m.RightButton;
        }

        public override void Draw(SpriteBatch sb)
        {
            // if the cache was erased for whatever reason, reconstruct
            if (mapCaches[0, 0].IsContentLost)
            {
                buildMapCache = true;
                savePx_mod = px_mod;
                px_mod = 0;
            }

            // build the cache texture. takes a bit of time, avoid as much as possible
            if (buildMapCache)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        rebuildMapCache(sb, mapCaches[x, y], x, y);
                    }
                }

                buildMapCache = false;
                px_mod = savePx_mod;
            }

            int xPos = 0;
            int yPos = 0;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    // base position
                    xPos = x * mapCaches[x, y].Width;
                    yPos = y * mapCaches[x, y].Height;

                    // up/down/left/right translation
                    xPos += (int)-tileIndex.X * (TILE_PX_SIZE + px_mod);
                    yPos += (int)-tileIndex.Y * (TILE_PX_SIZE + px_mod);

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
                Vector2 draw_position = (unit.position.ToVector2() - tileIndex) * (TILE_PX_SIZE + px_mod);
          
                // finally draw the unit
                sb.Draw(current_textures[(game.current_tick - unit.animation_start_tick) % current_textures.Length],
                    new Rectangle((int)draw_position.X, (int)draw_position.Y, (TILE_PX_SIZE + px_mod), (TILE_PX_SIZE + px_mod)), Color.White);
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
