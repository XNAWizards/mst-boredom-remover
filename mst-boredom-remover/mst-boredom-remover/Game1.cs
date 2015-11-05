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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        // TODO: Remove this
        private SpriteFont debugFont;

        private Engine engine;
        
        public const int width = 1280 / 2;
        public const int height = 720 / 2;

        List<UiObject> userInterface;
        public enum MenuScreen
        {
            Main,           // 0 main menu screen
            InGame,         // 1 in game screen
            NewGame,        // 2 New Game options screen
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            engine = new Engine(width, height);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            IsMouseVisible = true;
            //graphics.ToggleFullScreen(); // activate full screen mode. toggle with alt + enter
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            debugFont = Content.Load<SpriteFont>("debug_font");
            SpriteFont bigFont = Content.Load<SpriteFont>("Arial Big");

            Texture2D buttonTexture = Content.Load<Texture2D>("ButtonNormal");
            Texture2D buttonHover = Content.Load<Texture2D>("ButtonHover");
            Texture2D buttonActive = Content.Load<Texture2D>("ButtonActive");

            // UIObjects
            #region Buttons
            // New Game Button
            Button newButton = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(150,117), 1.0f, "New", bigFont);
            newButton.OnPress += newButton_OnPress;
            newButton.Clicked += newButton_Clicked;

            // Load Game Button
            Button loadButton = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(150, 217), 1.0f, "Load", bigFont);
            loadButton.OnPress += loadButton_OnPress;
            loadButton.Clicked += loadButton_Clicked;
            // Settings/Options Button

            // main menu Exit Button
            Button mmExitButton = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(150, 317), 1.0f, "Exit", bigFont);
            mmExitButton.OnPress += mmExitButton_OnPress;
            mmExitButton.Clicked += mmExitButton_Clicked;

            // Back Button for ingame screen
            Button backButton = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(1250, 27), .5f, "Back", bigFont);
            backButton.OnPress += backButton_OnPress;
            backButton.Clicked += backButton_Clicked;

            // Go Button for New Game screen
            Button goButton = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(600, 500), 1.0f, "Go", bigFont);
            goButton.OnPress += goButton_OnPress;
            goButton.Clicked += goButton_Clicked;

            // back button for new game
            Button ngbackButton = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(20, 675), .5f, "Back", bigFont);
            ngbackButton.OnPress += ngbackButton_OnPress;
            ngbackButton.Clicked += ngbackButton_Clicked;

            #endregion
            #region TextDisplays
            // testTextDisplay
            Texture2D blackTextBackground = Content.Load<Texture2D>("BlackTextBackground");
            SpriteFont font = Content.Load<SpriteFont>("Arial");
            TextDisplay testTextDisplay = new TextDisplay(blackTextBackground, new Vector2(300, 300), font, Color.Green);
            #endregion
            #region TextInputs
            Texture2D textInBackground = Content.Load<Texture2D>("TextInBackground");
            Texture2D textInBackgroundActive = Content.Load<Texture2D>("TextInBackgroundActive");
            
            TextInput testTextInput = new TextInput(textInBackground, textInBackgroundActive, new Vector2(100, 50), font);
            //testTextInput.newInput += new EventHandler(testTextInput_newInput);
            #endregion
            
            #region Menus
            Texture2D mainBackground = Content.Load<Texture2D>("MainBackground");

            List<UiObject> mainControls = new List<UiObject>();
            mainControls.Add(newButton);
            mainControls.Add(loadButton);
            mainControls.Add(mmExitButton);
            mainControls.Add(testTextDisplay);

            Menu mainMenu = new Menu(mainBackground, new Vector2(0, 0), mainControls, Color.White, 0);

            Texture2D gameBackground = Content.Load<Texture2D>("gameBackground");

            List<UiObject> gameControls = new List<UiObject>();
            gameControls.Add(backButton);

            Menu gameMenu = new Menu(gameBackground, new Vector2(0, 0), gameControls, Color.White, 1);

            // --- //
            Texture2D blankBackground = Content.Load<Texture2D>("BlankBackground");

            List<UiObject> newGameControls = new List<UiObject>();
            newGameControls.Add(testTextInput);
            newGameControls.Add(goButton);
            newGameControls.Add(ngbackButton);

            Texture2D plainsTexture = Content.Load<Texture2D>("Terrain\\Hills");
            Texture2D mountainsTexture = Content.Load<Texture2D>("Terrain\\Mountains");
            Texture2D desertTexture = Content.Load<Texture2D>("Terrain\\DesertFlat");
            Texture2D oceanTexture = Content.Load<Texture2D>("Terrain\\Ocean1");
            Texture2D dreadTexture = Content.Load<Texture2D>("Terrain\\Spoopy");
            Texture2D tundraTexture = Content.Load<Texture2D>("Terrain\\Tundra");
            Texture2D forestTexture = Content.Load<Texture2D>("Terrain\\Forest"); //6
            Texture2D coastTexture = Content.Load<Texture2D>("Terrain\\Coast1"); // 8-11
            Texture2D riverStraightTexture = Content.Load<Texture2D>("Terrain\\RiverStraight1"); //12-13
            Texture2D riverBendTexture = Content.Load<Texture2D>("Terrain\\RiverBend1"); //14-17


            List<Texture2D> tiles = new List<Texture2D>();

            tiles.Add(blankBackground);
            tiles.Add(plainsTexture);
            tiles.Add(mountainsTexture);
            tiles.Add(desertTexture);
            tiles.Add(oceanTexture);
            tiles.Add(dreadTexture);
            tiles.Add(tundraTexture);
            tiles.Add(forestTexture); // 7
            tiles.Add(coastTexture);
            tiles.Add(coastTexture);
            tiles.Add(coastTexture);
            tiles.Add(coastTexture); // 11
            tiles.Add(riverStraightTexture);
            tiles.Add(riverStraightTexture); // 13
            tiles.Add(riverBendTexture);
            tiles.Add(riverBendTexture);
            tiles.Add(riverBendTexture);
            tiles.Add(riverBendTexture); // 17

            Texture2D swordUnitTexture = Content.Load<Texture2D>("Units\\Kbase");
            Texture2D swordUnitAttackTexture = Content.Load<Texture2D>("Units\\Kbaseatk");
            Texture2D archerUnitTexture = Content.Load<Texture2D>("Units\\Arcbase");
            Texture2D mageUnitTexture = Content.Load<Texture2D>("Units\\Wbase");
            Texture2D baseTown = Content.Load<Texture2D>("basictownbase");
            Texture2D baseGoldMine = Content.Load<Texture2D>("goldminebase");

            engine.unitTypes.Add(new UnitType(name: "Swordsman",
                idleTextures: new[] { swordUnitTexture },
                moveTextures: new[] { swordUnitTexture },
                attackTextures: new[] { swordUnitAttackTexture },
                attackStrength: 15, defense: 2, gatherRate: 2, goldCost: 50));
            engine.unitTypes.Add(new UnitType(name: "Archer",
                idleTextures: new[] { archerUnitTexture },
                moveTextures: new[] { archerUnitTexture },
                attackTextures: new[] { archerUnitTexture },
                attackStrength: 10, attackRange: 5, defense: 0, gatherRate: 2, goldCost: 50));
            engine.unitTypes.Add(new UnitType(name: "Peasent",
                idleTextures: new[] { mageUnitTexture },
                moveTextures: new[] { mageUnitTexture },
                attackTextures: new[] { mageUnitTexture },
                attackStrength: 2, defense: 0, gatherRate: 10, goldCost: 50));
            engine.unitTypes.Add(new UnitType(name: "Town",
                idleTextures: new[] { baseTown },
                moveTextures: new[] { baseTown },
                attackTextures: new[] { baseTown },
                movementSpeed: 0, movementType: UnitType.MovementType.None,
                attackStrength: 0, defense: 10, gatherRate: 20, goldCost: 100));
            engine.unitTypes.Add(new UnitType(name: "Gold",
                idleTextures: new[] { baseGoldMine },
                moveTextures: new[] { baseGoldMine },
                attackTextures: new[] { baseGoldMine },
                movementSpeed: 0, movementType: UnitType.MovementType.None,
                attackStrength: 0, defense: 10, gatherRate: 20, goldCost: 100));

            Map m = new Map(Vector2.Zero, tiles, width, height, ref engine, GraphicsDevice);

            gameControls.Add(m);

            Menu newGameMenu = new Menu(blankBackground, Vector2.Zero, newGameControls, Color.White, 2);
            #endregion

            // hud
            Texture2D boxSelect = Content.Load<Texture2D>("BoxSelect");
            Texture2D HPbar = Content.Load<Texture2D>("HPbar");
            Hud hud = new Hud(ref engine, ref m, boxSelect, HPbar);

            gameControls.Add(hud);

            // list of all UI objects to be drawn/updated
            userInterface = new List<UiObject>();

            userInterface.Add(mainMenu);
            userInterface.Add(gameMenu);
            userInterface.Add(newGameMenu);
            mainMenu.Activate(); // activate the main menu first

            foreach (UiObject u in userInterface)
            {
                u.ChangeFont(font);
            }
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            // Allows the game to exit
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            // Toggle fullscreen
            if (keyboard.IsKeyDown(Keys.LeftAlt) && keyboard.IsKeyDown(Keys.Enter))
            {
                graphics.ToggleFullScreen();
                
            }
            // Toggle debug info
            if (keyboard.IsKeyDown(Keys.F1))
            {
                foreach (UiObject u in userInterface)
                {
                    u.ToggleDebugMode();
                }
            }

            // Update every UIObject
            foreach (UiObject x in userInterface)
            {
                x.Update(gameTime);
            }

            // TODO: Remove
            // Create testing units on the first tick
            if (engine.currentTick == 0)
            {
                for (int i = 0; i < 15; ++i)
                {
                    engine.AddUnit(new Unit(engine, engine.unitTypes[0], new Position(0, i), engine.players[0]));
                }
            }
            engine.Tick();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MediumSlateBlue);

            spriteBatch.Begin();

            foreach (UiObject x in userInterface)
            {
                x.Draw(spriteBatch);
            }

            spriteBatch.DrawString(debugFont, "Current tick: " + engine.currentTick, new Vector2(1, 1), Color.Black);
            if (engine.currentTick > 1)
            {
                spriteBatch.DrawString(debugFont, "x: " + engine.units[0].position.x, new Vector2(1, 1 + 32), Color.Black);
                spriteBatch.DrawString(debugFont, "y: " + engine.units[0].position.y, new Vector2(1, 1 + 32 * 2), Color.Black);
                spriteBatch.DrawString(debugFont, "player gold: " + engine.players[0].gold, new Vector2(1, 1 + 32 * 3), Color.Black);
                spriteBatch.DrawString(debugFont, "player iron: " + engine.players[0].iron, new Vector2(1, 1 + 32 * 4), Color.Black);
                spriteBatch.DrawString(debugFont, "player mc  : " + engine.players[0].manaCystals, new Vector2(1, 1 + 32 * 5), Color.Black);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        
        public void newButton_Clicked(object sender, EventArgs e)
        {
            //this.Exit();
            ChangeScreen(MenuScreen.NewGame); // change to new game screen
        }
        public void newButton_OnPress(object sender, EventArgs e)
        {

        }
        public void loadButton_Clicked(object sender, EventArgs e)
        {
            //this.Exit();
        }
        public void loadButton_OnPress(object sender, EventArgs e)
        {

        }

        public void mmExitButton_Clicked(object sender, EventArgs e)
        {
            this.Exit();
        }
        public void mmExitButton_OnPress(object sender, EventArgs e)
        {

        }
        public void backButton_Clicked(object sender, EventArgs e)
        {
            ChangeScreen(MenuScreen.Main); // change to main menu screen
        }
        public void backButton_OnPress(object sender, EventArgs e)
        {

        }
        public void goButton_Clicked(object sender, EventArgs e)
        {
            ChangeScreen(MenuScreen.InGame); // change to main menu screen
        }
        public void goButton_OnPress(object sender, EventArgs e)
        {

        }

        public void ngbackButton_Clicked(object sender, EventArgs e)
        {
            ChangeScreen(MenuScreen.Main); // change to main menu screen
        }
        public void ngbackButton_OnPress(object sender, EventArgs e)
        {

        }
        
        private void ChangeScreen(MenuScreen menu)
        {
            int id;
            foreach (UiObject x in userInterface)
            {
                id = Convert.ToInt32(menu);
                x.ChangeContext(id); // switch to game screen
            }
        }
    }
}
