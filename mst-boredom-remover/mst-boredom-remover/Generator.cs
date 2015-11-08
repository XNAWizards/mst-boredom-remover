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
        public static char[,] generate(int width, int height, int typeDrd, int typeDst, int typeP, int typeM, int typeT, int typeF, int typeO, int resource)
        {
            // chance out of 1000
            int resourceSparce = resource;
			int tundraAmount = typeT;
            int forestAmount = typeF;
            int mountainAmount = typeM;
            int plainAmount = typeP;
            int dreadlandAmount = typeDrd;
            int desertAmount = typeDst;
            int oceanAmount = typeO;
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
                    bio[p].type = 'T';//Tundra
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (forestAmount == 0) { }
              else
              {
                    bio[p].type = 'F';//Forest
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (mountainAmount == 0) { }
              else
              {
                    bio[p].type = 'M';//Mountain
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (plainAmount == 0) { }
              else
              {
                    bio[p].type = '+';//Plain
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (dreadlandAmount == 0) { }
              else
              {
                    bio[p].type = '%';//Dreadland
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (desertAmount == 0) { }
              else
              {
                    bio[p].type = 'D';//Desert
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (oceanAmount == 0) { }
              else
              {
                    bio[p].type = '~'; //Ocean
                    bio[p].x = r.Next(0, width);
                    bio[p].y = r.Next(0, height);
                    p++;
              }

              //checks for DEFAULT/NORMAL biome value
              if (tundraAmount >= 2)
              {
                    bio[p].type = 'T';//Tundra
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (forestAmount >= 2)
              {
                    bio[p].type = 'F';//Forest
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (mountainAmount >= 2)
              {
                    bio[p].type = 'M';//Mountain
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (plainAmount >= 2)
              {
                    bio[p].type = '+';//Plain
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (dreadlandAmount >= 2)
              {
                    bio[p].type = '%';//Dreadland
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (desertAmount >= 2)
              {
                    bio[p].type = 'D';//Desert
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (oceanAmount >= 2)
              {
                    bio[p].type = '~'; //Ocean
                    bio[p].x = r.Next(0, width);
                    bio[p].y = r.Next(0, height);
                    p++;
                }
              //checks for HIGH biome value
              if (tundraAmount >= 3)
              {
                    bio[p].type = 'T';//Tundra
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (forestAmount >= 3)
              {
                    bio[p].type = 'F';//Forest
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (mountainAmount >= 3)
              {
                    bio[p].type = 'M';//Mountain
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (plainAmount >= 3)
              {
                    bio[p].type = '+';//Plain
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (dreadlandAmount >= 3)
              {
                    bio[p].type = '%';//Dreadland
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (desertAmount >= 3)
              {
                    bio[p].type = 'D';//Desert
                    bio[p].x = r.Next(10, RX);
                    bio[p].y = r.Next(10, RY);
                    p++;
                }
              if (oceanAmount >= 3)
              {
                    bio[p].type = '~'; //Ocean
                    bio[p].x = r.Next(0, width);
                    bio[p].y = r.Next(0, height);
                    p++;
                }
            }
			
            //Sets Border To Ocean Biome
            for (int k = 0; k < width; k++)
            {//goes through and sets top and bottom rows to Ocean biome.
                bio[NumBio + k * 2].type = '~';
                bio[NumBio + k * 2].x = k;
                bio[NumBio + k * 2].y = 0;

                bio[NumBio + k * 2 + 1].type = '~';
                bio[NumBio + k * 2 + 1].x = k;
                bio[NumBio + k * 2 + 1].y = height - 1;
            }
            for (int l = 0; l < height; l++)
            {//Sets Left and Right border to Ocean biome
                bio[NumBio + width + l * 2].type = '~';
                bio[NumBio + width + l * 2].x = 0;
                bio[NumBio + width + l * 2].y = l;

                bio[NumBio + width + l * 2 + 1].type = '~';
                bio[NumBio + width + l * 2 + 1].x = width - 1;
                bio[NumBio + width + l * 2 + 1].y = l;
            }

            char[,] field = new char[width, height];
            int[,] elevation = new int[width, height];
            // i = y
            // j = x
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    char nearest = '~';
                    int dist = 5000;
                    for (int z = 0; z < NumBio + Border; z++)
                    {
                        int Xdiff = bio[z].x - j;
                        int Ydiff = bio[z].y - i;
                        int Cdist = Xdiff * Xdiff + Ydiff * Ydiff;
                        if (Cdist < dist)
                        {
                            nearest = bio[z].type;
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
			for(int j=0;j<height;j++){
				for(int i=0;i<width;i++){
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
				int riveRX = r.Next(50,width - 50);
				int riveRY = r.Next(50,height - 50);
				if(elevation[riveRX,riveRY] < 25){
					riveRX = r.Next(50,width - 50);
					riveRY = r.Next(50,height - 50);
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
				field[riveRX,riveRY] = '-';//Designed for '-' character, but use ocean biome for now.
				for(int j=0;j<riverLength;j++){
					int minHeight = 100;
					if(lastDir != 2 &&elevation[riveRX-1,riveRY]<minHeight){
						minHeight = elevation[riveRX-1,riveRY];
						Direction=0;
					}
					if(lastDir != 3 &&elevation[riveRX,riveRY+1]<minHeight){
						minHeight = elevation[riveRX,riveRY+1];
						Direction=1;
					}
					if(lastDir != 0 &&elevation[riveRX+1,riveRY]<minHeight){
						minHeight = elevation[riveRX+1,riveRY];
						Direction=2;
					}
					if(lastDir != 1 &&elevation[riveRX,riveRY-1]<minHeight){
						minHeight = elevation[riveRX,riveRY-1];
                        Direction = 3;
					}
					switch(Direction){
						case 0:
							riveRX-=1;
							break;
						case 1:
							riveRY+=1;
							break;
						case 2:
							riveRX+=1;
							break;
						case 3:
							riveRY-=1;
							break;
						default:
							break;
					}
					lastDir = Direction;
					if(field[riveRX,riveRY] == '~')
						break;
					else
						field[riveRX,riveRY] = '-';
				}
			}
            //Adding Resources.
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (field[j, i] == 'M')
                    {
                        if (r.Next(0, MAX_CHANCE) <= GOLD_CHANCE)
                            field[j, i] = 'G';//inserts gold mine resource
                    }
                    else if (field[j, i] == 'F')
                    {
                        if (r.Next(0, MAX_CHANCE) <= IRON_CHANCE)
                            field[j, i] = 'I';//sawmill for iron
                    }
                    else if (field[j, i] == '%')
                    {
                        if (r.Next(0, MAX_CHANCE) <= MANA_CHANCE)
                            field[j, i] = '*';//magic crystal resource
                    }
					else if(r.Next(0,resourceSparce)<=1){
						if(field[j,i] != '~'){
							int resourceType = r.Next(0, 3);
                            if (resourceType == 0)
                                field[j, i] = 'L';
                            if (resourceType == 1)
                                field[j, i] = 'G';
                            if (resourceType == 2)
                                field[j, i] = '*';
						}
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
						//Cardinal Directions of Coast
						if(field[j-1,i]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i-1] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i+1]== ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='@';//Coast tile with land on north.
						}
						if(field[j+1,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='Q';//Coast tile with land on east.
						}
						if(field[j+1,i]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i-1] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i+1] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='&';//Coast tile with land on south.
						}
						if(field[j+1,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='#';//Coast tile with land on west.
						}
						
						//3 edged by land Ocean tile
						if(field[j-1,i]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i-1] != ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i+1]!= ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='V';//Coast tile triple with bay mouth at north
						}
						if(field[j+1,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i] != ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='v';//Coast tile triple with bay mouth at east
						}
						if(field[j+1,i]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i-1] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i+1] != ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='B';//Coast tile triple with bay mouth at south
						}
						if(field[j+1,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i] != ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='b';//Coast tile triple with bay mouth at west
						}
						
						//smooth corner Ocean tile
						if(field[j-1,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i+1] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i-1]== ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='$';//Smooth corner Coast tile with land on NE
						}
						if(field[j-1,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i+1] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i-1]== ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='!';//Smooth corner Coast tile with land on NW
						}
						if(field[j+1,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i-1] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i+1]== ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='(';//Smooth corner Coast tile with land on SW
						}
						if(field[j+1,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i-1] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i+1]== ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]=')';//Smooth corner Coast tile with land on SE
						}
						
						//dot corner Ocean tile
						if(field[j-1,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i]==('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i+1]==('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i+1] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i-1]== ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='C';//Smooth corner Coast tile with land on NE
						}
						if(field[j-1,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i]==('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i-1]==('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i+1] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i-1]== ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='c';//Smooth corner Coast tile with land on NW
						}
						if(field[j+1,i-1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i]==('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i-1]==('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i-1] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i+1]== ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='n';//Smooth corner Coast tile with land on SW
						}
						if(field[j+1,i+1]!=('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i]==('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j,i+1]==('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j+1,i-1] == ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n') && field[j-1,i+1]== ('~'||'@'||'Q'||'&'||'#'||'$'||'('||')'||'!'||'B'||'b'||'V'||'v'||'C'||'c'||'N'||'n'))
						{
							field[j,i]='N';//Smooth corner Coast tile with land on SE
						}
					}
					//*****River Direction*****//
					if(field[j,i]=='-')
					{
						if(field[j-1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j+1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?'))
						{
							field[j,i]='^';//River running north to south.
						}
						if(field[j,i-1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j,i+1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?'))
						{
							field[j,i]=',';//River running east to west.
						}
						if(field[j+1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j,i+1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?'))
						{
							field[j,i]='<';//River mouths at east and south.
						}
						if(field[j+1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j,i-1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?'))
						{
							field[j,i]='>';//River mouths at west and south.
						}
						if(field[j-1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j,i-1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?'))
						{
							field[j,i]=']';//River mouths at west and north.
						}
						if(field[j-1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j,i+1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?'))
						{
							field[j,i]='[';//River mouths at east and north.
						}
						if(field[j,i-1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j,i+1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j-1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j+1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?')){
							field[j,i]='X'; // 4-way river
						}
						if(field[j,i-1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j,i+1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j-1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j+1,i]!=('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?')){
							field[j,i]='/'; // triple river land at south
						}
						if(field[j,i-1]!=('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j,i+1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j-1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j+1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?')){
							field[j,i]='|'; // triple river land at west
						}
						if(field[j,i-1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j,i+1]!=('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j-1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j+1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?')){
							field[j,i]='_'; // triple river land at east
						}
						if(field[j,i-1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j,i+1]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j-1,i]!=('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?') && field[j+1,i]==('-'||'^'||','||'<'||'>'||'['||']'||'X'||'/'||'|'||'_'||'?')){
							field[j,i]='?'; // triple river land at north
						}
					}
				}
			}
            return field;
        }
    }
}
