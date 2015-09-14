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
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int NUM_PASSES = 5; // number of world generation passes
        GraphicsDeviceManager graphics; // graphics device
        SpriteBatch spriteBatch; // spritebatch for 2d texture drawing
        World world; // the world
        List<Button> mainMenuButtons = new List<Button>(); // list of main menu buttons
        List<Button> worldSelectButtons = new List<Button>(); // list of world select buttons
        List<Button> playButtons = new List<Button>(); // list of play screen buttons
        bool hasChecked = false; // need to scan for worlds?
        Texture2D txCreareTitle; // title texture
        Texture2D txDeleteAlert; // delete alert texture
        bool delete = false; // is delete toggled on?
        Player player; // player storage
        Area area; // area storage
        Debug debug; // debug display storage
        double debugTimer = 0; // delay in debug toggle
        MouseState oldState; // previous mouse state
        MouseState currentState; // current mouse state
        UI ui; // user's interface
        Button btnSaveAndExit; // save and exit button
        double digTimer = 0;
        int worldToLoad = 0;
        int selectedInvSlot = -1;
        int newInvSlot = -1;
        Crafting crafting;
        Button btnBookCreate;
        Button btnBookCreate2;
        Button btnBookExit;
        Button btnArrowLeft;
        Button btnArrowRight;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this); // gets the graphics device
            Content.RootDirectory = "Content"; // resources are located in Content directory
            graphics.PreferredBackBufferHeight = 600; // window width 600
            graphics.PreferredBackBufferWidth = 800; // window height 800
            IsMouseVisible = true; // mouse is visible
            Window.Title = "Creare"; // window title is "Creare"
        }

        
        protected override void Initialize()
        {
            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            #region Global Items
            // creates a new sprite drawer
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Global.font = Content.Load<SpriteFont>("UI\\Arial"); // loads font for text
            Global.bigFont = Content.Load<SpriteFont>("UI\\Arial Big");
            debug = new Debug(); // creates a new debug object
            Global.grid = Content.Load<Texture2D>("UI\\Grid"); // saves the grid texture into global
            #endregion
            #region Main Menu Items
            txCreareTitle = Content.Load<Texture2D>("UI\\CreareTitle"); // texture for the main title picutre
            // texture and hover textures for buttons used on the main menu
            Texture2D txBtnNew = Content.Load<Texture2D>("Temp Buttons\\BtnNew");
            Texture2D txBtnNewHover = Content.Load<Texture2D>("Temp Buttons\\BtnNewHover");
            Texture2D txBtnLoad = Content.Load<Texture2D>("Temp Buttons\\BtnLoad");
            Texture2D txBtnLoadHover = Content.Load<Texture2D>("Temp Buttons\\BtnLoadHover");
            Texture2D txBtnExit = Content.Load<Texture2D>("Temp Buttons\\BtnExit");
            Texture2D txBtnExitHover = Content.Load<Texture2D>("Temp Buttons\\BtnExitHover");
            // create the 3 menu buttons and position them
            Button btnNew = new Button(txBtnNew, txBtnNewHover, txBtnNew, new Vector2(350, 400));
            Button btnLoad = new Button(txBtnLoad, txBtnLoadHover, txBtnLoad, new Vector2(350, 450));
            Button btnExit = new Button(txBtnExit, txBtnExitHover, txBtnExit, new Vector2(350, 500));
            // add the button event for when it is clicked
            btnNew.OnPress += new EventHandler(btnNew_OnPress);
            btnLoad.OnPress += new EventHandler(btnLoad_OnPress);
            btnExit.OnPress += new EventHandler(btnExit_OnPress);
            // add the buttons to a list of main menu buttons
            mainMenuButtons.Add(btnNew);
            mainMenuButtons.Add(btnLoad);
            mainMenuButtons.Add(btnExit);
            #endregion
            #region World Select Items

            txDeleteAlert = Content.Load<Texture2D>("UI\\Delete");

            Texture2D txBtnWorld1 = Content.Load<Texture2D>("Temp Buttons\\BtnWorld1");
            Texture2D txBtnWorld1Hover = Content.Load<Texture2D>("Temp Buttons\\BtnWorld1Hover");
            Texture2D txBtnWorld2 = Content.Load<Texture2D>("Temp Buttons\\BtnWorld2");
            Texture2D txBtnWorld2Hover = Content.Load<Texture2D>("Temp Buttons\\BtnWorld2Hover");
            Texture2D txBtnWorld3 = Content.Load<Texture2D>("Temp Buttons\\BtnWorld3");
            Texture2D txBtnWorld3Hover = Content.Load<Texture2D>("Temp Buttons\\BtnWorld3Hover");
            Texture2D txBtnWorld4 = Content.Load<Texture2D>("Temp Buttons\\BtnWorld4");
            Texture2D txBtnWorld4Hover = Content.Load<Texture2D>("Temp Buttons\\BtnWorld4Hover");
            Texture2D txBtnWorld5 = Content.Load<Texture2D>("Temp Buttons\\BtnWorld5");
            Texture2D txBtnWorld5Hover = Content.Load<Texture2D>("Temp Buttons\\BtnWorld5Hover");

            Texture2D txBtnBack = Content.Load<Texture2D>("Temp Buttons\\BtnBack");
            Texture2D txBtnBackHover = Content.Load<Texture2D>("Temp Buttons\\BtnBackHover");
            Texture2D txBtnDelete = Content.Load<Texture2D>("Temp Buttons\\BtnDelete");
            Texture2D txBtnDeleteHover = Content.Load<Texture2D>("Temp Buttons\\BtnDeleteHover");
            Texture2D txBtnGO = Content.Load<Texture2D>("Temp Buttons\\BtnGO");
            Texture2D txBtnGOHover = Content.Load<Texture2D>("Temp Buttons\\BtnGOHover");


            Button btnWorld1 = new Button(txBtnWorld1, txBtnWorld1Hover, txBtnWorld1, new Vector2(130, 300));
            Button btnWorld2 = new Button(txBtnWorld2, txBtnWorld2Hover, txBtnWorld2, new Vector2(240, 300));
            Button btnWorld3 = new Button(txBtnWorld3, txBtnWorld3Hover, txBtnWorld3, new Vector2(350, 300));
            Button btnWorld4 = new Button(txBtnWorld4, txBtnWorld4Hover, txBtnWorld4, new Vector2(460, 300));
            Button btnWorld5 = new Button(txBtnWorld5, txBtnWorld5Hover, txBtnWorld5, new Vector2(570, 300));

            Button btnDelete = new Button(txBtnDelete, txBtnDeleteHover, txBtnDelete, new Vector2(240, 500));
            Button btnBack = new Button(txBtnBack, txBtnBackHover, txBtnBack, new Vector2(460, 500));
            Button btnGO = new Button(txBtnGO, txBtnGOHover, txBtnGO, new Vector2(350, 500));


            btnWorld1.OnPress += new EventHandler(btnWorld1_OnPress);
            btnWorld2.OnPress += new EventHandler(btnWorld2_OnPress);
            btnWorld3.OnPress += new EventHandler(btnWorld3_OnPress);
            btnWorld4.OnPress += new EventHandler(btnWorld4_OnPress);
            btnWorld5.OnPress += new EventHandler(btnWorld5_OnPress);

            btnBack.OnPress += new EventHandler(btnBack_OnPress);
            btnDelete.OnPress += new EventHandler(btnDelete_OnPress);
            btnGO.OnPress += new EventHandler(btnGO_OnPress);

            worldSelectButtons.Add(btnWorld1);
            worldSelectButtons.Add(btnWorld2);
            worldSelectButtons.Add(btnWorld3);
            worldSelectButtons.Add(btnWorld4);
            worldSelectButtons.Add(btnWorld5);
            worldSelectButtons.Add(btnBack);
            worldSelectButtons.Add(btnDelete);
            worldSelectButtons.Add(btnGO);
            #endregion            
            #region Play Items
            // sequence of textures to play when the player is walking on the ground
            Texture2D[] txPlayer = new Texture2D[6]
            {
                Content.Load<Texture2D>("Player\\Image10000"),
                Content.Load<Texture2D>("Player\\Image10001"),
                Content.Load<Texture2D>("Player\\Image10002"),
                Content.Load<Texture2D>("Player\\Image10003"),
                Content.Load<Texture2D>("Player\\Image10004"),
                Content.Load<Texture2D>("Player\\Image10005"),
            };
            Texture2D blank = Content.Load<Texture2D>("Terrain\\Void"); // blank texture for display if the world is null or out of boudns
            Texture2D PlayerFall = Content.Load<Texture2D>("Player\\player falling"); // playertexture when in the air

            // sequence of textures to play when the player digs a block
            List<Texture2D> digTextures = new List<Texture2D>()
            {
                Content.Load<Texture2D>("UI\\Dig1"),
                Content.Load<Texture2D>("UI\\Dig2"),
                Content.Load<Texture2D>("UI\\Dig3"),
                Content.Load<Texture2D>("UI\\Dig4"),
            };
            area = new Area(blank, digTextures); // stores the indexes and information related to movement and block modification
            // array of textures for the player's health status
            Texture2D[] healthPotion = new Texture2D[3]
            {
                Content.Load<Texture2D>("UI\\HealthPotion Empty"),
                Content.Load<Texture2D>("UI\\HealthPotion Half"),
                Content.Load<Texture2D>("UI\\HealthPotion Full")
            };
            // overlay for the crafting book
            Texture2D craftingBook = Content.Load<Texture2D>("UI\\Book");
            // overlay for the inventory
            Texture2D inventoryBackground = Content.Load<Texture2D>("UI\\Invitory Screen");
            

            Texture2D miniBook = Content.Load<Texture2D>("UI\\MiniBook"); // texture of the book
            Texture2D miniBookHover = Content.Load<Texture2D>("UI\\MiniBookHover"); // texture of the book when you over over it
            Texture2D miniInv = Content.Load<Texture2D>("UI\\MiniInv"); // texture for inventory when it's not on the overlay mode
            Texture2D txBtnSaveAndExit = Content.Load<Texture2D>("UI\\BtnSaveAndExit");
            Texture2D btnCreate = Content.Load<Texture2D>("Temp Buttons\\Create Button"); // texture of the book when you over over it
            Texture2D btnCreate2 = Content.Load<Texture2D>("Temp Buttons\\Create Button"); // texture of the book when you over over it
            Texture2D txBtnBookExit = Content.Load<Texture2D>("Temp Buttons\\BookExit");
            Texture2D txBtnBookExitHover = Content.Load<Texture2D>("Temp Buttons\\BookExitHover");
            Texture2D btnCreateHover = Content.Load<Texture2D>("Temp Buttons\\Create Button Hover"); // texture for inventory when it's not on the overlay mode
            Texture2D txBtnArrowLeft = Content.Load<Texture2D>("UI\\BookArrow");
            Texture2D txBtnArrowRight = Content.Load<Texture2D>("UI\\BookArrowRight");

            Button btnBook = new Button(miniBook, miniBookHover, miniBook, new Vector2(135, 5)); // crafting book button
            btnBookCreate = new Button(btnCreate,btnCreateHover,btnCreate, new Vector2(45,515));
            btnBookCreate2 = new Button(btnCreate2, btnCreateHover, btnCreate, new Vector2(470, 515));
            btnBookExit = new Button(txBtnBookExit, txBtnBookExitHover, txBtnBookExit, new Vector2(765, 10));
            Button btnMiniInv = new Button(miniInv, miniInv, miniInv, new Vector2(0, 5)); // inventory button
            btnSaveAndExit = new Button(txBtnSaveAndExit, txBtnSaveAndExit, txBtnSaveAndExit, new Vector2(675, 550));
            btnArrowLeft = new Button(txBtnArrowLeft,txBtnArrowLeft,txBtnArrowLeft,new Vector2(20,275));
            btnArrowRight = new Button(txBtnArrowRight,txBtnArrowRight,txBtnArrowRight,new Vector2(675,275));

            btnBook.OnPress += new EventHandler(btnBook_OnPress); // event handlers for buttons
            btnMiniInv.OnPress += new EventHandler(btnMiniInv_OnPress);
            btnSaveAndExit.OnPress +=new EventHandler(btnSaveAndExit_OnPress);
            btnBookCreate.OnPress += new EventHandler(btnBookCreate_OnPress); // event handlers for buttons
            btnBookCreate2.OnPress += new EventHandler(btnBookCreate2_OnPress); // event handlers for buttons
            btnBookExit.OnPress += new EventHandler(btnBookExit_OnPress);
            btnArrowLeft.OnPress +=new EventHandler(btnArrowLeft_OnPress);
            btnArrowRight.OnPress +=new EventHandler(btnArrowRight_OnPress);
            playButtons.Add(btnBook); // added to the list of play screen buttons
            playButtons.Add(btnMiniInv); // added to the list of play screen buttons
            //playButtons.Add(btnSaveAndExit); // not to be added to play nuttons because it has special features

            Texture2D verticalGrass = Content.Load<Texture2D>("Terrain\\HorsGrass"); // horizontal grass
            Texture2D horizontalGrass = Content.Load<Texture2D>("Terrain\\Grass"); // vertical grass

            // add the loaded textures to the list of grass textures
            area.AddGrassVariety(verticalGrass);
            area.AddGrassVariety(horizontalGrass);

            Texture2D sky = Content.Load<Texture2D>("Terrain\\Sky"); // blue sky
            Texture2D dirt1 = Content.Load<Texture2D>("Terrain\\Dirt1"); // dirt varieties
            Texture2D dirt2 = Content.Load<Texture2D>("Terrain\\Dirt2");
            Texture2D dirt3 = Content.Load<Texture2D>("Terrain\\Dirt3");
            Texture2D stone1 = Content.Load<Texture2D>("Terrain\\Stone1"); // stone varieties
            Texture2D stone2 = Content.Load<Texture2D>("Terrain\\Stone2");
            Texture2D stone3 = Content.Load<Texture2D>("Terrain\\Stone3");
            Texture2D CopperOre = Content.Load<Texture2D>("Terrain\\CopperOre"); // copper ore
            Texture2D IronOre = Content.Load<Texture2D>("Terrain\\IronOre"); // iron ore
            Texture2D SilverOre = Content.Load<Texture2D>("Terrain\\SilverOre"); // silver ore
            Texture2D GoldOre = Content.Load<Texture2D>("Terrain\\GoldOre"); // gold ore
            Texture2D Trunk = Content.Load<Texture2D>("Terrain\\Trunk"); // tree trunk wood
            Texture2D Leaves = Content.Load<Texture2D>("Terrain\\Leaves"); // leaves
            // add loaded textures to the list saved in area
            area.AddTexture(sky); // 0
            area.AddTexture(dirt1); // 1
            area.AddTexture(dirt2); // 2
            area.AddTexture(dirt3); // 3
            area.AddTexture(stone1); // 4
            area.AddTexture(stone2); // 5
            area.AddTexture(stone3); // 6
            area.AddTexture(CopperOre); // 7
            area.AddTexture(IronOre); // 8
            area.AddTexture(SilverOre); //9
            area.AddTexture(GoldOre); //10
            area.AddTexture(Trunk); //11
            area.AddTexture(Leaves); //12

            Texture2D CaveBackGround = Content.Load<Texture2D>("Terrain\\CaveBackground");
            area.AddBackground(sky); // 0
            area.AddBackground(CaveBackGround); // 1

            // player equipment
            List<Texture2D> equipment = new List<Texture2D>()
            {
                Content.Load<Texture2D>("Equipment\\Copper Pickaxe"), // 0
                Content.Load<Texture2D>("Equipment\\Copper Axe"), // 1
                Content.Load<Texture2D>("Equipment\\Iron Pickaxe"), // 2
                Content.Load<Texture2D>("Equipment\\Iron Axe"), // 3
                Content.Load<Texture2D>("Equipment\\Steel Pickaxe"), // 4
                Content.Load<Texture2D>("Equipment\\Steel Axe"), // 5
                Content.Load<Texture2D>("Equipment\\Silver Pickaxe"), // 6
                Content.Load<Texture2D>("Equipment\\Silver Axe"), // 7
                Content.Load<Texture2D>("Equipment\\Gold Pickaxe"), // 8
                Content.Load<Texture2D>("Equipment\\Gold Axe"), // 9
            };
            // stores the information used to display and utilize the user interface when playing
            ui = new UI(healthPotion, inventoryBackground, equipment);
            player = new Player(txPlayer, PlayerFall, equipment);

            List<Texture2D> items = new List<Texture2D>();

            items.Add(Content.Load<Texture2D>("UI\\Items\\BTDirt")); // 0 not used
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTDirt")); // 1
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTDirt")); // 2 not used
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTDirt")); // 3 not used
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTStone")); // 4
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTStone")); // 5 not used
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTStone")); // 6 not used
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTCopper")); // 7
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTIron")); // 8
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTSilver")); // 9
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTGold")); // 10
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTWood")); // 11
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTSteel")); // 12

            List<Texture2D> itemsGray = new List<Texture2D>();

            items.Add(Content.Load<Texture2D>("UI\\Items\\BTDirt")); // 0 not used
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTDirt")); // 1
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTDirt")); // 2 not used
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTDirt")); // 3 not used
            items.Add(Content.Load<Texture2D>("UI\\Items Gray\\BTStone-Grayed")); // 4
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTStone")); // 5 not used
            items.Add(Content.Load<Texture2D>("UI\\Items\\BTStone")); // 6 not used
            items.Add(Content.Load<Texture2D>("UI\\Items Gray\\BTCopperGray")); // 7
            items.Add(Content.Load<Texture2D>("UI\\Items Gray\\BTIronGray")); // 8
            items.Add(Content.Load<Texture2D>("UI\\Items Gray\\BTSilver Gray")); // 9
            items.Add(Content.Load<Texture2D>("UI\\Items Gray\\BTGold Gray")); // 10
            items.Add(Content.Load<Texture2D>("UI\\Items Gray\\BTSteel Gray")); // 11

            // add items to the user interface
            ui.AddItemTextures(items);
            // add grayed items to the user interface
            ui.AddItemGrayTextures(itemsGray);
            #endregion
            // stores the world creation, saving, and loading systems
            world = new World(player, area);
            // list of selected slot textures based on which slot is selected
            List<Texture2D> miniInvSlots = new List<Texture2D>()
            {
                Content.Load<Texture2D>("MiniInvTop"),
                Content.Load<Texture2D>("MiniInvRight"),
                Content.Load<Texture2D>("MiniInvBot"),
                Content.Load<Texture2D>("MiniInvLeftt"),
            };
            // send the array to the player
            player.AddMiniInv(miniInvSlots);

            crafting = new Crafting(craftingBook, items, itemsGray, equipment);
        }

        
        protected override void UnloadContent()
        {
            
        }


        protected override void Update(GameTime gameTime)
        {
            debugTimer += gameTime.ElapsedGameTime.TotalSeconds; // delay for debug display toggle
            Global.timer += gameTime.ElapsedGameTime.TotalSeconds; // 1 second delay in between World Select button checks
            if (Global.debugMode)
            {
                debug.Update(gameTime, area, player);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            switch (Global.screen)
            {
                case -1: // during world generation
                    #region World Generation
                    world.GenerateWorld();
                    #endregion
                    break;
                case 0: // main menu
                    #region Main Menu
                    foreach (Button button in mainMenuButtons)
                    {
                        button.Update(gameTime);
                    }
                    #endregion
                    break;
                case 1: // world select
                    
                    #region World Select
                    if (worldToLoad != 0)
                    {
                        world.Update(player, area);
                    }
                    foreach (Button button in worldSelectButtons)
                    {
                        button.Update(gameTime);
                    }
                    #endregion
                    break;
                case 2: // play
                    #region Play
                    foreach (Button button in playButtons)
                    {
                        button.Update(gameTime);
                    }
                    world.Update(player, area);
                    btnSaveAndExit.Update(gameTime);
                    ui.Update(gameTime, player);
                    KeyboardInput();
                    MouseInput(gameTime);
                    area.Update(gameTime, player);
                    player.Update(gameTime, area, playButtons[1]);
                    if (Global.displayBook)
                    {
                        crafting.Update(gameTime, player);
                        btnBookCreate.Update(gameTime);
                        btnBookCreate2.Update(gameTime);
                        btnBookExit.Update(gameTime);
                        btnArrowLeft.Update(gameTime);
                        btnArrowRight.Update(gameTime);
                    }
                    #endregion
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);
            spriteBatch.Begin();
            
            switch (Global.screen)
            {
                case -1: // during world generation
                    #region World Generation
                    if (Global.timer > 1)
                    {
                        hasChecked = false; // needs to scan for worlds
                        Global.timer = 0; // reset timer
                    }
                    world.Draw(spriteBatch);
                    #endregion
                    break;
                case 0: // main menu
                    #region Main Menu
                    
                    spriteBatch.Draw(txCreareTitle, new Vector2(138, 50), Color.White);
                    foreach (Button button in mainMenuButtons)
                    {
                        button.Draw(spriteBatch);
                    }
                    #endregion
                    break;
                case 1: // world select
                    #region World Select
                    if (worldToLoad != 0)
                    {
                        world.Draw(spriteBatch);
                    }
                    if (!hasChecked) // if it needs to scan for worlds
                    {
                        bool[] worlds = Global.CheckWorlds();

                        for (int x = 0; x < worlds.Length; x++)
                        {
                            if (worlds[x])
                            {
                                worldSelectButtons[x].visible = true;
                            }
                            else
                            {
                                worldSelectButtons[x].visible = false;
                            }
                        }
                        hasChecked = true;
                    }
                    foreach (Button button in worldSelectButtons)
                    {
                        button.Draw(spriteBatch);
                    }
                    if (delete)
                    {
                        spriteBatch.Draw(txDeleteAlert, new Vector2(275, 460), Color.White);
                    }
                    #endregion
                    break;
                case 2: // play
                    #region Play
                    area.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    foreach (Button button in playButtons)
                    {
                        button.Draw(spriteBatch, 1);
                    }
                    btnSaveAndExit.Draw(spriteBatch);
                    ui.Draw(spriteBatch, player);
                    if (Global.displayBook)
                    {
                        crafting.Draw(spriteBatch);
                        btnBookCreate.Draw(spriteBatch);
                        btnBookCreate2.Draw(spriteBatch);
                        btnBookExit.Draw(spriteBatch, 2);
                        btnArrowLeft.Draw(spriteBatch, 2);
                        btnArrowRight.Draw(spriteBatch, 2);
                    }
                    #endregion
                    break;
            }
            if (Global.debugMode)
            {
                debug.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        // get all game related keyboard input
        public void KeyboardInput()
        {
            KeyboardState keyboard = Keyboard.GetState();

            // non-toggle/constant press buttons
            // to be removed
            if (keyboard.IsKeyDown(Keys.LeftShift))
            {
                player.movementSpeed = 1000f;
            }
            else if (keyboard.IsKeyDown(Keys.RightShift))
            {
                player.movementSpeed = 100000f;
            }
            else
            {
                player.movementSpeed = 150f;
            }
            // no longer to be removed
            if (keyboard.IsKeyDown(Keys.A))
            {
                player.flip = false;
                player.PlayingAnimation = true;
                if (player.canMoveLeft)
                {                    
                    area.smallDifference.X += (float)(player.movementSpeed / Global.ScreenWidth);
                }
            }
            else if (keyboard.IsKeyDown(Keys.D))
            {
                player.flip = true;
                player.PlayingAnimation = true;
                if (player.canMoveRight)
                {
                    area.smallDifference.X -= (float)(player.movementSpeed / Global.ScreenWidth);
                }
            }
            // to be removed    
            else if (keyboard.IsKeyDown(Keys.W))
            {
                if (Global.debugMode)
                {
                    player.PlayingAnimation = true;
                    area.smallDifference.Y += (float)(player.movementSpeed / Global.ScreenWidth);
                }
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                if (Global.debugMode)
                {
                    player.PlayingAnimation = true;
                    area.smallDifference.Y -= (float)(player.movementSpeed / Global.ScreenWidth);
                }
            }
            // not to be removed
            else
            {
                player.PlayingAnimation = false;
            }

            Keys[] keysPressed;

            keysPressed = keyboard.GetPressedKeys();
            // single press and toggle buttons
            for (int x = 0; x < keysPressed.Length; x++)
            {
                switch (keysPressed[x])
                {
                    case Keys.D1:
                        player.selectedInvSlot = 0;
                        break;
                    case Keys.D2:
                        player.selectedInvSlot = 1;
                        break;
                    case Keys.D3:
                        player.selectedInvSlot = 2;
                        break;
                    case Keys.D4:
                        player.selectedInvSlot = 3;
                        break;
                    case Keys.Space:
                        if (!player.jumping && area.colliding)
                        {
                            player.jumping = true;
                            player.jumpTimer = 1.5;
                        }
                        break;
                    case Keys.F3:
                        if (debugTimer > .3)
                        {
                            if (Global.debugMode)
                            {
                                Global.debugMode = false;
                            }
                            else
                            {
                                Global.debugMode = true;
                            }
                            debugTimer = 0;
                        }
                        break;
                }
            }
        }
        public void MouseInput(GameTime gameTime)
        {
            oldState = currentState;
            currentState = Mouse.GetState();

            if (Global.displayInv)
            {
                if (currentState.LeftButton == ButtonState.Pressed && Global.hover == false)
                {
                    //get selected inv slot
                    selectedInvSlot = ui.GetSlot(currentState);
                    if (selectedInvSlot != -1)
                    {
                        Global.hover = true;
                        int textureID = player.GetSlotTexture(selectedInvSlot);
                        ui.SetHover(textureID);
                    }
                }
                else if (currentState.LeftButton == ButtonState.Released && Global.hover == true)
                {
                    newInvSlot = ui.GetSlot(currentState);
                    if (newInvSlot != -1)
                    {
                        player.MoveSlots(selectedInvSlot, newInvSlot);
                        Global.hover = false;
                    }
                    else
                    {
                        player.MoveSlots(selectedInvSlot, selectedInvSlot);
                        Global.hover = false;
                    }
                }
            }
            else
            {
                if (currentState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Pressed)
                {
                    // if left mouse button is pressed and released
                    Global.digging = true;
                    area.BlockModifier("Delete", player, gameTime);
                }
                else
                {
                    area.ResetDig();
                }
                if (currentState.RightButton == ButtonState.Released && oldState.RightButton == ButtonState.Pressed)
                {
                    digTimer = 0;
                    // if left mouse button is pressed and released
                    area.BlockModifier("Add", player, gameTime);
                }
            }
        }
        public void Reset()
        {
            player.Reset();
            area.Reset();
        }
        // button events
        public void btnNew_OnPress(object sender, EventArgs e)
        {
            Global.screen = -1;
        }
        public void btnLoad_OnPress(object sender, EventArgs e)
        {
            Global.screen = 1;
            oldState = Mouse.GetState();
        }
        public void btnExit_OnPress(object sender, EventArgs e)
        {
            this.Exit();
        }
        public void btnWorld1_OnPress(object sender, EventArgs e)
        {
            if (delete)
            {
                File.Delete(Global.saveLoc + "World1.wld");
                hasChecked = false;
            }
            else
            {
                worldToLoad = 2;
                world.Load(1);
            }
        }
        public void btnWorld2_OnPress(object sender, EventArgs e)
        {
            if (delete)
            {
                File.Delete(Global.saveLoc + "World2.wld");
                hasChecked = false;
            }
            else
            {
                worldToLoad = 2;
                world.Load(2);
            }
        }
        public void btnWorld3_OnPress(object sender, EventArgs e)
        {
            if (delete)
            {
                File.Delete(Global.saveLoc + "World3.wld");
                hasChecked = false;
            }
            else
            {
                worldToLoad = 2;
                world.Load(3);
            }
        }
        public void btnWorld4_OnPress(object sender, EventArgs e)
        {
            if (delete)
            {
                File.Delete(Global.saveLoc + "World4.wld");
                hasChecked = false;
            }
            else
            {
                worldToLoad = 2;
                world.Load(4);
            }
        }
        public void btnWorld5_OnPress(object sender, EventArgs e)
        {
            if (delete)
            {
                File.Delete(Global.saveLoc + "World5.wld");
                hasChecked = false;
            }
            else
            {
                worldToLoad = 2;
                world.Load(5);
            }
            
        }
        public void btnBack_OnPress(object sender, EventArgs e)
        {
            if (delete)
            {
                delete = false;
            }
            Global.screen = 0;
            hasChecked = false;
        }
        public void btnDelete_OnPress(object sender, EventArgs e)
        {
            if (delete)
            {
                delete = false;
            }
            else
            {
                delete = true;
            }
        }
        public void btnGO_OnPress(object sender, EventArgs e)
        {
            //world.Load(worldToLoad);
            if (worldToLoad != 0)
            {
                Global.screen = 2;
            }
        }
        public void btnBook_OnPress(object sender, EventArgs e)
        {
            // toggles display book
            if (!Global.displayBook && !Global.displayInv)
            {
                Global.displayBook = true;
            }
            else
            {
                Global.displayBook = false;
            }
        }
        public void btnMiniInv_OnPress(object sender, EventArgs e)
        {
            // toggles display inv
            if (!Global.displayInv && !Global.displayBook)
            {
                Global.displayInv = true;
            }
            else
            {
                Global.displayInv = false;
            }
        }
        public void btnSaveAndExit_OnPress(object sender, EventArgs e)
        {
            world.Save(Global.usingWorld, 1);
            worldToLoad = 0;
            Global.screen = 0;
            Reset();
        }
        public void btnBookCreate_OnPress(object sender, EventArgs e)
        {
            crafting.CreateSlot1(player);
        }
        public void btnBookCreate2_OnPress(object sender, EventArgs e)
        {
            crafting.CreateSlot2(player);
        }
        public void btnBookExit_OnPress(object sender, EventArgs e)
        {
            Global.displayBook = false;
        }
        public void btnArrowLeft_OnPress(object sender, EventArgs e)
        {
            if (crafting.page != 0)
            {
                crafting.page -= 1;
            }
        }
        public void btnArrowRight_OnPress(object sender, EventArgs e)
        {
            if (crafting.page < 6)
            {
                crafting.page += 1;
            }
        }
    }
}
