using System;
using System.Collections.Generic;
using System.Linq;
using mst_boredom_remover.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace mst_boredom_remover
{
    class Hud : UiObject
    {
        Texture2D boxSelect;
        List<Unit> selectedUnits;
        KeyboardState k;
        MouseState m;
        MouseState m2;
        Engine engine;
        List<Unit> engineUnits;     // reference to the game units list
        Vector2 startRect;
        Vector2 endRect;
        Map map;
        double timer = 0;
        bool rectangleSelect = false;
        const float HOLD_THRESH = .25f;
        Texture2D HPbar;

        // handles context displays, selected units, resource counters, 
        public Hud(ref Engine game, ref Map map, Texture2D boxSelect, Texture2D HPbar, Engine engine)
        {
            selectedUnits = new List<Unit>();
            engineUnits = game.units;

            startRect = new Vector2();
            endRect = new Vector2();

            this.map = map;
            this.boxSelect = boxSelect;
            this.HPbar = HPbar;
            this.engine = engine;
            // initialize
            m = Mouse.GetState();
            m2 = Mouse.GetState();
        }

        private void ClearOrders()
        {
            foreach (Unit u in selectedUnits)
            {
                u.orders.Clear(); // ?
            }
        }

        public override void IssueOrder(string order)
        {
            switch (order)
            {
                case "Build Town":

                    break;
                case "Build Mine":

                    break;
                case "Produce Knight":

                    break;
                case "Produce Archer":

                    break;
                case "Produce Peasant":

                    break;
                case "Attack":

                    break;
                case "Move":
                    unitGroupMove(true);
                    break;
                case "Gather":

                    break;
                case "Stop":
                    ClearOrders();
                    break;
                default:

                    break;
            }
        }

        public void unitGroupMove(bool clearOrders)
        {
            Vector2 mouseTile = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            mouseTile.X = m.X / (map.getPxSizeMod()) + map.getTileIndexX();
            mouseTile.Y = m.Y / (map.getPxSizeMod()) + map.getTileIndexY();

            Position mouseGameTilePosition = new Position((int)mouseTile.X, (int)mouseTile.Y);

            Unit clickedUnit = engine.unitGrid[mouseGameTilePosition.x, mouseGameTilePosition.y];
            var enumerator = engine.map.BreadthFirst(mouseGameTilePosition).GetEnumerator();
            enumerator.MoveNext();
            foreach (Unit unit in selectedUnits)
            {
                if (clearOrders)
                {
                    unit.orders.Clear();
                }
                if (clickedUnit == unit) // Produce units
                {
                    engine.OrderProduce(unit, engine.unitTypes[1]);
                    break;
                }
                else if (clickedUnit != null) //Clicked a different unit TODO:make it so you don't attack your buddies
                {
                    if (selectedUnits.Contains(clickedUnit)) //this check makes it so that you can produce without have the other selected units attack
                    {
                        continue;
                    }
                    engine.OrderAttack(unit, clickedUnit);
                }
                else // Move units or gather
                {
                    var resource = engine.map.tiles[enumerator.Current.x, enumerator.Current.y].tileType.resourceType;
                    if (resource != TileType.ResourceType.None)
                    {
                        engine.OrderGather(unit, enumerator.Current);
                        continue;
                    }

                    while (engine.unitGrid[enumerator.Current.x, enumerator.Current.y] != null)
                    {
                        enumerator.MoveNext();
                    }
                    engine.OrderMove(unit, enumerator.Current);
                    enumerator.MoveNext();
                }
            }

        }

        List<Unit> FindUnitsIn(Vector2 start, Vector2 end)
        {
            //Rectangle bounds = new Rectangle((int)start.X, (int)start.Y, (int)(end.X - start.X), (int)(end.Y - start.Y));
            Rectangle bounds = new Rectangle((int)Math.Min(start.X, end.X), (int)Math.Min(start.Y, end.Y), (int)Math.Abs(end.X - start.X), (int)Math.Abs(end.Y - start.Y));

            // guarantee within map bounds
            if (bounds.X < 0)
            {
                bounds.X = 0;
            }
            if (bounds.Y < 0)
            {
                bounds.Y = 0;
            }

            return map.select(bounds);
        }

        public override void ChangeFont(SpriteFont f)
        {

        }

        public override void ChangeContext(int id)
        {

        }

        public override void ToggleDebugMode()
        {
            debugMode = !debugMode;
        }

        private void DebugUpdate(GameTime gt)
        {
            debugText = "SelectedUnit.Count: " + selectedUnits.Count;
        }
        private void DebugDraw(SpriteBatch sb)
        {

        }

        public override void Update(GameTime gt)
        {
            // update states
            m = Mouse.GetState();
            k = Keyboard.GetState();

            if (m.RightButton == ButtonState.Pressed && m2.RightButton == ButtonState.Released)
            {

            }
            else if (m.RightButton == ButtonState.Pressed && m2.RightButton == ButtonState.Pressed)
            {

            }
            else if (m.RightButton == ButtonState.Released && m2.RightButton == ButtonState.Pressed)
            {
                bool clearOrders = false;
                if( k.IsKeyUp(Keys.LeftShift) && k.IsKeyUp(Keys.RightShift))
                {
                    clearOrders = true;
                }

                unitGroupMove(clearOrders);
            }
            else
            {
                // reset right button variables
            }

            // clicker first pressed
            if (m.LeftButton == ButtonState.Pressed && m2.LeftButton == ButtonState.Released)
            {
                // distribute orders
                //map.unitGroupMove(selectedUnits);
                // save the current mouse position
                startRect = new Vector2(m.X, m.Y);
                
                timer = 0;
            }
            // clicker held
            else if (m.LeftButton == ButtonState.Pressed && m2.LeftButton == ButtonState.Pressed)
            {
                timer += gt.ElapsedGameTime.TotalSeconds;
                if (timer >= HOLD_THRESH)
                {
                    if (!rectangleSelect)
                    {
                        // if held longer than half a second
                        rectangleSelect = true;
                        endRect = new Vector2(m.X, m.Y);
                    }
                    else
                    {
                        endRect = new Vector2(m.X, m.Y);
                    }
                }
                
            }
            // clicker first released
            else if (m.LeftButton == ButtonState.Released && m2.LeftButton == ButtonState.Pressed)
            {
                // clear selected units
                foreach (Unit u in selectedUnits)
                {
                    u.selected = false;
                }
                selectedUnits.Clear();
                if (rectangleSelect)
                {
                    // save the end position for the rectangle
                    endRect = new Vector2(m.X, m.Y);

                    // calculate new selected units
                    selectedUnits = FindUnitsIn(startRect, endRect);
                }
                else // not rectangle select, single tile select
                {
                    // same algorithm to select units from a 1x1 tile square
                    selectedUnits = FindUnitsIn(startRect, new Vector2(startRect.X + 1, startRect.Y + 1));
                }
            }
            else // reset, no mouse buttons are pressed
            {
                rectangleSelect = false;
                timer = 0;
                startRect = Vector2.Zero;
                endRect = Vector2.Zero;
            }

            // update old state
            m2 = m;

            if (debugMode)
            {
                DebugUpdate(gt);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            // draw unit HP bars
            foreach (Unit u in engineUnits)
            {
                // calc hp percent
                double percent = u.health / u.type.maxHealth;

                Vector2 drawPosition = map.GetDrawPosition(u);

                sb.Draw(HPbar, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, (int)(map.getPxSizeMod() * percent), 3), Color.White);
            }
            if (rectangleSelect)
            {
                // draw the select box. ideally all units contaned within or touching are selected when the click is released
                //Rectangle r = new Rectangle((int)startRect.X, (int)startRect.Y, (int)(endRect.X - startRect.X), (int)(endRect.Y - startRect.Y));
                Rectangle r = new Rectangle((int)Math.Min(startRect.X, endRect.X), (int)Math.Min(startRect.Y, endRect.Y), (int)Math.Abs(endRect.X - startRect.X), (int)Math.Abs(endRect.Y - startRect.Y));
                //sb.Draw(boxSelect, r, Color.White);
                sb.Draw(boxSelect, r, null, Color.White * .25f, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            }
            if (debugMode)
            {
                DebugDraw(sb);
            }
        }
    }
}

