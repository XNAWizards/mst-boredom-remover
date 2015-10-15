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
    class Map : UiObject
    {
        private Vector2 _position;
        private List<Texture2D> _tileTextures;
        private char[,] _charmap;
        GraphicsDevice _graphicsDevice;
        bool _buildMapCache = true;

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
        private int _pxMod = 0;
        private int _savePxMod = 0;
        private float _zoom = 1.0f;
        private const int resX = 1280;
        private const int resY = 720;
        private bool _smallMode = false;
        private Vector2 _tileIndex;
        private bool _gDisable = false;
        private List<string> _tileNames;
        //private string debugText = "";
        private Engine _game;
        private List<Unit> _selectedUnits = new List<Unit>();
        private int _lastScrollValue;
        
        private ButtonState _previousLeftMouseState = ButtonState.Released;
        private ButtonState _previousRightMouseState = ButtonState.Released;

        RenderTarget2D[,] _mapCaches;

        private Vector2 _mouseTile = new Vector2();

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

            this._position = position;
            this._tileTextures = tileTextures;
            //this.map = map;
            //this.texture = texture;

            _tileIndex = Vector2.Zero;

            this._charmap = Generator.Generate(width, height);

            _tileNames = new List<string>();

            _tileNames.Add("null");
            _tileNames.Add("Plains");
            _tileNames.Add("Mountain");
            _tileNames.Add("Desert");
            _tileNames.Add("Ocean");
            _tileNames.Add("Dreadland");
            _tileNames.Add("Tundra");
            _tileNames.Add("Forest");
            
            this._game = game;

            this._graphicsDevice = graphicsDevice;
            _mapCaches = new RenderTarget2D[3, 2];
            RenderTarget2D r1 = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
            RenderTarget2D r2 = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
            RenderTarget2D r3 = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
            RenderTarget2D r4 = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
            RenderTarget2D r5 = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
            RenderTarget2D r6 = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
            _mapCaches[0, 0] = r1;
            _mapCaches[1, 0] = r2;
            _mapCaches[2, 0] = r3;
            _mapCaches[0, 1] = r4;
            _mapCaches[1, 1] = r5;
            _mapCaches[2, 1] = r6;
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

        public override void ChangeContext(int id)
        {
            //base.changeContext(id);

            if (id == 2)
            {
                // small mode
                _smallMode = true;
            }
            else
            {
                // regular mode
                _smallMode = false;
            }
        }

        public void MapMove(int deltaX, int deltaY)
        {
            _gDisable = false;
            
            if (deltaX != 0)
            {
                if (_tileIndex.X + deltaX >= 0)
                {
                    if (_tileIndex.X + deltaX < width - (resX / (tilePxSize + _pxMod)))
                    {
                        _tileIndex.X += deltaX;
                    }
                }
            }
            if (deltaY != 0)
            {
                if (_tileIndex.Y + deltaY >= 0)
                {
                    // res_y/px_size = number of tiles that fit on screen
                    // height - #tiles on screen = maximum tileIndex.Y to allow
                    if (_tileIndex.Y + deltaY < height - (resY / (tilePxSize + _pxMod)))
                    {
                        _tileIndex.Y += deltaY;
                    }
                }
            }
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

            // find out what tile is at tileIndex + MousePos / tile_px_size

            Vector2 mouseIndex = new Vector2(m.X / tilePxSize, m.Y / tilePxSize);

            if (_mouseTile.X < 0 || _mouseTile.X > width)
            {
                fail = true;
            }
            if (_mouseTile.Y < 0 || _mouseTile.Y > height)
            {
                fail = true;
            }
            if (mouseIndex.Y + _tileIndex.Y < 0 || mouseIndex.Y + _tileIndex.Y > height)
            {
                fail = true;
            }
            if (fail == false)
            {
                try
                {
                    c = _charmap[(int)(_mouseTile.X), (int)(_mouseTile.Y)];
                }
                catch (Exception e)
                {
                    c = '!';
                }
                c = _charmap[(int)(mouseIndex.X + _tileIndex.X), (int)(mouseIndex.Y + _tileIndex.Y)];
            }

            debugText = "";
            debugText += "Mouse Position: (" + m.X + ", " + m.Y + ")\n";
            debugText += "Mouse Tile: (" + _mouseTile.X + ", " + _mouseTile.Y + ")\n";
            debugText += "TileIndex: (" + _tileIndex.X + ", " + _tileIndex.Y + ")\n";
            debugText += _tileNames[CharToInt(c)];
            
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
            if (_tileIndex.X < 0)
            {
                _tileIndex.X = 0;
            }
            if (_tileIndex.Y < 0)
            {
                _tileIndex.Y = 0;
            }
            // max bounds
            if (_tileIndex.X + (resX / (tilePxSize + _pxMod)) > width)
            {
                _tileIndex.X = width - (resX / (tilePxSize + _pxMod));
            }
            if (_tileIndex.Y + (resY / (tilePxSize + _pxMod)) > height)
            {
                _tileIndex.Y = height - (resY / (tilePxSize + _pxMod));
            }
        }

        private void ZoomIn()
        {
            if (_pxMod < 18)
            {
                _pxMod += 2;
                // calculate zoom factor for drawing.

                _zoom = (float)(tilePxSize + _pxMod) / (float)tilePxSize;
                
                // calculate number of tiles after zoom
                int tilesX = resX / (tilePxSize + _pxMod);
                int tilesY = resY / (tilePxSize + _pxMod);
                // new tileIndex =  mouseTile - tiles after/2
                _tileIndex.X = _mouseTile.X - tilesX / 2;
                _tileIndex.Y = _mouseTile.Y - tilesY / 2;

                ForceBounds();
            }
        }
        private void ZoomOut()
        {
            // zoom out
            if (_pxMod > -16)
            {
                _pxMod -= 2;
                
                _zoom = (float)(tilePxSize + _pxMod) / (float)tilePxSize;
                
                // calculate number of tiles after zoom
                int tilesX = resX / (tilePxSize + _pxMod);
                int tilesY = resY / (tilePxSize + _pxMod);
                // new tileIndex =  mouseTile - tiles after/2
                _tileIndex.X = _mouseTile.X - tilesX / 2;
                _tileIndex.Y = _mouseTile.Y - tilesY / 2;

                ForceBounds();
            }
        }

        private void RebuildMapCache(SpriteBatch sb, RenderTarget2D cache, int numX, int numY)
        {
            int startX = numX * 214;
            int startY = numY * 214;
            sb.End();
            _graphicsDevice.SetRenderTarget(cache);
            sb.Begin();

            for (int x = startX; x < startX + 214 && x < width; x++)
            {
                for (int y = startY; y < startY + 214 && y < height; y++)
                {
                    sb.Draw(_tileTextures[CharToInt(_charmap[x, y])], new Rectangle((x - (numX * 214)) * (tilePxSize + _pxMod), (y - (numY * 214)) * (tilePxSize + _pxMod), (tilePxSize + _pxMod), (tilePxSize + _pxMod)), Color.White);
                }
             }
                
            sb.End();
            _graphicsDevice.SetRenderTarget(null);
            sb.Begin();
        }

        public override void Update(GameTime gt)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.G) && _gDisable == false)
            {
                // generate a new map, reconstruct cache
                _charmap = Generator.Generate(width, height);
                _gDisable = true;
                _buildMapCache = true;
                _savePxMod = _pxMod;
                _pxMod = 0;
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

            _mouseTile.X = m.X / (tilePxSize + _pxMod) + _tileIndex.X;
            _mouseTile.Y = m.Y / (tilePxSize + _pxMod) + _tileIndex.Y;

            // scroll up
            if (m.ScrollWheelValue > _lastScrollValue)
            {
                ZoomIn();
            }
            // scroll down
            else if (m.ScrollWheelValue < _lastScrollValue)
            {
                ZoomOut();
            }

            _lastScrollValue = m.ScrollWheelValue;

            Position mouseGameTilePosition = new Position((int)_mouseTile.X, (int)_mouseTile.Y);
            
            // Select units
            if (m.RightButton == ButtonState.Pressed && _previousRightMouseState == ButtonState.Released)
            {
                if (_game.unitGrid[mouseGameTilePosition.x, mouseGameTilePosition.y] != null)
                {
                    Unit targetUnit = _game.unitGrid[mouseGameTilePosition.x, mouseGameTilePosition.y];
                    if (_selectedUnits.Contains(targetUnit))
                    {
                        _selectedUnits.Remove(targetUnit);
                    }
                    else
                    {
                        _selectedUnits.Add(targetUnit);
                    }
                }
            }

            
            if (m.LeftButton == ButtonState.Pressed && _previousLeftMouseState == ButtonState.Released)
            {
                var enumerator = _game.map.BreadthFirst(mouseGameTilePosition).GetEnumerator();
                enumerator.MoveNext();
                Unit clickedSpot = _game.unitGrid[mouseGameTilePosition.x, mouseGameTilePosition.y];
                foreach (Unit unit in _selectedUnits)
                {
                    if (clickedSpot == unit) // Produce units
                    {
                        _game.OrderProduce(unit, _game.unitTypes[0]);
                        break;
                    }
                    else if ( clickedSpot != null ) //Clicked a different unit TODO:make it so you don't attack your buddies
                    {
                        if ( _selectedUnits.Contains(clickedSpot) ) //this check makes it so that you can produce without have the other selected units attack
                        {
                            continue;
                        }
                        _game.OrderAttack(unit, clickedSpot);
                    }
                    else // Move units
                    {
                        while (_game.unitGrid[enumerator.Current.x, enumerator.Current.y] != null)
                        {
                            enumerator.MoveNext();
                        }
                        _game.OrderMove(unit, enumerator.Current);
                        enumerator.MoveNext();
                    }
                }
            }
            
            if (debugMode)
            {
                DebugUpdate(gt);
            }
            
            // Update mouse states
            _previousLeftMouseState = m.LeftButton;
            _previousRightMouseState = m.RightButton;
        }

        public override void Draw(SpriteBatch sb)
        {
            // if the cache was erased for whatever reason, reconstruct
            if (_mapCaches[0, 0].IsContentLost)
            {
                _buildMapCache = true;
                _savePxMod = _pxMod;
                _pxMod = 0;
            }

            // build the cache texture. takes a bit of time, avoid as much as possible
            if (_buildMapCache)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        RebuildMapCache(sb, _mapCaches[x, y], x, y);
                    }
                }

                _buildMapCache = false;
                _pxMod = _savePxMod;
            }

            int xPos = 0;
            int yPos = 0;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    // base position
                    xPos = x * _mapCaches[x, y].Width;
                    yPos = y * _mapCaches[x, y].Height;

                    // up/down/left/right translation
                    xPos += (int)-_tileIndex.X * (tilePxSize + _pxMod);
                    yPos += (int)-_tileIndex.Y * (tilePxSize + _pxMod);

                    // zoom offset
                    xPos -= x * (int)(_mapCaches[x, y].Width - ((float)_mapCaches[x, y].Width * _zoom));
                    yPos -= y * (int)(_mapCaches[x, y].Height - ((float)_mapCaches[x, y].Height * _zoom));

                    // extra offset to account for float inaccuracy, does not occur when zoomed in
                    if (_zoom < 1.0f)
                    {
                        xPos -= x;
                        yPos -= y;
                    }

                    // finally draw the cache
                    sb.Draw(_mapCaches[x, y], new Vector2(xPos, yPos), null, Color.White, 0f, Vector2.Zero, _zoom, SpriteEffects.None, 1f);
                }
            }

            // Draw units
            // TODO: Only do this for units on screen, probably with the help of a quad-tree
            foreach (Unit unit in _game.units)
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
                Vector2 drawPosition = (unit.position.ToVector2() - _tileIndex) * (tilePxSize + _pxMod);
          
                // finally draw the unit
                sb.Draw(currentTextures[(_game.currentTick - unit.animationStartTick) % currentTextures.Length],
                    new Rectangle((int)drawPosition.X, (int)drawPosition.Y, (tilePxSize + _pxMod), (tilePxSize + _pxMod)), Color.White);
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
