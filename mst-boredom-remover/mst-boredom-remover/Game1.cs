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

            IsMouseVisible = true;

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

            // testTextDisplay
            Texture2D blackTextBackground = Content.Load<Texture2D>("BlackTextBackground");
            SpriteFont font = Content.Load<SpriteFont>("Arial");
            TextDisplay testTextDisplay = new TextDisplay(blackTextBackground, new Vector2(100, 300), font, Color.White);

            // list of all UI objects to be drawn/updated
            userInterface = new List<UIObject>();
            userInterface.Add(newButton);
            userInterface.Add(loadButton);
            userInterface.Add(testTextDisplay);

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
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
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
    }
}
