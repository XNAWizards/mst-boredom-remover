using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
{
    class Generator
    {
        public static char[,] generate(int width, int height)
        {
            // chance out of 1000
            const int MAX_CHANCE = 1000;
            const int GOLD_CHANCE = 5;
            const int IRON_CHANCE = 2;
            const int MANA_CHANCE = 2;
            const int NumBio = 700;
            Random r = new Random();
            Map.BiomeInfo[] bio = new Map.BiomeInfo[NumBio];
            for (int i = 0; i < bio.Length; i++)
            {
                bio[i] = new Map.BiomeInfo();
            }
            for (int a = 0; a < (NumBio / 7); a++)
            {
                bio[a * 7].Type = '~'; //Ocean
                bio[a * 7].X = r.Next(0, width);
                bio[a * 7].Y = r.Next(0, height);

                bio[a * 7 + 1].Type = '+';//Plain
                bio[a * 7 + 1].X = r.Next(0, width);
                bio[a * 7 + 1].Y = r.Next(0, height);

                bio[a * 7 + 2].Type = 'M';//Mountain
                bio[a * 7 + 2].X = r.Next(0, width);
                bio[a * 7 + 2].Y = r.Next(0, height);

                bio[a * 7 + 3].Type = 'F';//Forest
                bio[a * 7 + 3].X = r.Next(0, width);
                bio[a * 7 + 3].Y = r.Next(0, height);

                bio[a * 7 + 4].Type = '%';//Dreadlands
                bio[a * 7 + 4].X = r.Next(0, width);
                bio[a * 7 + 4].Y = r.Next(0, height);

                bio[a * 7 + 5].Type = 'D';//Desert
                bio[a * 7 + 5].X = r.Next(0, width);
                bio[a * 7 + 5].Y = r.Next(0, height);

                bio[a * 7 + 6].Type = 'T';//Tundra
                bio[a * 7 + 6].X = r.Next(0, width);
                bio[a * 7 + 6].Y = r.Next(0, height);
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
                    for (int z = 0; z < NumBio; z++)
                    {
                        int Xdiff = bio[z].X - i;
                        int Ydiff = bio[z].Y - j;
                        int Cdist = Xdiff * Xdiff + Ydiff * Ydiff;
                        if (Cdist < dist)
                        {
                            nearest = bio[z].Type;
                            dist = Cdist;
                        }

                    }
                    field[j, i] = nearest;
                }
            }
            for (int i = 0; i < height; i++)
            {
                field[0, i] = '~';          // left side
                field[width - 1, i] = '~';  // right side
            }
            for (int j = 0; j < width; j++)
            {
                field[j, 0] = '~';          // top
                field[j, height - 1] = '~';  // bottom
            }
            //Adding Resources.
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (field[j, i] == 'M')
                    {
                        if (r.Next(0, MAX_CHANCE) <= GOLD_CHANCE)
                            field[j, i] = 'G';//inserts gold mine resource
                    }
                    else if (field[j, i] == 'F')
                    {
                        if (r.Next(0, MAX_CHANCE) <= IRON_CHANCE)
                            field[j, i] = 'L';//sawmill for lumber
                    }
                    else if (field[j, i] == '%')
                    {
                        if (r.Next(0, MAX_CHANCE) <= MANA_CHANCE)
                            field[j, i] = '*';//magic crystal resource
                    }

                }
            }
            return field;
        }
    }
}
