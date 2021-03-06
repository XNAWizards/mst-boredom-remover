﻿using System;
using System.Collections.Generic;
using System.Security.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mst_boredom_remover.engine;

namespace mst_boredom_remover
{
    class Map : UiObject
    {
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

        public readonly int width;
        public readonly int height;

        public int numberOfOceans;
        public int numberOfForests;
        public int numberOfDreadlands;
        public int numberOfDeserts;
        public int numberOfPlains;
        public int numberOfTundras;
        public int numberOfMountains;
        public int numberOfResourceNodes;

        private readonly GraphicsDevice graphicsDevice;
        private readonly Engine engine;
        private readonly int xCaches;
        private readonly int yCaches;

        private bool buildMapCache = true;
        private char[,] charmap;
        private const int tilePxSize = 18;
        private int pxMod = 0;
        private int savePxMod = 0;
        private float zoom = 1.0f;
        private const int resX = 1280;
        private const int resY = 720;
        private bool smallMode = false;
        private Vector2 tileIndex;
        private bool disableMapRegeneration = false;
        private int lastScrollValue;

        private RenderTarget2D[,] mapCaches;

        private Vector2 mouseTile;

        public Map(Vector2 startingPosition, int width, int height, ref Engine engine, GraphicsDevice graphicsDevice,
            int numberOfDreadlands, int numberOfDeserts, int numberOfPlains, int numberOfMountains, int numberOfTundras,
            int numberOfForests, int numberOfOceans, int numberOfResourceNodes)
        {
            tileIndex = startingPosition;
            this.width = width;
            this.height = height;
            this.engine = engine;
            this.graphicsDevice = graphicsDevice;

            this.numberOfDreadlands = numberOfDreadlands;
            this.numberOfDeserts = numberOfDeserts;
            this.numberOfPlains = numberOfPlains;
            this.numberOfMountains = numberOfMountains;
            this.numberOfTundras = numberOfTundras;
            this.numberOfForests = numberOfForests;
            this.numberOfOceans = numberOfOceans;
            this.numberOfResourceNodes = numberOfResourceNodes;

            // Generate map
            charmap = Generator.generate(width, height, numberOfDreadlands, numberOfDeserts, numberOfPlains, numberOfMountains, numberOfTundras, numberOfForests, numberOfOceans, numberOfResourceNodes);
            engine.map.UpdateTilesFromCharmap(charmap);
            
            // Generate map cache
            xCaches = (int) Math.Ceiling(width/214.0);
            yCaches = (int) Math.Ceiling(height/214.0);
            mapCaches = new RenderTarget2D[xCaches, yCaches];
            for (int y = 0; y < yCaches; ++y)
            {
                for (int x = 0; x < xCaches; ++x)
                {
                    mapCaches[x, y] = new RenderTarget2D(graphicsDevice, 214 * tilePxSize, 214 * tilePxSize);
                }
            }
        }

        public Vector2 ScreenToGame(Vector2 screenPosition)
        {
            return new Vector2(
                screenPosition.X / (tilePxSize + pxMod) + tileIndex.X,
                screenPosition.Y / (tilePxSize + pxMod) + tileIndex.Y);
        }

        public Vector2 GameToScreen(Vector2 tilePosition)
        {
            return new Vector2(
                (tilePosition.X - tileIndex.X) * (tilePxSize + pxMod),
                (tilePosition.Y - tileIndex.Y) * (tilePxSize + pxMod));
        }

        public bool InsideMap(Vector2 tilePosition)
        {
            return tilePosition.X >= 0 && tilePosition.Y >= 0 && tilePosition.X < width && tilePosition.Y < height;
        }

        public int GetPxSizeMod()
        {
            return tilePxSize + pxMod;
        }
        public int GetTileIndexX()
        {
            return (int)tileIndex.X;
        }
        public int GetTileIndexY()
        {
            return (int)tileIndex.Y;
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
            disableMapRegeneration = false;
            
            tileIndex += new Vector2(deltaX, deltaY);
            ForceBounds();
        }

        // returns a list of units in the bounds provided.
        public List<Unit> Select(Rectangle bounds, Player owner)
        {
            List<Unit> selectedUnits = new List<Unit>();

            var startPosition = ScreenToGame(new Vector2(bounds.Left, bounds.Top));
            var endPosition = ScreenToGame(new Vector2(bounds.Right, bounds.Bottom));
            Position startTile = new Position((int)Math.Floor(startPosition.X), (int)Math.Floor(startPosition.Y));
            Position endTile = new Position((int)Math.Ceiling(endPosition.X), (int)Math.Ceiling(endPosition.Y));

            // search only the grid. hopefully small n^2
            for (var y = startTile.y; y < endTile.y; ++y)
            {
                for (var x = startTile.x; x < endTile.x; ++x)
                {
                    Unit unit = engine.GetUnitAt(x, y);
                    if (unit != null && unit.owner.Equals(owner))
                    {
                        unit.selected = true;
                        selectedUnits.Add(unit);
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
            // find out what tile the mouse is pointing at
            string tileName = "Unkown";
            MouseState m = Mouse.GetState();
            try
            {
                tileName = engine.map.tiles[(int)mouseTile.X, (int)mouseTile.Y].tileType.name;
                tileName += " " + charmap[(int)mouseTile.X, (int)mouseTile.Y];
            }
            catch (IndexOutOfRangeException)
            {
                // Do nothing
            }

            debugText = "";
            debugText += "Mouse Position: (" + m.X + ", " + m.Y + ")\n";
            debugText += "Mouse Tile: (" + mouseTile.X + ", " + mouseTile.Y + ")\n";
            debugText += "TileIndex: (" + tileIndex.X + ", " + tileIndex.Y + ")\n";
            debugText += tileName;
            
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
            // Align viewport to integers
            tileIndex.X = (float) Math.Round(tileIndex.X);
            tileIndex.Y = (float) Math.Round(tileIndex.Y);

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
            if (pxMod < 32)
            {
                MouseState m = Mouse.GetState();
                var originalMousePosition = ScreenToGame(new Vector2(m.X, m.Y));

                pxMod += 2;
                zoom = (float)(tilePxSize + pxMod) / (float)tilePxSize;

                var newMousePosition = ScreenToGame(new Vector2(m.X, m.Y));
                tileIndex -= (newMousePosition - originalMousePosition);
                ForceBounds();
            }
        }

        private void ZoomOut()
        {
            if (pxMod > -16)
            {
                MouseState m = Mouse.GetState();
                var originalMousePosition = ScreenToGame(new Vector2(m.X, m.Y));

                pxMod -= 2;
                zoom = (float)(tilePxSize + pxMod) / (float)tilePxSize;

                var newMousePosition = ScreenToGame(new Vector2(m.X, m.Y));
                tileIndex -= (newMousePosition - originalMousePosition);
                ForceBounds();
            }
        }

        private void RebuildMapCache(SpriteBatch sb, RenderTarget2D cache, int numX, int numY)
        {
            int startX = numX * 214;
            int startY = numY * 214;
            int tileWidth = tilePxSize + pxMod;
            sb.End();
            graphicsDevice.SetRenderTarget(cache);
            sb.Begin();

            for (int x = startX; x < startX + 214 && x < width; x++)
            {
                for (int y = startY; y < startY + 214 && y < height; y++)
                {
                    Rectangle bounds = new Rectangle((x - startX) * tileWidth, (y - startY) * tileWidth, tileWidth, tileWidth);
                    
                    TileType tileType = engine.map.tiles[x, y].tileType;

                    // Translate the image to account for rotating around the top-left point
                    Vector2 origin = new Vector2(bounds.Width / 2.0f, bounds.Height / 2.0f);
                    Vector2 image = Vector2.Transform(origin, Matrix.CreateRotationZ(tileType.rotation));
                    Vector2 deltaVector = origin - image;
                    bounds.Offset((int)Math.Round(deltaVector.X), (int)Math.Round(deltaVector.Y));

                    sb.Draw(tileType.texture, bounds, null, Color.White, tileType.rotation, Vector2.Zero, SpriteEffects.None, 0);
                }
             }
                
            sb.End();
            graphicsDevice.SetRenderTarget(null);
            sb.Begin();
        }


        public Vector2 getHPBarDrawPosition(Unit unit)
        {
            Vector2 drawPosition = (unit.GetAnimatedPosition() - tileIndex) * (tilePxSize + pxMod);

            // add offset for HP bar position
            drawPosition.Y += tilePxSize + pxMod;

            return drawPosition;
        }

        public override void Update(GameTime gt)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.G) && disableMapRegeneration == false)
            {
                // generate a new map, reconstruct cache
                charmap = Generator.generate(width, height, numberOfDreadlands, numberOfDeserts, numberOfPlains, numberOfMountains, numberOfTundras, numberOfForests, numberOfOceans, numberOfResourceNodes);
                engine.map.UpdateTilesFromCharmap(charmap);
                disableMapRegeneration = true;
                buildMapCache = true;
                savePxMod = pxMod;
                pxMod = 0;
            }

            int step = 1;
            // Slow down movement speed when left control is held
            if (keyboard.IsKeyDown(Keys.LeftControl))
            {
                step = gt.TotalGameTime.Ticks % 3 == 0 ? 1 : 0;
            }
            // Move screen with WASD
            if (keyboard.IsKeyDown(Keys.W))
            {
                MapMove(0, -step);
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                MapMove(-step, 0);
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                MapMove(0, step);
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                MapMove(step, 0);
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
                for (int x = 0; x < xCaches; x++)
                {
                    for (int y = 0; y < yCaches; y++)
                    {
                        RebuildMapCache(sb, mapCaches[x, y], x, y);
                    }
                }

                buildMapCache = false;
                pxMod = savePxMod;
            }

            for (int x = 0; x < xCaches; x++)
            {
                for (int y = 0; y < yCaches; y++)
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
                int progress = engine.currentTick - unit.animationStartTick;
                // TODO: Move current_textures outside of this loop and into unit logic
                int[] indices = null;
                switch (unit.status)
                {
                    case Unit.Status.Idle:
                        indices = unit.type.idleIndices[unit.direction];
                        break;
                    case Unit.Status.Moving:
                        indices = unit.type.walkIndices[unit.direction];
                        progress = (progress * indices.Length) / unit.GetMoveCooldown(unit.previousPosition, unit.position);
                        break;
                    case Unit.Status.Attacking:
                        indices = unit.type.attackIndices[unit.direction];
                        progress = (progress * indices.Length) / unit.GetAttackCooldown();
                        break;
                    default:
                        indices = unit.type.idleIndices[unit.direction];
                        break;
                }

                // calculate screen space based on map coordinates
                // (coordinate of the unit - coordinate of the camera) * tile_pixel_size
                Vector2 drawPosition = (unit.GetAnimatedPosition() - tileIndex) * (tilePxSize + pxMod);
                Color c = Color.White;
                
                // TODO: Make color a member of Player
                if (unit.owner == engine.players[1])
                {
                    c = Color.MediumBlue;
                }
                if (unit.selected)
                {
                    c = Color.Red;
                }
                
                int frame = indices[progress % indices.Length];
                int xFrames = unit.type.texture.Width / unit.type.frameWidth;
                int tx = unit.type.frameWidth*(frame % xFrames);
                int ty = unit.type.frameHeight*(frame / xFrames);

                // finally draw the unit
                sb.Draw(unit.type.texture,
                    destinationRectangle: new Rectangle((int) drawPosition.X, (int) drawPosition.Y, tilePxSize + pxMod, tilePxSize + pxMod),
                    sourceRectangle: new Rectangle(tx, ty, unit.type.frameWidth, unit.type.frameHeight),
                    color: c);
            }

            if (debugMode)
            {
                DebugDraw(sb);
            }
            //base.Draw(sb);
        }
    }
}
