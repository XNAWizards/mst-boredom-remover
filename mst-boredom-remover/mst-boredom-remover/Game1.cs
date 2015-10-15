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

namespace mst_boredom_remover
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        
        // TODO: Remove this
        private SpriteFont _debugFont;

        private Engine _engine;
        
        public const int width = 1280 / 2;
        public const int height = 720 / 2;

        List<UiObject> _userInterface;
        public enum MenuScreen
        {
            Main,           // 0 main menu screen
            InGame,         // 1 in game screen
            NewGame,        // 2 New Game options screen
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            _engine = new Engine(width, height);

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;

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
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            _debugFont = Content.Load<SpriteFont>("debug_font");

            // UIObjects
            #region Buttons
            // New Game Button
            Texture2D newButtonTexture = Content.Load<Texture2D>("Buttons\\BtnNew");
            Texture2D newButtonTextureHover = Content.Load<Texture2D>("Buttons\\BtnNewHover");
            Button newButton = new Button(newButtonTexture, newButtonTextureHover, newButtonTextureHover, new Vector2(150,117));
            newButton.OnPress += new EventHandler(newButton_OnPress);
            newButton.Clicked += new EventHandler(newButton_Clicked);

            // Load Game Button
            Texture2D loadButtonTexture = Content.Load<Texture2D>("Buttons\\BtnLoad");
            Texture2D loadButtonTextureHover = Content.Load<Texture2D>("Buttons\\BtnLoadHover");
            Button loadButton = new Button(loadButtonTexture, loadButtonTextureHover, loadButtonTextureHover, new Vector2(150, 217));
            loadButton.OnPress += new EventHandler(loadButton_OnPress);
            loadButton.Clicked += new EventHandler(loadButton_Clicked);
            // Settings/Options Button

            // main menu Exit Button
            Texture2D mmExitButtonTexture = Content.Load<Texture2D>("Buttons\\BtnExit");
            Texture2D mmExitButtonTextureHover = Content.Load<Texture2D>("Buttons\\BtnExitHover");
            Button mmExitButton = new Button(mmExitButtonTexture, mmExitButtonTextureHover, mmExitButtonTextureHover, new Vector2(150, 317));
            mmExitButton.OnPress += new EventHandler(mmExitButton_OnPress);
            mmExitButton.Clicked += new EventHandler(mmExitButton_Clicked);

            // Back Button for ingame screen
            Texture2D backButtonTexture = Content.Load<Texture2D>("Buttons\\BtnBack");
            Texture2D backButtonTextureHover = Content.Load<Texture2D>("Buttons\\BtnBackHover");
            Button backButton = new Button(backButtonTexture, backButtonTextureHover, backButtonTextureHover, new Vector2(1250, 27), .5f);
            backButton.OnPress += new EventHandler(backButton_OnPress);
            backButton.Clicked += new EventHandler(backButton_Clicked);

            // Go Button for New Game screen
            Texture2D goButtonTexture = Content.Load<Texture2D>("Buttons\\BtnGo");
            Texture2D goButtonTextureHover = Content.Load<Texture2D>("Buttons\\BtnGoHover");
            Button goButton = new Button(goButtonTexture, goButtonTextureHover, goButtonTextureHover, new Vector2(600, 500));
            goButton.OnPress += new EventHandler(goButton_OnPress);
            goButton.Clicked += new EventHandler(goButton_Clicked);

            // back button for new game
            Texture2D ngbackButtonTexture = Content.Load<Texture2D>("Buttons\\BtnBack");
            Texture2D ngbackButtonTextureHover = Content.Load<Texture2D>("Buttons\\BtnBackHover");
            Button ngbackButton = new Button(backButtonTexture, backButtonTextureHover, backButtonTextureHover, new Vector2(20, 675), .5f);
            ngbackButton.OnPress += new EventHandler(ngbackButton_OnPress);
            ngbackButton.Clicked += new EventHandler(ngbackButton_Clicked);

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
            Texture2D forestTexture = Content.Load<Texture2D>("Terrain\\Forest");

            List<Texture2D> tiles = new List<Texture2D>();

            tiles.Add(blankBackground);
            tiles.Add(plainsTexture);
            tiles.Add(mountainsTexture);
            tiles.Add(desertTexture);
            tiles.Add(oceanTexture);
            tiles.Add(dreadTexture);
            tiles.Add(tundraTexture);
            tiles.Add(forestTexture);

            Texture2D swordUnitTexture = Content.Load<Texture2D>("Units\\knightbase");
            Texture2D archerUnitTexture = Content.Load<Texture2D>("Units\\knightbase"); // temp
            Texture2D mageUnitTexture = Content.Load<Texture2D>("Units\\magebase");

            _engine.unitTypes.Add(new UnitType(name: "Swordsman",
                idleTextures: new Texture2D[] { swordUnitTexture },
                moveTextures: new Texture2D[] { swordUnitTexture },
                attackTextures: new Texture2D[] { swordUnitTexture }));

            Map m = new Map(Vector2.Zero, tiles, width, height, ref _engine, GraphicsDevice);

            gameControls.Add(m);

            Menu newGameMenu = new Menu(blankBackground, Vector2.Zero, newGameControls, Color.White, 2);
            #endregion

            // list of all UI objects to be drawn/updated
            _userInterface = new List<UiObject>();

            _userInterface.Add(mainMenu);
            _userInterface.Add(gameMenu);
            _userInterface.Add(newGameMenu);
            mainMenu.Activate(); // activate the main menu first

            foreach (UiObject u in _userInterface)
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
                _graphics.ToggleFullScreen();
                
            }
            // Toggle debug info
            if (keyboard.IsKeyDown(Keys.F1))
            {
                foreach (UiObject u in _userInterface)
                {
                    u.ToggleDebugMode();
                }
            }

            // Update every UIObject
            foreach (UiObject x in _userInterface)
            {
                x.Update(gameTime);
            }

            // TODO: Remove
            // Create testing units on the first tick
            if (_engine.currentTick == 0)
            {
                for (int i = 0; i < 15; ++i)
                {
                    _engine.AddUnit(new Unit(_engine, _engine.unitTypes[0], new Position(0, i), _engine.players[0]));
                }
            }
            _engine.Tick();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MediumSlateBlue);

            _spriteBatch.Begin();

            foreach (UiObject x in _userInterface)
            {
                x.Draw(_spriteBatch);
            }

            _spriteBatch.DrawString(_debugFont, "Current tick: " + _engine.currentTick, new Vector2(1, 1), Color.Black);
            if (_engine.currentTick > 1)
            {
                _spriteBatch.DrawString(_debugFont, "x: " + _engine.units[0].position.x, new Vector2(1, 1 + 32), Color.Black);
                _spriteBatch.DrawString(_debugFont, "y: " + _engine.units[0].position.y, new Vector2(1, 1 + 32 * 2), Color.Black);
            }

            _spriteBatch.End();
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
            foreach (UiObject x in _userInterface)
            {
                id = System.Convert.ToInt32(menu);
                x.ChangeContext(id); // switch to game screen
            }
        }
    }
}
