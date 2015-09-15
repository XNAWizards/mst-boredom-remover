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
    class World
    {
        public string genText = "";
        int passesCompleted = 0;
        int[, ,] level = new int[Global.SIZE_X, Global.SIZE_Y, 2];
        Player player;
        Area area;
        bool forceSave = false;

        // index 0 = sky
        // 1 = dirt1
        // 2 = dirt2
        // 3 = dirt3
        // 4 = stone1
        // 5 = stone2
        // 6 = stone3

        //List<Texture2D> textures = new List<Texture2D>();

        public World(Player player, Area area)
        {
            this.player = player;
            this.area = area;// GenerateWorld(genPasses);
        }
        public void GenerateWorld()
        {
            Random random = new Random();
            switch (passesCompleted)
            {
                case 0:
                    genPass1(random);
                    passesCompleted++;
                    break;
                case 1:
                    genPass2(random);
                    passesCompleted++;
                    break;
                case 2:
                    genPass3(random);
                    passesCompleted++;
                    break;
                case 3:
                    genPass4(random);
                    passesCompleted++;
                    break;
                case 4:
                    int save = 0;
                    bool[] worlds = Global.CheckWorlds();
                    for (int x = 0; x < worlds.Length; x++)
                    {
                        if (worlds[x])
                        {

                        }
                        else
                        {
                            save = x + 1;
                            break;
                        }
                    }
                    if (save != 0)
                    {
                        Save(save);
                    }
                    save = 0;
                    passesCompleted = 0;
                    Global.screen = 0;
                    break;
            }
        }
        public void AddTexture(Texture2D texture)
        {
            //textures.Add(texture);
        }
        public void Save(int worldNum, int situation = 0)
        {
            if (situation == 1)
            {
                level = Global.world;
                forceSave = true;
            }
            FileStream fs = new FileStream(Global.saveLoc + "World" + worldNum.ToString() + ".wld", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            for (int fsY = 0; fsY < Global.SIZE_Y; fsY++)
            {
                for (int fsX = 0; fsX < Global.SIZE_X; fsX++)
                {
                    if (fsX != Global.SIZE_X - 1)
                    {
                        sw.Write(level[fsX, fsY, 0] + ",");
                    }
                    else
                    {
                        sw.Write(level[fsX, fsY, 0]);
                    }
                }
                sw.WriteLine();
            }
            sw.WriteLine("---");
            // write all sorts of information
            // hp
            // difference.x
            // difference.y
            // smalldifference.x
            // smalldifference.y
            // inventory array
            try
            {
                sw.WriteLine(player.hitPoints);
            }
            catch
            {
                if (!forceSave)
                {
                    int temp = -1;
                    sw.WriteLine(temp);

                    sw.Close();
                    fs.Close();
                    return;
                }
                forceSave = false;
            }
            if (situation == 0)
            {
                Vector2 spawn = CalculateSpawn();

                spawn.X -= 30;
                spawn.Y -= 26;

                sw.WriteLine(spawn.X);
                sw.WriteLine(spawn.Y);
            }
            else
            {
                sw.WriteLine(area.difference.X);
                sw.WriteLine(area.difference.Y);
            }

            sw.WriteLine(area.smallDifference.X);
            sw.WriteLine(area.smallDifference.Y);

            for (int fsY = 0; fsY < 2; fsY++)
            {
                for (int fsX = 0; fsX < 18; fsX++)
                {
                    if (fsX != 18 - 1)
                    {
                        sw.Write(player.Inv[fsX, fsY] + ",");
                    }
                    else
                    {
                        sw.Write(player.Inv[fsX, fsY]);
                    }
                }
                sw.WriteLine();
            }

            sw.Close();
            fs.Close();
        }
        public Vector2 CalculateSpawn()
        {
            for (int y = 0; y < 1200; y++)
            {
                if (level[Global.SIZE_X / 2, y, 0] >= 1)
                {
                    return new Vector2(Global.SIZE_X / 2, y);
                }
            }
            return new Vector2(0, 0);
        }
        public void Load(int worldNum)
        {
            Global.usingWorld = worldNum; // saves which world we're using

            FileStream fs = new FileStream(Global.saveLoc + "World" + worldNum.ToString() + ".wld", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            string sTemp = "";
            string sPerm = "";
            string[] tempArray;

            for (int y = 0; y < Global.SIZE_Y; y++)
            {
                sTemp = sr.ReadLine();
                tempArray = sTemp.Split(',');

                for (int x = 0; x < Global.SIZE_X; x++)
                {
                    level[x, y, 0] = Convert.ToInt32(tempArray[x]);
                }
            }

            Global.world = level;
            //level = new int[Global.SIZE_X, Global.SIZE_Y, 2];

            LoadPlayerData(fs, sr);

            sr.Close();
            fs.Close();
        }

        private void LoadPlayerData(FileStream fs, StreamReader sr)
        {
            sr.ReadLine(); // reads ---
            // hp
            // difference.x
            // difference.y
            // smalldifference.x
            // smalldifference.y
            // inventory array
            int HP = Convert.ToInt32(sr.ReadLine());
            if (HP != -1) // if there is data to be read
            {
                int[,] inventory = new int[18, 2];

                Vector2 difference = new Vector2(0, 0);
                Vector2 smallDifference = new Vector2(0, 0);

                difference.X = Convert.ToInt32((sr.ReadLine()));
                difference.Y = Convert.ToInt32((sr.ReadLine()));

                smallDifference.X = Convert.ToSingle((sr.ReadLine()));
                smallDifference.Y = Convert.ToSingle((sr.ReadLine()));

                string sTemp = "";
                string[] tempArray;

                for (int y = 0; y < 2; y++)
                {
                    sTemp = sr.ReadLine();
                    tempArray = sTemp.Split(',');

                    for (int x = 0; x < 18; x++)
                    {
                        inventory[x, y] = Convert.ToInt32(tempArray[x]);
                    }
                }

                player.LoadData(HP, inventory);
                area.LoadData(difference, smallDifference);
            }
        }
        // generates the base terrain
        public void genPass1(Random random)
        {
            int iMix = 0;
            genText = "Creating terrain";
            // loop through world
            for (int x = 0; x < Global.SIZE_X; x++)
            {
                for (int y = 0; y < Global.SIZE_Y; y++)
                {
                    // if the height is less than 300, add sky
                    if (y < 300)
                    {
                        level[x, y, 0] = 0;
                    }
                    // if height between 300 and 350, dirt
                    else if (y > 300 && y < 351)
                    {
                        level[x, y, 0] = random.Next(1, 4);
                    }
                    // if height between 350-440, Mixture of dirt and stone
                    #region Gradual
                    else if (y > 350 && y < 450)
                    {
                        iMix = y - 350;
                        if (iMix >= random.Next(1, 100))
                        {
                            level[x, y, 0] = random.Next(4, 7);
                        }
                        else
                        {
                            level[x, y, 0] = random.Next(1, 4);
                        }
                    }
                    #endregion
                    //If Y is greater then 440, Stone
                    else if (y > 449)
                    {
                        level[x, y, 0] = random.Next(4, 7);
                    }
                }
            }

        }

        // hills and valleys
        public void genPass2(Random random)
        {
            genText = "Adding hills";

            int x = 1;
            int y = 300;

            List<Vector2> highPoints = new List<Vector2>();
            List<Vector2> lowPoints = new List<Vector2>();

            for (x = 1; x < Global.SIZE_X - 1; x++)
            {
                if (random.Next(1, 10) <= 3)
                {
                    switch (random.Next(1, 4))
                    {
                        case 1://High Height
                            highPoints.Add(new Vector2(x, 300 - random.Next(1, 45)));
                            break;
                        case 2://Low height
                            lowPoints.Add(new Vector2(x, 300 + random.Next(1, 45)));
                            break;
                        case 3:
                            //Nothing happens

                            break;
                    }
                }
            }
            // creates hills and valleys
            #region High Points Loop
            foreach (Vector2 high in highPoints) // loops through each hill randomly generated
            {
                // creates a custer of temporarily saved vectors for use in the slope generation system
                Vector2 temp = high; // temporarilly saves the point needed to be reached
                if (temp.X < Global.SIZE_X)
                {
                    Vector2 temp2 = high;
                    Vector2 temp3 = high;

                    while (temp.Y < 301) // fill in the column below the high point with dirt
                    {
                        level[(int)temp.X, (int)temp.Y, 0] = random.Next(1, 4);
                        temp.Y++;
                    }
                    #region Right Side
                    List<Vector2> slopeHighsR = new List<Vector2>(); // right side of the hill
                    while (temp2.Y < 301)
                    {
                        if (temp2.X < Global.SIZE_X && temp2.X > 0f)
                        {
                            int slopeDown = random.Next(1, 5);
                            temp2.Y += slopeDown - 1;
                            slopeHighsR.Add(new Vector2(temp2.X, temp2.Y));
                            temp2.X++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    foreach (Vector2 rightHigh in slopeHighsR)
                    {
                        Vector2 rightHighTemp = rightHigh;
                        while (rightHighTemp.Y < 301) // fill in the column below the high point with dirt
                        {
                            level[(int)rightHighTemp.X, (int)rightHighTemp.Y, 0] = random.Next(1, 4);
                            rightHighTemp.Y++;
                        }
                    }
                    #endregion
                    #region Left Side
                    List<Vector2> slopeHighsL = new List<Vector2>(); // right side of the hill
                    while (temp3.Y < 301)
                    {
                        if (temp3.X < Global.SIZE_X && temp3.X > 0f)
                        {
                            int slopeDown = random.Next(1, 5);
                            temp3.Y += slopeDown - 1;
                            slopeHighsL.Add(new Vector2(temp3.X, temp3.Y));
                            temp3.X--;
                        }
                        else
                        {
                            break;
                        }
                    }
                    foreach (Vector2 leftHigh in slopeHighsL)
                    {
                        Vector2 leftHighTemp = leftHigh;
                        while (leftHighTemp.Y < 301) // fill in the column below the high point with dirt
                        {
                            level[(int)leftHighTemp.X, (int)leftHighTemp.Y, 0] = random.Next(1, 4);
                            leftHighTemp.Y++;
                        }
                    }
                    #endregion
                }
            } // foreach high point
            #endregion
            #region Low Points Loop
            foreach (Vector2 low in highPoints) // loops through each hill randomly generated
            {
                // creates a custer of temporarily saved vectors for use in the slope generation system
                Vector2 temp = low; // temporarilly saves the point needed to be reached
                if (temp.X < Global.SIZE_X)
                {
                    Vector2 temp2 = low;
                    Vector2 temp3 = low;

                    while (temp.Y > 299) // fill in the column below the high point with dirt
                    {
                        level[(int)temp.X, (int)temp.Y, 0] = 0;
                        temp.Y--;
                    }
                    #region Right Side
                    List<Vector2> slopeLowsR = new List<Vector2>(); // right side of the hill
                    while (temp2.Y > 299)
                    {
                        if (temp2.X < Global.SIZE_X && temp2.X > 0f)
                        {
                            int slopeUp = random.Next(1, 5);
                            temp2.Y -= slopeUp - 1;
                            slopeLowsR.Add(new Vector2(temp2.X, temp2.Y));
                            temp2.X++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    foreach (Vector2 rightLow in slopeLowsR)
                    {
                        Vector2 rightLowTemp = rightLow;
                        while (rightLowTemp.Y > 299) // fill in the column below the high point with air
                        {
                            level[(int)rightLowTemp.X, (int)rightLowTemp.Y, 0] = 0;
                            rightLowTemp.Y--;
                        }
                    }
                    #endregion
                    #region Left Side
                    List<Vector2> slopeLowsL = new List<Vector2>(); // right side of the hill
                    while (temp3.Y > 299)
                    {
                        if (temp3.X < Global.SIZE_X && temp3.X > 0f)
                        {
                            int slopeUp = random.Next(1, 5);
                            temp3.Y -= slopeUp - 1;
                            slopeLowsL.Add(new Vector2(temp3.X, temp3.Y));
                            temp3.X--;
                        }
                        else
                        {
                            break;
                        }
                    }
                    foreach (Vector2 leftLow in slopeLowsL)
                    {
                        Vector2 leftLowTemp = leftLow;
                        while (leftLowTemp.Y > 299) // fill in the column below the high point with air
                        {
                            level[(int)leftLowTemp.X, (int)leftLowTemp.Y, 0] = 0;
                            leftLowTemp.Y++;
                        }
                    }
                    #endregion
                }
            } // foreach low point
            #endregion // creates


        }
        // ore and trees
        public void genPass3(Random random)
        {


            genText = "Dropping ore";
            // ore
            for (int y = 450; y < Global.SIZE_Y; y++)
            {
                for (int x = 0; x < Global.SIZE_X; x++)
                {
                    // (Chance/10000, map index, random, x, y)
                    GenerateOres(32, random.Next(1, 4), random, x, y); // Dirt patches
                    GenerateOres(160, 7, random, x, y); // copper
                    GenerateOres(80, 8, random, x, y); // iron
                    GenerateOres(40, 9, random, x, y); // Silver
                    GenerateOres(20, 10, random, x, y); // Gold
                }
            }




        }

        public void GenerateTrees(Random random, List<Vector2> Trees)
        {
            foreach (Vector2 tree in Trees)
            {
                for (int x = 0; x <= 1; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        level[(int)tree.X + x, (int)tree.Y - y, 0] = 11;
                    }
                }
            }
        }
        // caves
        //Cave Generation Change START
        public void genPass4(Random random)
        {

            List<Vector2> Trees = new List<Vector2>();
            genText = "Digging caves";
            int x = 0;
            int y = 0;
            int LastMove = 0;

            int[,] CaveBrush = new int[10, 10];
            for (y = 0; y < Global.SIZE_Y; y++)
            {
                for (x = 0; x < Global.SIZE_X; x++)
                {
                    if (level[x, y, 0] >= 1 && level[x, y, 0] <= 6)
                    {
                        if (random.Next(1, 10000) <= 3)//Chance of a cave generating
                        {
                            #region FirstPass
                            CaveBrush = new int[,]
                            {
                                {1, 1, 1, 0, 1, 1, 1, 0, 0, 0},
                                {0, 1, 1, 1, 1, 1, 1, 0, 1, 0},
                                {0, 0, 1, 1, 1, 1, 1, 1, 1, 1},
                                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                {1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
                                {0, 0, 1, 1, 1, 1, 1, 1, 0, 0},
                                {0, 0, 0, 1, 1, 1, 1, 1, 0, 0},
                                {0, 0, 0, 1, 1, 1, 1, 0, 0, 0},
                            };
                            for (int y2 = 0; y2 < 10; y2++)
                            {
                                for (int x2 = 0; x2 < 10; x2++)
                                {
                                    if (random.Next(1, 100) <= 16)
                                    {
                                        if (CaveBrush[x2, y2] == 0)
                                        {
                                            CaveBrush[x2, y2] = 1;
                                        }
                                        else
                                        {
                                            CaveBrush[x2, y2] = 0;
                                        }
                                    }
                                }
                            }

                            for (int y2 = 9; y2 > 0; y2--)
                            {
                                for (int x2 = 9; x2 > 0; x2--)
                                {
                                    if (CaveBrush[x2, y2] == 1)
                                    {
                                        try
                                        {
                                            level[x - x2, y - y2, 0] = 0;
                                        }
                                        catch // off the edge of the world
                                        {

                                        }
                                    }
                                }
                            }
                            #endregion
                            int Chance = 1;
                            int CaveCordX = 0;
                            int CaveCordY = 0;
                            CaveCordX = x + CaveCordX;
                            CaveCordY = y + CaveCordY;
                            while (Chance <= random.Next(1, 200))//Random Decreasing chance.
                            {


                                switch (random.Next(1, 7))
                                {
                                    //Left And Up must go 11
                                    case 1://Up
                                        CaveCordY -= random.Next(9, 10);
                                        break;
                                    case 2://Right
                                        if (LastMove != 2)
                                        {
                                            CaveCordX += random.Next(1, 6);
                                            LastMove = 1;
                                        }
                                        else
                                        {

                                        }
                                        break;
                                    case 3://Right
                                        if (LastMove != 2)
                                        {
                                            CaveCordX += random.Next(1, 6);
                                            LastMove = 1;
                                        }
                                        else
                                        {

                                        }
                                        break;
                                    case 4://left
                                        if (LastMove != 1)
                                        {
                                            CaveCordX -= random.Next(9, 10);
                                            LastMove = 2;
                                        }
                                        else
                                        {

                                        }
                                        break;
                                    case 5://Left
                                        if (LastMove != 1)
                                        {
                                            CaveCordX -= random.Next(9, 10);
                                            LastMove = 2;
                                        }
                                        else
                                        {

                                        }
                                        break;
                                    case 6://Down
                                        CaveCordY += random.Next(1, 6);
                                        break;
                                }
                                #region Random cave gen
                                CaveBrush = new int[,]
                            {
                                {0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
                                {0, 0, 0, 1, 1, 1, 1, 0, 0, 0},
                                {0, 0, 1, 1, 1, 1, 1, 1, 0, 0},
                                {1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
                                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                {1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
                                {0, 0, 1, 1, 1, 1, 1, 1, 0, 0},
                                {0, 0, 0, 1, 1, 1, 1, 1, 0, 0},
                                {0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
                            };
                                for (int y2 = 0; y2 < 10; y2++)
                                {
                                    for (int x2 = 0; x2 < 10; x2++)
                                    {
                                        if (x2 == 0 || x2 == 9)
                                        {
                                            CaveBrush[x2, y2] = 1;
                                        }
                                        if (random.Next(1, 100) <= 16)
                                        {
                                            if (CaveBrush[x2, y2] == 0)
                                            {
                                                CaveBrush[x2, y2] = 1;
                                            }
                                            else
                                            {
                                                CaveBrush[x2, y2] = 0;
                                            }

                                        }
                                    }
                                }
                                if (CaveCordX <= 1)
                                {
                                    CaveCordX = 1;
                                }
                                if (CaveCordX >= Global.SIZE_X)
                                {
                                    CaveCordX = Global.SIZE_X - 1;
                                }
                                if (CaveCordY <= 1)
                                {
                                    CaveCordY = 1;
                                }
                                if (CaveCordY >= Global.SIZE_Y)
                                {
                                    CaveCordY = Global.SIZE_Y - 1;
                                }
                                //if (level[CaveCordX, CaveCordY, 0] >= 1 && level[CaveCordX, CaveCordY, 0] <= 6)
                                //{
                                for (int y2 = 9; y2 > 0; y2--)
                                {
                                    for (int x2 = 9; x2 > 0; x2--)
                                    {
                                        if (CaveBrush[x2, y2] == 1)
                                        {
                                            try
                                            {
                                                level[CaveCordX - x2, CaveCordY - y2, 0] = 0;
                                            }
                                            catch // off the edge of the world
                                            {

                                            }
                                        }
                                    }
                                }
                                Chance++;
                                //}
                                //else
                                //{
                                //    break;
                                //}
                            }//End of while
                                #endregion
                        } // if random.next < 10000
                    } // if
                } // for x
            } // for y

            #region Artifact Deleter
            //Start Artifact Deleter
            // aka pile o' if's
            for (int y3 = 1; y3 < Global.SIZE_Y - 1; y3++)
            {
                for (int x3 = 1; x3 < Global.SIZE_X - 1; x3++)
                {
                    if (level[x3, y3, 0] != 0)
                    {
                        if (level[x3 - 1, y3, 0] == 0)
                        {
                            if (level[x3 + 1, y3, 0] == 0)
                            {
                                if (level[x3, y3 + 1, 0] == 0)
                                {
                                    if (level[x3, y3 - 1, 0] == 0)
                                    {
                                        level[x3, y3, 0] = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //End Artifact Deleter
            #endregion

            // pile of tree related if statements
            for (int y2 = 250; y2 < 350; y2++)
            {
                for (int x2 = 0; x2 < Global.SIZE_X - 3; x2++)
                {
                    if (level[x2, y2, 0] == 1 || level[x2, y2, 0] == 2 || level[x2, y2, 0] == 3)//If dirt
                    {
                        if (level[x2 + 1, y2, 0] == 1 || level[x2 + 1, y2, 0] == 2 || level[x2 + 1, y2, 0] == 3)//If dirt
                        {
                            if (level[x2, y2 - 1, 0] == 0 && level[x2 + 1, y2 - 1, 0] == 0)
                            {
                                if (random.Next(1, 100) < 25)
                                {
                                    Trees.Add(new Vector2(x2, y2 - 1));
                                    x2 += 3;
                                }
                            }
                        }

                    }
                }
            }
            GenerateTrees(random, Trees);
            GenerateLeaves(random, Trees);
        }// pass 4

        public void GenerateLeaves(Random random, List<Vector2> Trees)
        {

            int[,] LeaveBrush = new int[8, 8];
            LeaveBrush = new int[,]
            {
                {0, 0, 0, 0, 1, 1, 0, 0},
                {0, 0, 0, 1, 1, 1, 1, 0},
                {0, 0, 1, 1, 1, 1, 1, 1},
                {0, 1, 1, 1, 1, 1, 1, 0},
                {0, 1, 1, 1, 1, 1, 1, 0},
                {0, 0, 1, 1, 1, 1, 1, 1},
                {0, 0, 0, 1, 1, 1, 1, 0},
                {0, 0, 0, 0, 1, 1, 0, 0},
            };
            foreach (Vector2 tree in Trees)
            {
                Vector2 topOfTrunk = new Vector2(tree.X, tree.Y - 9);
                Vector2 leafOrigin = new Vector2(topOfTrunk.X - 3, topOfTrunk.Y - 7);
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (leafOrigin.X + x < Global.SIZE_X && leafOrigin.Y + y < Global.SIZE_Y)
                        {
                            if (leafOrigin.X + x > 0 && leafOrigin.Y + y > 0)
                            {
                                if (LeaveBrush[x, y] == 1)
                                {
                                    level[(int)leafOrigin.X + x, (int)leafOrigin.Y + y, 0] = 12;
                                }
                            }
                        }
                    }

                }
            }
        }
        public void Update(Player player, Area area)
        {
            this.player = player;
            this.area = area;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            switch (Global.screen)
            {
                case -1: // world creation
                    spriteBatch.DrawString(Global.font, genText, Global.fontLocation, Color.Black);
                    break;
                case 1: // world selection
                    for (int y = (int)((area.difference.Y) - 75f) + 26; y < (int)((area.difference.Y) + 75f) + 26; y++)
                    {
                        for (int x = (int)((area.difference.X) - 100f) + 30; x < (int)((area.difference.X) + 100f) + 30; x++)
                        {
                            try
                            {
                                if (Global.world[x, y, 0] == 0)
                                {
                                    if (y < 450)
                                    {
                                        spriteBatch.Draw(area.backgrounds[0], new Rectangle((4 * 100) + ((int)(x - area.difference.X - 30) * 4), (4 * 75) + (int)(y - area.difference.Y - 26) * 4, 4, 4), Color.White);
                                    }
                                    else if (y >= 450)
                                    {
                                        spriteBatch.Draw(area.backgrounds[1], new Rectangle((4 * 100) + ((int)(x - area.difference.X - 30) * 4), (4 * 75) + (int)(y - area.difference.Y - 26) * 4, 4, 4), Color.White);
                                    }
                                }
                                spriteBatch.Draw(area.textures[Global.world[x, y, 0]], new Rectangle((4 * 100) + ((int)(x - area.difference.X - 30) * 4), (4 * 75) + (int)(y - area.difference.Y - 26) * 4, 4, 4), Color.White);
                            }
                            catch
                            {
                                spriteBatch.Draw(area.blank, new Rectangle((4 * 100) + ((int)(x - area.difference.X - 30) * 4), (4 * 75) + (int)(y - area.difference.Y - 26) * 4, 4, 4), Color.White);
                            }
                        }
                    }
                    break;
            }
        }
        public void GenerateOres(int Chance, int Ore, Random random, int x, int y)
        {
            if (random.Next(1, 100000) <= Chance)//Iron
            {
                bool[,] shape = Brush(random);

                for (int y2 = 3; y2 > 0; y2--)
                {
                    for (int x2 = 3; x2 > 0; x2--)
                    {
                        if (shape[x2, y2])
                        {
                            try
                            {
                                level[(x - x2), (y - y2), 0] = Ore;
                            }
                            catch // off the edge of the world
                            {

                            }
                        }
                    }
                }
            }

        }
        public bool[,] Brush(Random random)
        {
            //Brush Shapes, Circle, Messed up square.
            bool[,] shape = new bool[4, 4];
            switch (random.Next(1, 4))
            {
                case 1:
                    shape = new bool[,]
                    {
                        {false, true, true, false},
                        {false, false, true, false},
                        {false, false, true, true},
                        {false, false, false, true},
                    };
                    break;
                case 2:
                    shape = new bool[,]
                    {
                        {false, false, false, false},
                        {false, true, true, false},
                        {true, true, true, false},
                        {false, true, false, false},
                    };
                    break;
                case 3:
                    shape = new bool[,]
                    {
                        {true, true, true, false},
                        {true, true, true, true},
                        {false, true, true, false},
                        {false, false, false, false},
                    };
                    break;
            } // sswitch
            switch (random.Next(1, 5))
            {
                case 1: // rotate 180 degrees
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            shape[x, y] = shape[y, x];
                        }
                    }
                    break;
                case 2: // left
                    for (int y = 3; y >= 0; y--)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            shape[y, 3 - x] = shape[x, y];
                        }
                    }
                    break;
                case 3: // right
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 3; x >= 0; x--)
                        {
                            shape[x, 3 - y] = shape[x, y];
                        }
                    }
                    break;
                case 4: // normal
                    // do nothing to it
                    break;
            }

            PlaceOres(shape, random, false, random.Next(3, 10));
            return shape;
        }
        public void PlaceOres(bool[,] shape, Random random, bool second, int numberOfOres)
        {

            if (second)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (!shape[x, y])
                        {
                            if (random.Next(1, 100) < 50)
                            {
                                shape[x, y] = true;
                                numberOfOres--;
                            }
                        }
                    } // for
                } // for
                if (numberOfOres <= 0)
                {
                    return;
                } // if
                else
                {
                    PlaceOres(shape, random, true, numberOfOres);
                }

            }
            else // not second pass
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (shape[x, y])
                        {
                            numberOfOres--;
                        }
                    } // for
                } // for
                if (numberOfOres <= 0)
                {
                    return;
                } // if
                else
                {
                    PlaceOres(shape, random, true, numberOfOres);
                }
            }

        } // PlaceOres()
    } // class
} // end of the world
