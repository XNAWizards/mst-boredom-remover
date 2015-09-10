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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        
        List<UIObject> userInterface;
        Button newButton;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            IsMouseVisible = true;
            graphics.ToggleFullScreen(); // activate full screen mode. toggle with alt + enter
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

            #region Buttons
            // New Game Button
            Texture2D newButtonTexture = Content.Load<Texture2D>("Buttons\\BtnNew");
            Texture2D newButtonTextureHover = Content.Load<Texture2D>("Buttons\\BtnNewHover");
            Button newButton = new Button(newButtonTexture, newButtonTextureHover, newButtonTextureHover, new Vector2(100,100));
            newButton.OnPress += new EventHandler(newButton_OnPress);
            newButton.Clicked += new EventHandler(newButton_Clicked);

            // Load Game Button
            Texture2D loadButtonTexture = Content.Load<Texture2D>("Buttons\\BtnLoad");
            Texture2D loadButtonTextureHover = Content.Load<Texture2D>("Buttons\\BtnLoadHover");
            Button loadButton = new Button(loadButtonTexture, loadButtonTextureHover, loadButtonTextureHover, new Vector2(100, 200));
            loadButton.OnPress += new EventHandler(loadButton_OnPress);
            loadButton.Clicked += new EventHandler(loadButton_Clicked);
            // Settings/Options Button

            // Exit Button

            // Back Button
            Texture2D backButtonTexture = Content.Load<Texture2D>("Buttons\\BtnBack");
            Texture2D backButtonTextureHover = Content.Load<Texture2D>("Buttons\\BtnBackHover");
            Button backButton = new Button(backButtonTexture, backButtonTextureHover, backButtonTextureHover, new Vector2(1200, 10), .5f);
            backButton.OnPress += new EventHandler(backButton_OnPress);
            backButton.Clicked += new EventHandler(backButton_Clicked);

            #endregion
            #region TextDisplays
            // testTextDisplay
            Texture2D blackTextBackground = Content.Load<Texture2D>("BlackTextBackground");
            SpriteFont font = Content.Load<SpriteFont>("Arial");
            TextDisplay testTextDisplay = new TextDisplay(blackTextBackground, new Vector2(100, 300), font, Color.Green);
            #endregion
            #region Menus
            Texture2D mainBackground = Content.Load<Texture2D>("MainBackground");

            List<UIObject> mainControls = new List<UIObject>();
            mainControls.Add(newButton);
            mainControls.Add(loadButton);
            mainControls.Add(testTextDisplay);

            Menu mainMenu = new Menu(mainBackground, new Vector2(0, 0), mainControls, Color.White, 0);

            Texture2D gameBackground = Content.Load<Texture2D>("gameBackground");

            List<UIObject> gameControls = new List<UIObject>();

            gameControls.Add(backButton);

            Menu gameMenu = new Menu(gameBackground, new Vector2(0, 0), gameControls, Color.White, 1);
            #endregion
            
            // list of all UI objects to be drawn/updated
            userInterface = new List<UIObject>();

            userInterface.Add(mainMenu);
            userInterface.Add(gameMenu);
            mainMenu.activate(); // activate the main user interface

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
            if (keyboard.IsKeyDown(Keys.D0))
            {
                changeScreen(0);
            }
            if (keyboard.IsKeyDown(Keys.D1))
            {
                changeScreen(1);
            }
            if (keyboard.IsKeyDown(Keys.LeftAlt) && keyboard.IsKeyDown(Keys.Enter))
            {
                graphics.ToggleFullScreen();
            }
            
            foreach (UIObject x in userInterface)
            {
                x.Update(gameTime);
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MediumSlateBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            foreach (UIObject x in userInterface)
            {
                x.Draw(spriteBatch);
            }
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public void newButton_Clicked(object sender, EventArgs e)
        {
            this.Exit();
        }
        public void newButton_OnPress(object sender, EventArgs e)
        {

        }
        public void loadButton_Clicked(object sender, EventArgs e)
        {
            this.Exit();
        }
        public void loadButton_OnPress(object sender, EventArgs e)
        {

        }
        public void backButton_Clicked(object sender, EventArgs e)
        {
            changeScreen(0);
        }
        public void backButton_OnPress(object sender, EventArgs e)
        {

        }

        private void changeScreen(int id)
        {
            foreach (UIObject x in userInterface)
            {
                x.changeContext(id); // switch to game screen
            }
        }
    }
}
