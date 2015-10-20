﻿using System;
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
            int tundraAmount = 2;
            int forestAmount = 2;
            int mountainAmount = 2;
            int plainAmount = 2;
            int dreadlandAmount = 2;
            int desertAmount = 2;
            int oceanAmount = 2;
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
            
			//start terrain set
            int p = 0;
            while (p < 750){
                //check for NO or LOW biome values.
              if (tundraAmount == 0) { }
              else
              {
                    bio[p].Type = 'T';//Tundra
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (forestAmount == 0) { }
              else
              {
                    bio[p].Type = 'F';//Forest
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (mountainAmount == 0) { }
              else
              {
                    bio[p].Type = 'M';//Mountain
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (plainAmount == 0) { }
              else
              {
                    bio[p].Type = '+';//Plain
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (dreadlandAmount == 0) { }
              else
              {
                    bio[p].Type = '%';//Dreadland
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (desertAmount == 0) { }
              else
              {
                    bio[p].Type = 'D';//Desert
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (oceanAmount == 0) { }
              else
              {
                    bio[p].Type = '~'; //Ocean
                    bio[p].X = r.Next(0, width);
                    bio[p].Y = r.Next(0, height);
                    p++;
              }

              //checks for DEFAULT/NORMAL biome value
              if (tundraAmount >= 2)
              {
                    bio[p].Type = 'T';//Tundra
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (forestAmount >= 2)
              {
                    bio[p].Type = 'F';//Forest
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (mountainAmount >= 2)
              {
                    bio[p].Type = 'M';//Mountain
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (plainAmount >= 2)
              {
                    bio[p].Type = '+';//Plain
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (dreadlandAmount >= 2)
              {
                    bio[p].Type = '%';//Dreadland
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (desertAmount >= 2)
              {
                    bio[p].Type = 'D';//Desert
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (oceanAmount >= 2)
              {
                    bio[p].Type = '~'; //Ocean
                    bio[p].X = r.Next(0, width);
                    bio[p].Y = r.Next(0, height);
                    p++;
                }
              //checks for HIGH biome value
              if (tundraAmount >= 3)
              {
                    bio[p].Type = 'T';//Tundra
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (forestAmount >= 3)
              {
                    bio[p].Type = 'F';//Forest
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (mountainAmount >= 3)
              {
                    bio[p].Type = 'M';//Mountain
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (plainAmount >= 3)
              {
                    bio[p].Type = '+';//Plain
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (dreadlandAmount >= 3)
              {
                    bio[p].Type = '%';//Dreadland
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (desertAmount >= 3)
              {
                    bio[p].Type = 'D';//Desert
                    bio[p].X = r.Next(10, RX);
                    bio[p].Y = r.Next(10, RY);
                    p++;
                }
              if (oceanAmount >= 3)
              {
                    bio[p].Type = '~'; //Ocean
                    bio[p].X = r.Next(0, width);
                    bio[p].Y = r.Next(0, height);
                    p++;
                }
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
				
				int lastDir =5;
				int Direction = r.Next(0, 4); //0=North, 1=East, 2=South 3=West
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
			//Generates coastlines and gives rivers a direction.
			//each direction of river/Ocean will be set to a different character.
			for(int j=1;j<width-1;j++)
			{
				for(int i=1;i<height-1;i++)
				{
					//******Ocean Border*****//
					if(field[j,i]=='~')
					{
						if(field[j-1,i]!='~' && field[j-1,i-1]!='~' && field[j-1,i+1]!='~')
						{
							field[j,i]='@';//Coast tile with land on north.
						}
						if(field[j+1,i+1]!='~' && field[j,i+1]!='~' && field[j-1,i+1]!='~')
						{
							field[j,i]='%';//Coast tile with land on east.
						}
						if(field[j+1,i]!='~' && field[j+1,i-1]!='~' && field[j+1,i+1]!='~')
						{
							field[j,i]='&';//Coast tile with land on south.
						}
						if(field[j+1,i-1]!='~' && field[j,i-1]!='~' && field[j-1,i-1]!='~')
						{
							field[j,i]='#';//Coast tile with land on west.
						}
					}
					//*****River Direction*****//
					if(field[j,i]=='-')
					{
						if(field[j-1,i]=='-' && field[j+1,i]=='-')
						{
							field[j,i]='^';//River running north to south.
						}
						if(field[j,i-1]=='-' && field[j+i+1]=='-')
						{
							field[j,i]=',';//River running east to west.
						}
						if(field[j+1,i]=='-' && field[j,i+1]=='-')
						{
							field[j,i]='<';//River mouths at east and south.
						}
						if(field[j+1,i]=='-' && field[j,i-1]=='-')
						{
							field[j,i]='>';//River mouths at west and south.
						}
						if(field[j-1,i]=='-' && field[j,i-1]=='-')
						{
							field[j,i]=']';//River mouths at west and north.
						}
						if(field[j-1,i]=='-' && field[j,i+1]=='-')
						{
							field[j,i]='[';//River mouths at east and north.
						}
					}
				}
			}
            return field;
        }
    }
}
