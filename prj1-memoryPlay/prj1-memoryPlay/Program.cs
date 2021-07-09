using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace prj1_memoryPlay
{
    class Program
    {


        static void Main(string[] args)
        {

            string[] usernames = inputUserNames();
            int sizeIndx;
            do
            {
                Console.WriteLine("   Choose play desk size 1  (2x2)\n" +
                                  "                         2  (4x4)\n" +
                                  "                         3  (6x6)\n" +
                                  "                         4  (8x8) ");
                sizeIndx = int.Parse(Console.ReadLine());


            }
            while ((sizeIndx > 4) || (sizeIndx < 0));

            #region Vars_and_arrays_initialization


            int plDeskSize = 2;
            int userNum = 2;
            int[] loc1;
            int[] loc2;
            int cardsNotInUseNum = 10;


            switch (sizeIndx)
            {
                case 1:
                    plDeskSize = 2;
                    break;
                case 2:
                    plDeskSize = 4;
                    break;
                case 3:
                    plDeskSize = 6;
                    break;
                case 4:
                    plDeskSize = 8;
                    break;

            }
            string[,] cards = PlayDeskGenerator(plDeskSize);
            int[] scors = new int[userNum];

            int[,] stat = new int[plDeskSize, plDeskSize];  //play desk status array. is applyed as mask for for cards view.
            for (int i = 0; i < stat.GetLength(0); i++)
            {
                for (int j = 0; j < stat.GetLength(1); j++)
                {
                    stat[i, j] = -1;
                }
            }
            #endregion

            int usnmCounter = 0;
            bool plDeskIsNotEmpty = true;

            while (plDeskIsNotEmpty)
            {
                ViewPlayDesk(cards, stat);
                PrintScors(usernames, scors);
                loc1 = GetLocation(usernames[usnmCounter], stat);
                SetStat("open", loc1, stat);
                ViewPlayDesk(cards, stat);
                PrintScors(usernames, scors);
                loc2 = GetLocation(usernames[usnmCounter], stat);
                SetStat("open", loc2, stat);
                ViewPlayDesk(cards, stat); PrintScors(usernames, scors);
                Thread.Sleep(2500);
                if (cards[loc1[0], loc1[1]] == cards[loc2[0], loc2[1]])
                {
                    scors[usnmCounter]++;
                    SetStat("signUsed", loc1, stat);
                    SetStat("signUsed", loc2, stat);
                    ViewPlayDesk(cards, stat); PrintScors(usernames, scors);
                }
                else
                {
                    SetStat("close", loc1, stat);
                    SetStat("close", loc2, stat);
                    ViewPlayDesk(cards, stat); PrintScors(usernames, scors);
                    usnmCounter++;
                    if (usnmCounter == userNum)
                    {
                        usnmCounter = 0;
                    }
                }

                plDeskIsNotEmpty = CheckPlayDeskStatus(stat);
            }
            Console.Clear();
            Console.WriteLine("\n\n\n\n\n");
            Console.WriteLine("                                 THE GAME IS OVER !");
            Console.WriteLine("\n\n\n\n\n");
            PrintScors(usernames, scors);
            Console.ReadLine();





            //  loc = GetLocation();



            //        Console.WriteLine("location : {0},{1} - elem {2}", loc[0],loc[1], cards[loc[0],loc[1] ]);
        }
        //#########################################################################################


        //############################  PlayDeskGenerator  #########################################################     
        public static string[,] PlayDeskGenerator(int size)
        {
            string card;
            string[,] mtrx = new string[size, size];

            int[] loc;
            string[] unicalCards = new string[size * size / 2];


            // generate new cards and write to matrix

            for (int i = 0; i < size * size / 2; i++)
            {

                loc = FindNotUsedLocation(mtrx);
                card = RandomCard();
                mtrx[loc[0], loc[1]] = card;
                unicalCards[i] = card;

            }
            // write the pairs to matrix
            for (int i = 0; i < size * size / 2; i++)
            {

                loc = FindNotUsedLocation(mtrx);
                card = unicalCards[i];
                if (loc[0] == -1) { break; }
                mtrx[loc[0], loc[1]] = card;


            }




            return mtrx;
        }

        //####################  ViewPlayDesk   ####################################################################
        /// <summary>
        /// prints play Desk 
        /// cards contain card picture 
        /// playStatus contain mask -1  - keep card closed
        ///                          0  card is already used
        ///                          1  open card for view
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="playStatus"></param>
        public static void ViewPlayDesk(string[,] cards, int[,] playStatus)
        {
            Console.Clear();
            Console.WriteLine("\n\n\t\t\t\t\t\t   MEMORY   PLAY");
            Console.WriteLine();
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                Console.Write("\t\t\t\t\t\t");
                for (int j = 0; j < cards.GetLength(1); j++)
                {
                    switch (playStatus[i, j])
                    {
                        case -1:
                            Console.Write("   " + "XX");
                            break;
                        case 0:
                            Console.Write("   " + "--");
                            break;
                        case 1:
                            Console.Write("   " + cards[i, j]);
                            break;


                    }

                }

                Console.WriteLine("\n\n");
            }


        }
        //#######################   GetLocation   ##############################################################
        public static int[] GetLocation(string username, int[,] stat)
        {
            int[] loc = new int[2];
            do

            {
                Console.Write("       Type row 'point' column number (for example row 1 col 3 type 1.3) \n      {0} , now it's your turn to enter: ", username);
                float rc = float.Parse(Console.ReadLine());
                loc[0] = (int)rc - 1;
                loc[1] = (int)((rc * 10.001F) % 10 - 1);
                if (stat[loc[0], loc[1]] == 0)  // CHECK IF LOCATION IS NOT VALID
                {
                    Console.WriteLine(" ####      This location is empty!   ####");
                    Thread.Sleep(1000);
                }
            }

            while (stat[loc[0], loc[1]] == 0);
            return loc;
        }
        //########################   inputUserNames   ##########################################################3
        public static string[] inputUserNames()
        {


            Console.Write(" ####    Memory game   ####\n\n    How many players will play now (2-4):");
            int unum = int.Parse(Console.ReadLine());
            string[] unames = new string[unum];
            string name;

            for (int i = 0; i < unum; i++)
            {
                Console.Write(" \n\n   enter player {0} name : ", i + 1);
                name = Console.ReadLine();
                unames[i] = name;
            }

            return unames;
        }



        //#########################   SetStat   ################################################################
        /// <summary>
        /// set mask  array (for play desk view) by card location
        ///
        /// operation ="close"|"open"|"signUsed"
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="locInStat"></param>
        /// <param name="status"></param>
        public static void SetStat(string operation, int[] locInStat, int[,] status)
        {
            int i = locInStat[0];
            int j = locInStat[1];
            switch (operation)
            {
                case "close":
                    status[i, j] = -1;
                    break;
                case "open":
                    status[i, j] = 1;
                    break;
                case "signUsed":
                    status[i, j] = 0;
                    break;

            }


        }
        //########################  PrintScors  ######################################################
        public static void PrintScors(string[] usernames, int[] scors)
        {
            Console.WriteLine("\n\t\t\t\t\t\tSCORES:");

            for (int i = 0; i < scors.Length; i++)
            {
                Console.WriteLine("\t\t\t\t\t\t" + usernames[i] + ":" + scors[i]);

            }

        }

        //##########################   RandomCard   ########################################################
        /// <summary>
        /// returns a random string of 2 letters: "23","Ta"," 9" etc
        /// </summary>
        /// <returns></returns>
        public static string RandomCard()
        {
            string card;
            string[] letter1 = { "Z", "T", "V", "B", "N", "M", "L", "G", "S", "R" };
            string[] letter2 = { "a", "e", "u", "i", "o" };
            string[] num1 = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] num2 = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            Thread.Sleep(25);
            Random rnd = new Random();
            int rnum = rnd.Next(0, 100);
            if (rnum < 10)
            {
                card = " " + num2[rnum];
            }
            else if (rnum % 3 == 0)
            {
                card = "" + rnum;
            }
            else
            {
                int k = rnd.Next(0, 10);
                int m = rnd.Next(0, 5);
                card = letter1[k] + letter2[m];
            }


            return card;
        }

        //##########################   FindNotUsedLocation  ###################################
        /// <summary>
        /// searches for empty location in string matrix
        /// returns int array [row,col] of location
        /// if empty was not found returns [-1,-1] 
        /// </summary>
        /// <param name="mtr"></param>
        /// <param name="fromRow"></param>
        /// <param name="fromCol"></param>
        /// <returns></returns>
        public static int[] FindNotUsedLocation(string[,] mtr)
        {
            Thread.Sleep(25);
            int[] loc = new int[2];
            Random rn = new Random();
            for (int i = 0; i < 20; i++)  //looking for a random location 20 times
            {
                int k = rn.Next(0, mtr.GetLength(0));
                int m = rn.Next(0, mtr.GetLength(0));

                if (string.IsNullOrEmpty(mtr[k, m]))
                {
                    loc[0] = k;
                    loc[1] = m;
                    return loc;
                }

            }

            for (int i = 0; i < mtr.GetLength(0); i++) //looking for any empty location
            {
                for (int j = 0; j < mtr.GetLength(1); j++)
                {
                    if (string.IsNullOrEmpty(mtr[i, j]))
                    {
                        loc[0] = i;
                        loc[1] = j;
                        return loc;
                    }
                }
            }
            int[] loc1 = { -1, -1 };
            return loc1;
        }

        //###########################    CheckPlayDeskStatus    #################################
        public static bool CheckPlayDeskStatus(int[,] stat)
        {
            for (int i = 0; i < stat.GetLength(0); i++)
            {
                for (int j = 0; j < stat.GetLength(1); j++)
                {
                    if (stat[i, j] == -1) { return true; } //there are some closed cards on the desk yet
                }
            }
            return false;
        }



    }
}
