using System;
using System.Collections.Generic;
using mst_boredom_remover.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mst_boredom_remover
{
    class Hud : UiObject
    {
        readonly Engine engine;
        Player currentPlayer;
        Texture2D boxSelect;
        List<Unit> selectedUnits;
        KeyboardState k;
        MouseState m;
        MouseState m2;
        String lastButtonPressed;
        Vector2 startRect;
        Vector2 endRect;
        Map map;
        double timer = 0;
        bool rectangleSelect = false;
        const float HOLD_THRESH = .25f;
        Texture2D HPbar;

        // handles context displays, selected units, resource counters, 
        public Hud(ref Engine engine, ref Map map, Texture2D boxSelect, Texture2D HPbar)
        {
            this.engine = engine;
            this.map = map;
            this.boxSelect = boxSelect;
            this.HPbar = HPbar;
            this.currentPlayer = engine.players[0];

            selectedUnits = new List<Unit>();
            startRect = new Vector2();
            endRect = new Vector2();
            
            // initialize
            m = Mouse.GetState();
            m2 = Mouse.GetState();
            lastButtonPressed = "None";
        }

        private void ClearOrders()
        {
            foreach (Unit u in selectedUnits)
            {
                u.orders.Clear();
            }
        }

        public override void IssueOrder(string order)
        {
            bool orderExecuted = false;
            switch (order)
            {
                case "Build Town":
                    lastButtonPressed = "Build Town";
                    break;
                case "Build Mine":
                    lastButtonPressed = "Build Mine";
                    break;
                case "Produce Knight":
                    foreach ( Unit u in selectedUnits )
                    {
                        if ( u.CanProduce() )
                        {
                            engine.OrderProduce(u, engine.unitTypes[0]);
                            orderExecuted = true;
                        }
                    }
                    if (orderExecuted)
                    {
                        lastButtonPressed = "None";
                    }
                    else
                    {
                        lastButtonPressed = "Produce Knight";
                    }
                    break;
                case "Produce Archer":
                    foreach (Unit u in selectedUnits)
                    {
                        if (u.CanProduce())
                        {
                            engine.OrderProduce(u, engine.unitTypes[1]);
                            orderExecuted = true;
                        }
                    }
                    if (orderExecuted)
                    {
                        lastButtonPressed = "None";
                    }
                    else
                    {
                        lastButtonPressed = "Produce Archer";
                    }
                    break;
                case "Produce Peasant":
                    foreach (Unit u in selectedUnits)
                    {
                        if (u.CanProduce())
                        {
                            engine.OrderProduce(u, engine.unitTypes[2]);
                            orderExecuted = true;
                        }
                    }
                    if (orderExecuted)
                    {
                        lastButtonPressed = "None";
                    }
                    else
                    {
                        lastButtonPressed = "Produce Peasant";
                    }
                    break;
                case "Attack":

                    lastButtonPressed = "Attack";
                    break;
                case "Move":
                    lastButtonPressed = "Move";
                    break;
                case "Gather":

                    lastButtonPressed = "Gather";
                    break;
                case "Stop":
                    lastButtonPressed = "Stop";
                    ClearOrders();
                    break;
                default:

                    break;
            }
        }

        public void UnitGroupCommand()
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keys = Keyboard.GetState();

            Vector2 mouseTile = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            mouseTile.X = m.X / (map.GetPxSizeMod()) + map.GetTileIndexX();
            mouseTile.Y = m.Y / (map.GetPxSizeMod()) + map.GetTileIndexY();

            Position mouseGameTilePosition = new Position((int)mouseTile.X, (int)mouseTile.Y);

            Unit clickedUnit = engine.GetUnitAt(mouseGameTilePosition);
            var enumerator = engine.map.BreadthFirst(mouseGameTilePosition).GetEnumerator();
            enumerator.MoveNext();
            foreach (Unit unit in selectedUnits)
            {
                if (lastButtonPressed.Equals("None"))
                {
                    if (keys.IsKeyUp(Keys.LeftShift) && keys.IsKeyUp(Keys.RightShift))
                    {
                        unit.orders.Clear();
                    }
                    if (clickedUnit == unit) // Produce units
                    {
                        if (unit.CanProduce())
                        {
                            engine.OrderProduce(unit, engine.unitTypes[1]);
                        }
                        break;
                    }
                    else if (clickedUnit != null) //Clicked a different unit TODO:make it so you don't attack your buddies
                    {
                        if (selectedUnits.Contains(clickedUnit)) //this check makes it so that you can produce without have the other selected units attack
                        {
                            continue;
                        }
                        if (unit.CanAttack())
                        {
                            engine.OrderAttack(unit, clickedUnit);
                        }
                    }
                    else // Move units or gather
                    {
                        var resource = engine.map.tiles[enumerator.Current.x, enumerator.Current.y].tileType.resourceType;
                        if (resource != TileType.ResourceType.None)
                        {
                            engine.OrderGather(unit, enumerator.Current);
                            enumerator.MoveNext();
                            continue;
                        }

                        while (engine.GetUnitAt(enumerator.Current) != null)
                        {
                            enumerator.MoveNext();
                        }
                        engine.OrderMove(unit, enumerator.Current);
                        enumerator.MoveNext();
                    }
                }
                else
                {
                    unit.orders.Clear(); 
                    switch (lastButtonPressed)
                    {
                        case "Build Town":
                            foreach (Unit u in selectedUnits)
                            {
                                if (u.CanBuild())
                                {
                                    engine.OrderProduce(u, engine.unitTypes[3], mouseGameTilePosition);
                                }
                            }
                            break;
                        case "Build Mine":
                            foreach (Unit u in selectedUnits)
                            {
                                if (u.CanBuild())
                                {
                                    engine.OrderProduce(u, engine.unitTypes[4], mouseGameTilePosition);
                                }
                            }
                            break;
                        case "Produce Knight":
                            foreach (Unit u in selectedUnits)
                            {
                                if (u.CanProduce())
                                {
                                    engine.OrderProduce(u, engine.unitTypes[0]);
                                }
                            }
                            break;
                        case "Produce Archer":
                            foreach (Unit u in selectedUnits)
                            {
                                if (u.CanProduce())
                                {
                                    engine.OrderProduce(u, engine.unitTypes[1]);
                                }
                            }
                            break;
                        case "Produce Peasant":
                            foreach (Unit u in selectedUnits)
                            {
                                if (u.CanProduce())
                                {
                                    engine.OrderProduce(u, engine.unitTypes[2]);
                                }
                            }
                            break;
                        case "Attack":
                            foreach (Unit u in selectedUnits)
                            {
                                if (u.CanProduce())
                                {
                                    engine.OrderProduce(u, engine.unitTypes[2]);
                                }
                            }
                            break;
                        case "Move":
                            foreach (Unit u in selectedUnits)
                            {
                                if (u.CanProduce())
                                {
                                    engine.OrderProduce(u, engine.unitTypes[2]);
                                }
                            }
                            break;
                        case "Gather":
                            foreach (Unit u in selectedUnits)
                            {
                                if (u.CanProduce())
                                {
                                    engine.OrderProduce(u, engine.unitTypes[2]);
                                }
                            }
                            break;
                        case "Stop":
                            foreach (Unit u in selectedUnits)
                            {
                                if (u.CanProduce())
                                {
                                    engine.OrderProduce(u, engine.unitTypes[2]);
                                }
                            }
                            ClearOrders();
                            break;
                        default:
                            
                            break;
                    } //End switch
                    lastButtonPressed = "None";
                }
            }
            if (keys.IsKeyUp(Keys.LeftControl) && keys.IsKeyUp(Keys.RightControl))
            {
                lastButtonPressed = "None";
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

            return map.Select(bounds, currentPlayer);
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

        private void ClearSelectedUnits()
        {
            foreach (Unit u in selectedUnits)
            {
                u.selected = false;
            }
            selectedUnits.Clear();
        }

        public override void Update(GameTime gt)
        {
            // update states
            m = Mouse.GetState();
            k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.NumPad1))
            {
                currentPlayer = engine.players[0];
            }
            else if (k.IsKeyDown(Keys.NumPad2))
            {
                currentPlayer = engine.players[1];
            }
            if (m.RightButton == ButtonState.Pressed && m2.RightButton == ButtonState.Released)
            {

            }
            else if (m.RightButton == ButtonState.Pressed && m2.RightButton == ButtonState.Pressed)
            {

            }
            else if (m.RightButton == ButtonState.Released && m2.RightButton == ButtonState.Pressed)
            {
                UnitGroupCommand();
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
                if (rectangleSelect)
                {
                    ClearSelectedUnits();
                    // save the end position for the rectangle
                    endRect = new Vector2(m.X, m.Y);

                    // calculate new selected units
                    selectedUnits = FindUnitsIn(startRect, endRect);
                }
                else // not rectangle select, single tile select
                {
                    if ( lastButtonPressed.Equals("None") )
                    {
                        ClearSelectedUnits();
                        // same algorithm to select units from a 1x1 tile square
                        selectedUnits = FindUnitsIn(startRect, new Vector2(startRect.X + 1, startRect.Y + 1));
                    }
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
            foreach (Unit u in engine.units)
            {
                // calc hp percent
                double percent = u.health / u.type.maxHealth;

                Vector2 drawPosition = map.getHPBarDrawPosition(u);

                sb.Draw(HPbar, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, (int)(map.GetPxSizeMod() * percent), 3), Color.White);
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

