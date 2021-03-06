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
            // Create initial players
            engine.AddPlayer("Frodo", teamIndex: 0);
            engine.AddPlayer("Sauron", teamIndex: 1);
            engine.ai1.me = engine.players[1];
            engine.ai2.me = engine.players[0];

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

            Vector2 size = new Vector2(80, 32);

            // UIObjects
            #region Buttons
            // New Game Button
            //Button newButton = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(150, 117), "New", bigFont);
            //newButton.OnPress += newButton_OnPress;
            //newButton.Clicked += newButton_Clicked;

            //// Load Game Button
            //Button loadButton = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(150, 217), "Load", bigFont);
            //loadButton.OnPress += loadButton_OnPress;
            //loadButton.Clicked += loadButton_Clicked;
            //// Settings/Options Button

            //// main menu Exit Button
            //Button mmExitButton = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(150, 317), "Exit", bigFont);
            //mmExitButton.OnPress += mmExitButton_OnPress;
            //mmExitButton.Clicked += mmExitButton_Clicked;

            //// Back Button for ingame screen
            //Button backButton = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(1250, 27), "Back", bigFont, size);
            //backButton.OnPress += backButton_OnPress;
            //backButton.Clicked += backButton_Clicked;

            //// Go Button for New Game screen
            //Button goButton = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(600, 500), "Go", bigFont);
            //goButton.OnPress += goButton_OnPress;
            //goButton.Clicked += goButton_Clicked;

            //// back button for new game
            //Button ngbackButton = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(20, 675), "Back", bigFont);
            //ngbackButton.OnPress += ngbackButton_OnPress;
            //ngbackButton.Clicked += ngbackButton_Clicked;

            //Button igBuildTown = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(240, 660), "Build Town", bigFont, size);
            //igBuildTown.OnPress += igBuildTown_OnPress;
            //igBuildTown.Clicked += igBuildTown_Clicked;

            //Button igBuildMine = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(240, 700), "Build Mine", bigFont, size);
            //igBuildMine.OnPress += igBuildMine_OnPress;
            //igBuildMine.Clicked += igBuildMine_Clicked;

            //Button igProduceKnight = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(320, 660), "Produce Knight", bigFont, size);
            //igProduceKnight.OnPress += igProduceKnight_OnPress;
            //igProduceKnight.Clicked += igProduceKnight_Clicked;

            //Button igProduceArcher = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(320, 700), "Produce Archer", bigFont, size);
            //igProduceArcher.OnPress += igProduceArcher_OnPress;
            //igProduceArcher.Clicked += igProduceArcher_Clicked;

            //Button igProducePeasant = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(400, 660), "Produce Peasant", bigFont, size);
            //igProducePeasant.OnPress += igProducePeasant_OnPress;
            //igProducePeasant.Clicked += igProducePeasant_Clicked;

            //Button igAttack = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(80, 700), "Attack", bigFont, size);
            //igAttack.OnPress += igAttack_OnPress;
            //igAttack.Clicked += igAttack_Clicked;

            //Button igMove = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(80, 660), "Move", bigFont, size);
            //igMove.OnPress += igMove_OnPress;
            //igMove.Clicked += igMove_Clicked;

            //Button igGather = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(160, 660), "Gather", bigFont, size);
            //igGather.OnPress += igGather_OnPress;
            //igGather.Clicked += igGather_Clicked;

            //Button igStop = new Button(buttonTexture, buttonHover, buttonActive, new Vector2(160, 700), "Stop", bigFont, size);
            //igStop.OnPress += igStop_OnPress;
            //igStop.Clicked += igStop_Clicked;

            #endregion
            #region TextDisplays
            // testTextDisplay
            //Texture2D blackTextBackground = Content.Load<Texture2D>("BlackTextBackground");
            SpriteFont font = Content.Load<SpriteFont>("Arial");
            //TextDisplay testTextDisplay = new TextDisplay(blackTextBackground, new Vector2(300, 300), font, Color.Green);
            #endregion
            #region TextInputs
            Texture2D textInBackground = Content.Load<Texture2D>("TextInBackground");
            Texture2D textInBackgroundActive = Content.Load<Texture2D>("TextInBackgroundActive");

            TextInput testTextInput = new TextInput(textInBackground, textInBackgroundActive, new Vector2(100, 50), font);
            //testTextInput.newInput += new EventHandler(testTextInput_newInput);

            Texture2D Biomeback = Content.Load<Texture2D>("BiomeB");
            Texture2D BiomeBackActive = Content.Load<Texture2D>("BiomeBA");

            TextInput numOcean = new TextInput(Biomeback, BiomeBackActive, new Vector2(700, 100), font);
            TextInput numPlain = new TextInput(Biomeback, BiomeBackActive, new Vector2(850, 100), font);
            TextInput numForest = new TextInput(Biomeback, BiomeBackActive, new Vector2(1000, 100), font);
            TextInput numMountain = new TextInput(Biomeback, BiomeBackActive, new Vector2(700, 200), font);
            TextInput numDesert = new TextInput(Biomeback, BiomeBackActive, new Vector2(850, 200), font);
            TextInput numTundra = new TextInput(Biomeback, BiomeBackActive, new Vector2(1000, 200), font);
            TextInput numDreadland = new TextInput(Biomeback, BiomeBackActive, new Vector2(775, 300), font);
            TextInput numResource = new TextInput(Biomeback, BiomeBackActive, new Vector2(925, 300), font);
            //testTextInput.newInput += new EventHandler(testTextInput_newInput);                                  
            numOcean.SetText("2");
            numPlain.SetText("2");
            numForest.SetText("2");
            numMountain.SetText("2");
            numDesert.SetText("2");
            numTundra.SetText("2");
            numDreadland.SetText("2");
            numResource.SetText("500");
            #endregion

            #region Menus
            Texture2D mainBackground = Content.Load<Texture2D>("MainBackground");
            Texture2D hudBackground = Content.Load<Texture2D>("Hud");

            //List<UiObject> mainControls = new List<UiObject>();
            //mainControls.Add(newButton);
            //mainControls.Add(loadButton);
            //mainControls.Add(mmExitButton);
            //mainControls.Add(testTextDisplay);

            //Menu mainMenu = new Menu(hudBackground, new Vector2(0, 0), mainControls, Color.White, 0);
            
            Texture2D blankBackground = Content.Load<Texture2D>("BlankBackground");
            Texture2D oceanTexture = Content.Load<Texture2D>("Terrain\\Ocean");
            Texture2D plainsTexture = Content.Load<Texture2D>("Terrain\\Plains1");
            Texture2D plains2Texture = Content.Load<Texture2D>("Terrain\\Plains2");
            Texture2D plains3Texture = Content.Load<Texture2D>("Terrain\\Plains3");
            Texture2D mountainsTexture = Content.Load<Texture2D>("Terrain\\Mountain1");
            Texture2D mountains2Texture = Content.Load<Texture2D>("Terrain\\Mountain2");
            Texture2D mountains3Texture = Content.Load<Texture2D>("Terrain\\Mountain3");
            Texture2D desertTexture = Content.Load<Texture2D>("Terrain\\Desert1");
            Texture2D desert2Texture = Content.Load<Texture2D>("Terrain\\Desert2");
            Texture2D desert3Texture = Content.Load<Texture2D>("Terrain\\Desert3");
            Texture2D dreadTexture = Content.Load<Texture2D>("Terrain\\Spoopy1");
            Texture2D dread2Texture = Content.Load<Texture2D>("Terrain\\Spoopy2");
            Texture2D dread3Texture = Content.Load<Texture2D>("Terrain\\Spoopy3");
            Texture2D tundraTexture = Content.Load<Texture2D>("Terrain\\Tundra1");
            Texture2D tundra2Texture = Content.Load<Texture2D>("Terrain\\Tundra2");
            Texture2D tundra3Texture = Content.Load<Texture2D>("Terrain\\Tundra3");
            Texture2D forestTexture = Content.Load<Texture2D>("Terrain\\Forest1");
            Texture2D forest2Texture = Content.Load<Texture2D>("Terrain\\Forest2");
            Texture2D forest3Texture = Content.Load<Texture2D>("Terrain\\Forest3");
            Texture2D coastTexture = Content.Load<Texture2D>("Terrain\\CoastStraight");
            Texture2D coastBendTexture = Content.Load<Texture2D>("Terrain\\CoastBend");
            Texture2D coastCoveTexture = Content.Load<Texture2D>("Terrain\\CoastCove");
            Texture2D riverStraightTexture = Content.Load<Texture2D>("Terrain\\RiverStraight");
            Texture2D riverBendTexture = Content.Load<Texture2D>("Terrain\\RiverBend");
            Texture2D riverThreeTexture = Content.Load<Texture2D>("Terrain\\River3");
            Texture2D riverFourTexture = Content.Load<Texture2D>("Terrain\\River4");
            Texture2D goldTexture = Content.Load<Texture2D>("Terrain\\Gold");
            Texture2D ironTexture = Content.Load<Texture2D>("Terrain\\Iron");
            Texture2D manaTexture = Content.Load<Texture2D>("Terrain\\Mana");


            // Create initial TileTypes
            engine.tileTypes.Add(new TileType("Ocean", texture: oceanTexture, biome: TileType.Biome.Ocean, movementCost: 7d));
            engine.tileTypes.Add(new TileType("Plain", texture: plainsTexture, biome: TileType.Biome.Plain));
            engine.tileTypes.Add(new TileType("Plain2", texture: plains2Texture, biome: TileType.Biome.Plain));
            engine.tileTypes.Add(new TileType("Plain3", texture: plains3Texture, biome: TileType.Biome.Plain));
            engine.tileTypes.Add(new TileType("Mountain", texture: mountainsTexture, biome: TileType.Biome.Mountain));
            engine.tileTypes.Add(new TileType("Mountain2", texture: mountains2Texture, biome: TileType.Biome.Mountain));
            engine.tileTypes.Add(new TileType("Mountain3", texture: mountains3Texture, biome: TileType.Biome.Mountain));
            engine.tileTypes.Add(new TileType("Forest", texture: forestTexture, biome: TileType.Biome.Forest));
            engine.tileTypes.Add(new TileType("Forest2", texture: forest2Texture, biome: TileType.Biome.Forest));
            engine.tileTypes.Add(new TileType("Forest3", texture: forest3Texture, biome: TileType.Biome.Forest));
            engine.tileTypes.Add(new TileType("Dreadlands", texture: dreadTexture, biome: TileType.Biome.Dreadlands));
            engine.tileTypes.Add(new TileType("Dreadlands2", texture: dread2Texture, biome: TileType.Biome.Dreadlands));
            engine.tileTypes.Add(new TileType("Dreadlands3", texture: dread3Texture, biome: TileType.Biome.Dreadlands));
            engine.tileTypes.Add(new TileType("Desert", texture: desertTexture, biome: TileType.Biome.Desert));
            engine.tileTypes.Add(new TileType("Desert2", texture: desert2Texture, biome: TileType.Biome.Desert));
            engine.tileTypes.Add(new TileType("Desert3", texture: desert3Texture, biome: TileType.Biome.Desert));
            engine.tileTypes.Add(new TileType("Tundra", texture: tundraTexture, biome: TileType.Biome.Tundra));
            engine.tileTypes.Add(new TileType("Tundra2", texture: tundra2Texture, biome: TileType.Biome.Tundra));
            engine.tileTypes.Add(new TileType("Tundra3", texture: tundra3Texture, biome: TileType.Biome.Tundra));

            engine.tileTypes.Add(new TileType("Gold", texture: goldTexture, biome: TileType.Biome.Gold, resourceType: TileType.ResourceType.Gold));
            engine.tileTypes.Add(new TileType("Iron", texture: ironTexture, biome: TileType.Biome.Iron, resourceType: TileType.ResourceType.Iron));
            engine.tileTypes.Add(new TileType("ManaCrystals", texture: manaTexture, biome: TileType.Biome.ManaCrystals, resourceType: TileType.ResourceType.ManaCrystals));

            engine.tileTypes.Add(new TileType("Coast Land on North", texture: coastTexture, biome: TileType.Biome.Shore, rotation: (float)Math.PI * 1.0f, movementCost: 7d)); // Clockwise radians
            engine.tileTypes.Add(new TileType("Coast Land on East", texture: coastTexture, biome: TileType.Biome.Shore, rotation: (float)Math.PI * 1.5f, movementCost: 7d));
            engine.tileTypes.Add(new TileType("Coast Land on South", texture: coastTexture, biome: TileType.Biome.Shore, rotation: (float)Math.PI * 0.0f, movementCost: 7d));
            engine.tileTypes.Add(new TileType("Coast Land on West", texture: coastTexture, biome: TileType.Biome.Shore, rotation: (float)Math.PI * 0.5f, movementCost: 7d));

            engine.tileTypes.Add(new TileType("Coast Land on North and East", texture: coastBendTexture, biome: TileType.Biome.Shore, rotation: (float)Math.PI * 1.5f, movementCost: 7d)); // Clockwise radians
            engine.tileTypes.Add(new TileType("Coast Land on East and South", texture: coastBendTexture, biome: TileType.Biome.Shore, rotation: (float)Math.PI * 0.0f, movementCost: 7d));
            engine.tileTypes.Add(new TileType("Coast Land on South and West", texture: coastBendTexture, biome: TileType.Biome.Shore, rotation: (float)Math.PI * 0.5f, movementCost: 7d));
            engine.tileTypes.Add(new TileType("Coast Land on West and North", texture: coastBendTexture, biome: TileType.Biome.Shore, rotation: (float)Math.PI * 1.0f, movementCost: 7d));

            engine.tileTypes.Add(new TileType("Coast Ocean on North", texture: coastCoveTexture, biome: TileType.Biome.Shore, rotation: (float)Math.PI * 0.5f, movementCost: 7d)); // Clockwise radians
            engine.tileTypes.Add(new TileType("Coast Ocean on East", texture: coastCoveTexture, biome: TileType.Biome.Shore, rotation: (float)Math.PI * 1.0f, movementCost: 7d));
            engine.tileTypes.Add(new TileType("Coast Ocean on South", texture: coastCoveTexture, biome: TileType.Biome.Shore, rotation: (float)Math.PI * 1.5f, movementCost: 7d));
            engine.tileTypes.Add(new TileType("Coast Ocean on West", texture: coastCoveTexture, biome: TileType.Biome.Shore, rotation: (float)Math.PI * 0.0f, movementCost: 7d));

            engine.tileTypes.Add(new TileType("River Straight Vertical", texture: riverStraightTexture, biome: TileType.Biome.River, rotation: (float)Math.PI * 0.0f, movementCost: 2d));
            engine.tileTypes.Add(new TileType("River Straight Horizontal", texture: riverStraightTexture, biome: TileType.Biome.River, rotation: (float)Math.PI * 0.5f, movementCost: 2d));
            
            engine.tileTypes.Add(new TileType("River East and South", texture: riverBendTexture, biome: TileType.Biome.River, rotation: (float)Math.PI * 1.5f, movementCost: 2d));
            engine.tileTypes.Add(new TileType("River West and South", texture: riverBendTexture, biome: TileType.Biome.River, rotation: (float)Math.PI * 0.0f, movementCost: 2d));
            engine.tileTypes.Add(new TileType("River West and North", texture: riverBendTexture, biome: TileType.Biome.River, rotation: (float)Math.PI * 0.5f, movementCost: 2d));
            engine.tileTypes.Add(new TileType("River East and North", texture: riverBendTexture, biome: TileType.Biome.River, rotation: (float)Math.PI * 1.0f, movementCost: 2d));

            engine.tileTypes.Add(new TileType("River Land on North", texture: riverThreeTexture, biome: TileType.Biome.River, rotation: (float)Math.PI * 0.0f, movementCost: 2d));
            engine.tileTypes.Add(new TileType("River Land on East", texture: riverThreeTexture, biome: TileType.Biome.River, rotation: (float)Math.PI * 0.5f, movementCost: 2d));
            engine.tileTypes.Add(new TileType("River Land on South", texture: riverThreeTexture, biome: TileType.Biome.River, rotation: (float)Math.PI * 1.0f, movementCost: 2d));
            engine.tileTypes.Add(new TileType("River Land on West", texture: riverThreeTexture, biome: TileType.Biome.River, rotation: (float)Math.PI * 1.5f, movementCost: 2d));

            engine.tileTypes.Add(new TileType("River Four", texture: riverFourTexture, biome: TileType.Biome.River, rotation: (float)Math.PI * 0.0f, movementCost: 2d));

            //Map m = new Map(Vector2.Zero, width, height, ref engine, GraphicsDevice);
            Map m = new Map(Vector2.Zero, width, height, ref engine, GraphicsDevice, numDreadland.GetTextAsInt(), numDesert.GetTextAsInt(), numPlain.GetTextAsInt(), numMountain.GetTextAsInt(), numTundra.GetTextAsInt(), numForest.GetTextAsInt(), numOcean.GetTextAsInt(), numResource.GetTextAsInt());
            //List<UiObject> gameControls = new List<UiObject>();

            Texture2D gameBackground = Content.Load<Texture2D>("gameBackground");

            // hud
            Texture2D boxSelect = Content.Load<Texture2D>("BoxSelect");
            Texture2D HPbar = Content.Load<Texture2D>("HPbar");
            Hud hud = new Hud(ref engine, ref m, boxSelect, HPbar);

            //gameControls.Add(m);
            //gameControls.Add(backButton);
            //gameControls.Add(igBuildTown);
            //gameControls.Add(igBuildMine);
            //gameControls.Add(igProduceKnight);
            //gameControls.Add(igProduceArcher);
            //gameControls.Add(igProducePeasant);
            //gameControls.Add(igAttack);
            //gameControls.Add(igMove);
            //gameControls.Add(igGather);
            //gameControls.Add(igStop);
            //gameControls.Add(hud);

            //Menu gameMenu = new Menu(hudBackground, new Vector2(0, 0), gameControls, Color.White, 1);

            // --- //

            //List<UiObject> newGameControls = new List<UiObject>();
            //newGameControls.Add(testTextInput);
            //newGameControls.Add(goButton);
            //newGameControls.Add(ngbackButton);
            //newGameControls.Add(numOcean);
            //newGameControls.Add(numForest);
            //newGameControls.Add(numPlain);
            //newGameControls.Add(numMountain);
            //newGameControls.Add(numDesert);
            //newGameControls.Add(numTundra);
            //newGameControls.Add(numDreadland);
            //newGameControls.Add(numResource);
            
            /*Texture2D swordUnitTexture = Content.Load<Texture2D>("Units\\Kbase");
            Texture2D swordUnitAttackTexture = Content.Load<Texture2D>("Units\\Kbaseatk");
            Texture2D archerUnitTexture = Content.Load<Texture2D>("Units\\Arcbase");
            Texture2D mageUnitTexture = Content.Load<Texture2D>("Units\\Wbase");
            Texture2D baseTown = Content.Load<Texture2D>("basictownbase");
            Texture2D baseGoldMine = Content.Load<Texture2D>("goldminebase");*/

            engine.unitTypes.Add(UnitType.LoadFromFile("config/Knight.xml", Content));
            engine.unitTypes.Add(UnitType.LoadFromFile("config/Archer.xml", Content));
            engine.unitTypes.Add(UnitType.LoadFromFile("config/Peasant.xml", Content));
            engine.unitTypes.Add(UnitType.LoadFromFile("config/Town.xml", Content));
            engine.unitTypes.Add(UnitType.LoadFromFile("config/Mine.xml", Content));

            /*(engine.unitTypes.Add(new UnitType(name: "Swordsman",
                idleTextures: new[] { swordUnitTexture },
                moveTextures: new[] { swordUnitTexture },
                attackTextures: new[] { swordUnitAttackTexture },
                actions: new List<UnitType.Action> { UnitType.Action.Attack, UnitType.Action.Move },
                attackStrength: 15, defense: 2, gatherRate: 2, goldCost: 100));
            engine.unitTypes.Add(new UnitType(name: "Archer",
                idleTextures: new[] { archerUnitTexture },
                moveTextures: new[] { archerUnitTexture },
                attackTextures: new[] { archerUnitTexture },
                actions: new List<UnitType.Action> { UnitType.Action.Attack, UnitType.Action.Move },
                attackStrength: 10, attackRange: 5, defense: 0, gatherRate: 2, goldCost: 100));
            engine.unitTypes.Add(new UnitType(name: "Peasent",
                idleTextures: new[] { mageUnitTexture },
                moveTextures: new[] { mageUnitTexture },
                attackTextures: new[] { mageUnitTexture },
                actions: new List<UnitType.Action> { UnitType.Action.Attack, UnitType.Action.Gather, UnitType.Action.Move, UnitType.Action.Build, UnitType.Action.Produce },
                attackStrength: 2, defense: 0, gatherRate: 2, goldCost: 50));
            engine.unitTypes.Add(new UnitType(name: "Town",
                idleTextures: new[] { baseTown },
                moveTextures: new[] { baseTown },
                attackTextures: new[] { baseTown },
                actions: new List<UnitType.Action> {  UnitType.Action.Produce },
                movementSpeed: 0, movementType: UnitType.MovementType.None, maxHealth: 1000,
                attackStrength: 0, defense: 10, gatherRate: 15, goldCost: 1000));
            engine.unitTypes.Add(new UnitType(name: "GoldMine",
                idleTextures: new[] { baseGoldMine },
                moveTextures: new[] { baseGoldMine },
                attackTextures: new[] { baseGoldMine },
                actions: new List<UnitType.Action> { UnitType.Action.Gather },
                movementSpeed: 0, movementType: UnitType.MovementType.None, maxHealth: 500,
                attackStrength: 0, defense: 10, gatherRate: 50, goldCost: 500));*/

            //Menu newGameMenu = new Menu(hudBackground, Vector2.Zero, newGameControls, Color.White, 2);
            #endregion

            SoundEngine.backgroundMusic = Content.Load<Song>("Audio\\RTS_BGM");

            // list of all UI objects to be drawn/updated
            userInterface = new List<UiObject>();

            //userInterface.Add(mainMenu);
            //userInterface.Add(gameMenu);
            //userInterface.Add(newGameMenu);
            //mainMenu.Activate(); // activate the main menu first

            userInterface = ContentLoader.ParseUI("Config.txt", Content, this, font);

            // force these


            Menu mainMenuScreen = (Menu)userInterface[0];
            Menu inGameScreen = (Menu)userInterface[1];
            Menu newGameScreen = (Menu)userInterface[2];

            inGameScreen.controls.Insert(0, m);
            inGameScreen.controls.Add(hud);

            mainMenuScreen.Activate();

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

            // Find starting point for Frodo
            Position frodoStartPosition = null;
            foreach (var position in Pathfinder.BreadthFirst(engine, new Position(0, 0), -1, -1))
            {
                if (engine.map.GetTileAt(position).tileType.movementCost < 2)
                {
                    frodoStartPosition = position;
                    break;
                }
            }

            // Find starting point for Sauron
            Position sauronStartPosition = null;
            foreach (var position in Pathfinder.BreadthFirst(engine, new Position(100, 50), -1, -1))
            {
                if (engine.map.GetTileAt(position).tileType.movementCost < 2)
                {
                    sauronStartPosition = position;
                    break;
                }
            }
            
            Menu inGameScreen = (Menu)userInterface[1];
            if (inGameScreen.active)
            {
                // TODO: Remove
                // Create testing units on the first tick
                if (engine.currentTick == 0)
                {
                    for (int i = 0; i < 10; ++i)
                    {
                        engine.AddUnit(engine.unitTypes[2], frodoStartPosition + new Position(0, i), engine.players[0]);

                        engine.AddUnit(engine.unitTypes[2], sauronStartPosition + new Position(0, i), engine.players[1]);
                    }
                }
                engine.Tick();
            }

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
            spriteBatch.End();
            
            BlendState invert = new BlendState();
            invert.AlphaBlendFunction = BlendFunction.Subtract;
            invert.AlphaSourceBlend = Blend.SourceAlpha;
            invert.AlphaDestinationBlend = Blend.DestinationAlpha;
            invert.ColorBlendFunction = BlendFunction.Add;
            invert.ColorSourceBlend = Blend.InverseDestinationColor;
            invert.ColorDestinationBlend = Blend.InverseSourceColor;

            spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, blendState: invert);
            spriteBatch.DrawString(debugFont, "Current tick: " + engine.currentTick, new Vector2(1, 1), Color.White);
            if (engine.currentTick > 1)
            {
                spriteBatch.DrawString(debugFont, "1 player gold: " + engine.players[0].gold, new Vector2(1, 1 + 32 * 1), Color.White);
                spriteBatch.DrawString(debugFont, "1 player iron: " + engine.players[0].iron, new Vector2(1, 1 + 32 * 2), Color.White);
                spriteBatch.DrawString(debugFont, "1 player mc  : " + engine.players[0].manaCystals, new Vector2(1, 1 + 32 * 3), Color.White);
                spriteBatch.DrawString(debugFont, "2 player gold: " + engine.players[1].gold, new Vector2(1, 1 + 32 * 4), Color.White);
                spriteBatch.DrawString(debugFont, "2 player iron: " + engine.players[1].iron, new Vector2(1, 1 + 32 * 5), Color.White);
                spriteBatch.DrawString(debugFont, "2 player mc  : " + engine.players[1].manaCystals, new Vector2(1, 1 + 32 * 6), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void button_Clicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            switch (b.text)
            {
                case "New":
                    newButton_Clicked(sender, e);
                    break;
                case "Load":

                    break;
                case "Exit":
                    mmExitButton_Clicked(sender, e);
                    break;
                case "Back":
                    // if ingame screen
                    if (b.parentReference == userInterface[1])
                    {
                        backButton_Clicked(sender, e);
                    }
                    else // new game screen
                    {
                        ngbackButton_Clicked(sender, e);
                    }
                    break;
                case "Build Town":
                    igBuildTown_Clicked(sender, e);
                    break;
                case "Build Mine":
                    igBuildMine_Clicked(sender, e);
                    break;
                case "Produce Knight":
                    igProduceKnight_Clicked(sender, e);
                    break;
                case "Produce Archer":
                    igProduceArcher_Clicked(sender, e);
                    break;
                case "Produce Peasant":
                    igProducePeasant_Clicked(sender, e);
                    break;
                case "Attack":
                    igAttack_Clicked(sender, e);
                    break;
                case "Move":
                    igMove_Clicked(sender, e);
                    break;
                case "Gather":
                    igGather_Clicked(sender, e);
                    break;
                case "Stop":
                    igStop_Clicked(sender, e);
                    break;
                case "Go":
                    goButton_Clicked(sender, e);
                    break;
                //case "Back":
                //    ngbackButton_Clicked(sender, e);
                //    break;
                default:
                    // nothing
                    break;
            }
        }

        public void button_OnPress(object sender, EventArgs e)
        {

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
            SoundEngine.StopBGM();
        }
        public void backButton_OnPress(object sender, EventArgs e)
        {

        }
        public void goButton_Clicked(object sender, EventArgs e)
        {
            ChangeScreen(MenuScreen.InGame); // change to main menu screen
            SoundEngine.PlayBGM();
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

        public void igBuildTown_Clicked(object sender, EventArgs e)
        {
            IssueOrder("Build Town");
        }
        public void igBuildTown_OnPress(object sender, EventArgs e)
        {

        }

        public void igBuildMine_Clicked(object sender, EventArgs e)
        {
            IssueOrder("Build Mine");
        }
        public void igBuildMine_OnPress(object sender, EventArgs e)
        {

        }

        public void igProduceKnight_Clicked(object sender, EventArgs e)
        {
            IssueOrder("Produce Knight");
        }
        public void igProduceKnight_OnPress(object sender, EventArgs e)
        {

        }

        public void igProduceArcher_Clicked(object sender, EventArgs e)
        {
            IssueOrder("Produce Archer");
        }
        public void igProduceArcher_OnPress(object sender, EventArgs e)
        {

        }

        public void igProducePeasant_Clicked(object sender, EventArgs e)
        {
            IssueOrder("Produce Peasant");
        }
        public void igProducePeasant_OnPress(object sender, EventArgs e)
        {

        }

        public void igAttack_Clicked(object sender, EventArgs e)
        {
            IssueOrder("Attack");
        }
        public void igAttack_OnPress(object sender, EventArgs e)
        {

        }

        public void igMove_Clicked(object sender, EventArgs e)
        {
            IssueOrder("Move");
        }
        public void igMove_OnPress(object sender, EventArgs e)
        {

        }

        public void igGather_Clicked(object sender, EventArgs e)
        {
            IssueOrder("Gather");
        }
        public void igGather_OnPress(object sender, EventArgs e)
        {

        }

        public void igStop_Clicked(object sender, EventArgs e)
        {
            IssueOrder("Stop");
        }
        public void igStop_OnPress(object sender, EventArgs e)
        {

        }
        private void IssueOrder(string order)
        {
            // send order to ingame screen
            (userInterface[(int)MenuScreen.InGame]).IssueOrder(order);
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
