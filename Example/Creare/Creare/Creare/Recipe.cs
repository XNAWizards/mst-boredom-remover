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
    class Recipe
    {
        string name;
        Texture2D texture;
        public int[,] requirements = new int[8, 2];
        public int itemIndex = 0;

        public Recipe(string name, int[,] requirements, int itemIndex)
        {
            this.name = name;
            this.requirements = requirements;
            this.itemIndex = itemIndex;
        }
    }
}
