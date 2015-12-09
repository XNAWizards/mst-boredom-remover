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

namespace mst_boredom_remover
{
    static class ContentLoader
    {
        // config setup

        /*
         * [int:number of menus]
         * [int:number of buttons for nth menu]
         * [int:xpos]
         * [int:ypos]
         * [float:scale]
         * [string:Button text]
         * ;
         * */


        /* new dynamic system
         * menu start
         * [string:type of object]
         * ...
         * [string:type of object] end
         * menu end
         * end
         * 
         * 
         
         * */

        static FileStream fs;
        static StreamReader sr;

        static SpriteFont font;

        static Texture2D btnNormal;
        static Texture2D btnHover;
        static Texture2D btnActive;

        static Game1 reference;
        static ContentManager Content;

        static Vector2 size = new Vector2(80, 32);

        private static TextInput ParseTextInput()
        {
            /*
             * text input start
             * [string:texture name]
             * [string: active texture name]
             * [int: xpos]
             * [int: ypos]
             * text input end
             * */

            string textureName = sr.ReadLine();
            string textureActiveName = sr.ReadLine();
            int xPos = Convert.ToInt32(sr.ReadLine());
            int yPos = Convert.ToInt32(sr.ReadLine());

            Texture2D texture = Content.Load<Texture2D>(textureName);
            Texture2D textureActive = Content.Load<Texture2D>(textureActiveName);

            Vector2 position = new Vector2(xPos, yPos);

            TextInput ti = new TextInput(texture, textureActive, position, font);

            return ti;
        }

        private static Button ParseButton()
        {
            /*
             * button start
             * [int:xpos]
             * [int:ypos]
             * [string:Button text]
             * [t/f use size?]
             * button end
             * */
            Vector2 pos = new Vector2();

            pos.X = Convert.ToSingle(sr.ReadLine());
            pos.Y = Convert.ToSingle(sr.ReadLine());

            string text = sr.ReadLine();

            string useSize = sr.ReadLine();

            if (sr.ReadLine() != "button end")
            {
                // somethings wrong
            }

            Button b;

            if (useSize == "f")
            {
                b = new Button(btnNormal, btnHover, btnActive, pos, text, font);
            }
            else
            {
                b = new Button(btnNormal, btnHover, btnActive, pos, text, font, size);
            }

            b.Clicked += new EventHandler(reference.button_Clicked);
            b.Clicked += new EventHandler(reference.button_OnPress);

            return b;
        }
        private static Menu ParseMenu()
        {
            // menu parsing
            /*
             * menu start
             * [string:texture name]
             * [int: xpos]
             * [int: ypos]
             * [int: thisID]
             * menu controls start
             * [control type] start
             * menu controls end
             * menu end
             * */

            string textureName = sr.ReadLine();
            int xPos = Convert.ToInt32(sr.ReadLine());
            int yPos = Convert.ToInt32(sr.ReadLine());
            int thisID = Convert.ToInt32(sr.ReadLine());

            Texture2D menuTexture = Content.Load<Texture2D>(textureName);
            Vector2 position = new Vector2(xPos, yPos);

            if (sr.ReadLine() != "menu controls start")
            {
                // something is wrong
            }

            List<UiObject> controls = new List<UiObject>();

            string objectType = sr.ReadLine();

            // foreach button on this menu
            while (objectType != "menu controls end")
            {
                switch (objectType)
                {
                    case "button start":
                        Button b = ParseButton();
                        controls.Add(b);
                        break;
                    case "text input start":
                        TextInput t = ParseTextInput();
                        controls.Add(t);
                        break;
                    case "text display start":

                        break;
                    case "map start":

                        break;
                    case "hud start":

                        break;
                }

                objectType = sr.ReadLine();
            }
            if (sr.ReadLine() != "menu end")
            {
                // something is wrong
            }

            Menu m = new Menu(menuTexture, position, controls, Color.White, thisID);

            return m;
        }
        public static List<UiObject> ParseUI(string config, ContentManager paramContent, Game1 parReference, SpriteFont paramfont)
        {
            font = paramfont;
            Content = paramContent;
            List<UiObject> userInterface = new List<UiObject>();

            btnNormal = Content.Load<Texture2D>("buttonNormal");
            btnHover = Content.Load<Texture2D>("buttonHover");
            btnActive = Content.Load<Texture2D>("buttonActive");

            fs = new FileStream(config, FileMode.Open);
            sr = new StreamReader(fs);

            reference = parReference;

            // foreach menu
            while (sr.ReadLine() == "menu start")
            {
                userInterface.Add(ParseMenu());
            }

            if (sr.ReadLine() != "end")
            {
                // somethings wrong
            }

            sr.Close();
            fs.Close();

            return userInterface;
        }
    }
}
