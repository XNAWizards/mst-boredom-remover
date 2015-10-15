using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
{
    class Generator
    {
        public struct BiomeInfo
        {
            public char type;
            public int x;
            public int y;
        };
        public static char[,] Generate(int width, int height)
        {
            // chance out of 1000
            
            int rx = width - 10;
            int ry = height - 10;
            const int maxChance = 1000;
            const int goldChance = 5;
            const int ironChance = 2;
            const int manaChance = 2;
            const int numBio = 800;
            const int border = 2000;
            Random r = new Random();
            BiomeInfo[] bio = new BiomeInfo[numBio + border];
            for (int i = 0; i < bio.Length; i++)
            {
                bio[i] = new BiomeInfo();
            }
            for (int a = 0; a < (numBio / 8); a++)
            {
                bio[a * 8].type = '~'; //Ocean
                bio[a * 8].x = r.Next(0, width);
                bio[a * 8].y = r.Next(0, height);

                bio[a * 8 + 1].type = '+';//Plain
                bio[a * 8 + 1].x = r.Next(10, rx);
                bio[a * 8 + 1].y = r.Next(10, ry);

                bio[a * 8 + 2].type = 'M';//Mountain
                bio[a * 8 + 2].x = r.Next(10, rx);
                bio[a * 8 + 2].y = r.Next(10, ry);

                bio[a * 8 + 3].type = 'F';//Forest
                bio[a * 8 + 3].x = r.Next(10, rx);
                bio[a * 8 + 3].y = r.Next(10, ry);

                bio[a * 8 + 4].type = '%';//Dreadlands
                bio[a * 8 + 4].x = r.Next(10, rx);
                bio[a * 8 + 4].y = r.Next(10, ry);

                bio[a * 8 + 5].type = 'D';//Desert
                bio[a * 8 + 5].x = r.Next(10, rx);
                bio[a * 8 + 5].y = r.Next(10, ry);

                bio[a * 8 + 6].type = 'T';//Tundra
                bio[a * 8 + 6].x = r.Next(10, rx);
                bio[a * 8 + 6].y = r.Next(10, ry);

                bio[a * 8 + 7].type = '~'; //Ocean
                bio[a * 8 + 7].x = r.Next(0, width);
                bio[a * 8 + 7].y = r.Next(0, height);
            }
            //Sets Border To Ocean Biome
            for (int k = 0; k < width; k++)
            {//goes through and sets top and bottom rows to Ocean biome.
                bio[numBio + k * 2].type = '~';
                bio[numBio + k * 2].x = k;
                bio[numBio + k * 2].y = 0;

                bio[numBio + k * 2 + 1].type = '~';
                bio[numBio + k * 2 + 1].x = k;
                bio[numBio + k * 2 + 1].y = height - 1;
            }
            for (int l = 0; l < height; l++)
            {//Sets Left and Right border to Ocean biome
                bio[numBio + width + l * 2].type = '~';
                bio[numBio + width + l * 2].x = 0;
                bio[numBio + width + l * 2].y = l;

                bio[numBio + width + l * 2 + 1].type = '~';
                bio[numBio + width + l * 2 + 1].x = width - 1;
                bio[numBio + width + l * 2 + 1].y = l;
            }

            char[,] field = new char[width, height];
            // i = y
            // j = x
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    char nearest = '~';
                    int dist = 5000;
                    for (int z = 0; z < numBio + border; z++)
                    {
                        int xdiff = bio[z].x - j;
                        int ydiff = bio[z].y - i;
                        int cdist = xdiff * xdiff + ydiff * ydiff;
                        if (cdist < dist)
                        {
                            nearest = bio[z].type;
                            dist = cdist;
                        }

                    }
                    field[j, i] = nearest;
                }
            }
            /*for (int i = 0; i < height; i++)
            {
                field[0, i] = '~';          // left side
                field[width - 1, i] = '~';  // right side
            }
            for (int j = 0; j < width; j++)
            {
                field[j, 0] = '~';          // top
                field[j, height - 1] = '~';  // bottom
            }*/
            //Adding Resources.
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (field[j, i] == 'M')
                    {
                        if (r.Next(0, maxChance) <= goldChance)
                            field[j, i] = 'G';//inserts gold mine resource
                    }
                    else if (field[j, i] == 'F')
                    {
                        if (r.Next(0, maxChance) <= ironChance)
                            field[j, i] = 'L';//sawmill for lumber
                    }
                    else if (field[j, i] == '%')
                    {
                        if (r.Next(0, maxChance) <= manaChance)
                            field[j, i] = '*';//magic crystal resource
                    }

                }
            }
            return field;
        }
    }
}
