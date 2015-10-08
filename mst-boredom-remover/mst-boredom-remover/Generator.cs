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
            public char Type;
            public int X;
            public int Y;
        };
        public static char[,] generate(int width, int height)
        {
            // chance out of 1000
            
            int RX = width - 10;
            int RY = height - 10;
            const int MAX_CHANCE = 1000;
            const int GOLD_CHANCE = 5;
            const int IRON_CHANCE = 2;
            const int MANA_CHANCE = 2;
            const int NumBio = 800;
            const int Border = 2000;
            const int numRivers = 50;
            Random r = new Random();
            BiomeInfo[] bio = new BiomeInfo[NumBio + Border];
            for (int i = 0; i < bio.Length; i++)
            {
                bio[i] = new BiomeInfo();
            }
            for (int a = 0; a < (NumBio / 8); a++)
            {
                bio[a * 8].Type = '~'; //Ocean
                bio[a * 8].X = r.Next(0, width);
                bio[a * 8].Y = r.Next(0, height);

                bio[a * 8 + 1].Type = '+';//Plain
                bio[a * 8 + 1].X = r.Next(10, RX);
                bio[a * 8 + 1].Y = r.Next(10, RY);

                bio[a * 8 + 2].Type = 'M';//Mountain
                bio[a * 8 + 2].X = r.Next(10, RX);
                bio[a * 8 + 2].Y = r.Next(10, RY);

                bio[a * 8 + 3].Type = 'F';//Forest
                bio[a * 8 + 3].X = r.Next(10, RX);
                bio[a * 8 + 3].Y = r.Next(10, RY);

                bio[a * 8 + 4].Type = '%';//Dreadlands
                bio[a * 8 + 4].X = r.Next(10, RX);
                bio[a * 8 + 4].Y = r.Next(10, RY);

                bio[a * 8 + 5].Type = 'D';//Desert
                bio[a * 8 + 5].X = r.Next(10, RX);
                bio[a * 8 + 5].Y = r.Next(10, RY);

                bio[a * 8 + 6].Type = 'T';//Tundra
                bio[a * 8 + 6].X = r.Next(10, RX);
                bio[a * 8 + 6].Y = r.Next(10, RY);

                bio[a * 8 + 7].Type = '~'; //Ocean
                bio[a * 8 + 7].X = r.Next(0, width);
                bio[a * 8 + 7].Y = r.Next(0, height);
            }
            //Sets Border To Ocean Biome
            for (int k = 0; k < width; k++)
            {//goes through and sets top and bottom rows to Ocean biome.
                bio[NumBio + k * 2].Type = '~';
                bio[NumBio + k * 2].X = k;
                bio[NumBio + k * 2].Y = 0;

                bio[NumBio + k * 2 + 1].Type = '~';
                bio[NumBio + k * 2 + 1].X = k;
                bio[NumBio + k * 2 + 1].Y = height - 1;
            }
            for (int l = 0; l < height; l++)
            {//Sets Left and Right border to Ocean biome
                bio[NumBio + width + l * 2].Type = '~';
                bio[NumBio + width + l * 2].X = 0;
                bio[NumBio + width + l * 2].Y = l;

                bio[NumBio + width + l * 2 + 1].Type = '~';
                bio[NumBio + width + l * 2 + 1].X = width - 1;
                bio[NumBio + width + l * 2 + 1].Y = l;
            }

            char[,] field = new char[width, height];
            int[,] elevation = new int[width, height];
            // i = y
            // j = x
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    char nearest = '~';
                    int dist = 5000;
                    for (int z = 0; z < NumBio + Border; z++)
                    {
                        int Xdiff = bio[z].X - j;
                        int Ydiff = bio[z].Y - i;
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
			
			//River Algorithm
			for(int i=0;i<100;i++){
				for(int j=0;j<100;j++){
					if(field[j, i]=='M')
						elevation[j, i] = r.Next(40,90);
					if(field[j, i]=='T')
						elevation[j, i] = r.Next(10,40);
					if(field[j, i]=='P')
						elevation[j, i] = r.Next(5,35);
					if(field[j, i]=='D')
						elevation[j, i] = r.Next(10,35);
					if(field[j, i]=='%')
						elevation[j, i] = r.Next(0,20);
					if(field[j, i]=='~')
						elevation[j, i] = 0;
					if(field[j, i]=='F')
						elevation[j, i] = r.Next(20,60);
				}
			}
			
			for(int i=0;i<numRivers;i++){
				int riverX = r.Next(50,width-50);
				int riverY = r.Next(50,height-50);
				if(elevation[riverX,riverY]<25){
					riverX = r.Next(50,width-50);
					riverY = r.Next(50,height-50);
				}
<<<<<<< HEAD
				int Direction; //0=North, 1=East, 2=South 3=West
				int lastDir;
=======
				int Direction = r.Next(0, 4); //0=North, 1=East, 2=South 3=West
>>>>>>> origin/MapGen
				int riverLength = r.Next(0,6);
				switch(riverLength){//0=medium, 1=long, 2=extensive, else small
					case 0:
						riverLength = r.Next(100,180);
						break;
					case 1:
						riverLength = r.Next(200,350);
						break;
					case 2:
						riverLength = r.Next(400,600);
						break;
					default:
						riverLength = r.Next(30,70);
						break;
				}
				
				//make this random length
				field[riverX,riverY] = '-';//Designed for '-' character, but use ocean biome for now.
				for(int j=0;j<riverLength;j++){
					int minHeight = 100;
					if(lastDir != 2 &&elevation[riverX-1,riverY]<minHeight){
						minHeight = elevation[riverX-1,riverY];
						Direction=0;
					}
					if(lastDir != 3 &&elevation[riverX,riverY+1]<minHeight){
						minHeight = elevation[riverX,riverY+1];
						Direction=1;
					}
					if(lastDir != 0 &&elevation[riverX+1,riverY]<minHeight){
						minHeight = elevation[riverX+1,riverY];
						Direction=2;
					}
					if(lastDir != 1 &&elevation[riverX,riverY-1]<minHeight){
						minHeight = elevation[riverX,riverY-1];
                        Direction = 3;
					}
					switch(Direction){
						case 0:
							riverX-=1;
							break;
						case 1:
							riverY+=1;
							break;
						case 2:
							riverX+=1;
							break;
						case 3:
							riverY-=1;
							break;
						default:
							break;
					}
					lastDir = Direction;
					if(field[riverX,riverY]=='~')
						break;
					else
						field[riverX,riverY]='-';
				}
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
