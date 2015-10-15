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
    public static class Global
    {
        public static SpriteFont font;
        public static SpriteFont bigFont;
        public static int screen = 0;
        public static int[,,] world;
        public static int SIZE_X = 4300;
        public static int SIZE_Y = 1200;
        public static Vector2 fontLocation = new Vector2(100, 100);
        public static int FPS = 0;
        public static int FPSat1 = 0;
        public static double timer = 0;
        public static float ScreenWidth = 1600; //GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static float ScreenHeight = 1200; //GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        public static string saveLoc = "E:\\Worlds\\";
        public static Texture2D grid;
        public static bool debugMode = false;
        public static int usingWorld = 0;
        public static bool digging = false;
        //public static Vector2 blockBeingDug = new Vector2(0, 0);
        //public static bool leftMouse = false;
        public static bool hover = false;
        public static bool displayInv = false;
        public static bool displayBook = false;

        public static bool[] CheckWorlds()
        {
            bool[] worlds = new bool[5];
            for (int x = 1; x <= 5; x++)
            {
                if (File.Exists(saveLoc + "World" + x + ".wld"))
                {
                    worlds[x - 1] = true;
                }
            }
            return worlds;
        }
        public static MouseState GetMouse()
        {
            return Mouse.GetState();
        }
    }

}
