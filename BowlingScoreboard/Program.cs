using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreboard
{
    class Program
    {
        //Declaring globals*****NOTE ONLY USE GLOBALS WHEN YOU MUST**********
        private static readonly int Spare = 1, Strike = 2;
        private static bool isValid = false, isStrike = false, isSpare = false;

        static void Main(string[] args)
        {
            int[,] scoreBoard = new int[2, 10];//Set array for scorboard

            //Declaring variables
            int rollsLeft; Console.ForegroundColor = ConsoleColor.Cyan;

            for (int i = 0; i <= 9; i++)//Loop for 10 score frames
            {
                int rollOne = 0; int rollTwo = 0;//Declaring variables to use in this for loop.

                do
                {
                    Console.WriteLine("Enter first roll.");
                    isValid = int.TryParse(Console.ReadLine(), out rollOne);//Returns true if the parse was successful and overwrites rollOne.
                    if (isValid)
                    {

                        if (IsThisScoreValid(rollOne, 0))
                        {
                            isStrike = IsThisStrike(rollOne);//Runs method to determain Strike.
                            if (!isStrike)
                            {
                                Console.WriteLine("Enter second roll.");
                                isValid = int.TryParse(Console.ReadLine(), out rollTwo);//Returns true if the parse was successful and overwrites rollTwo.

                                if (isValid)
                                {
                                    if (IsThisScoreValid(rollOne, rollTwo))
                                    {
                                        isSpare = IsThisSpare(rollOne, rollTwo);//Runs method to determain Spare.
                                    }
                                    else
                                    {
                                        Console.WriteLine("Score not possible please try again");//Error Message.
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Please enter valid roll!");//Error Message.
                                    isValid = false;
                                }
                            }
                            else
                            {
                                rollTwo = 0;//Strike so rollTwo has to equal 0 by default.
                            }
                        }
                        else
                        {
                            Console.WriteLine("Score not possible please try again");//Error Message.
                            isValid = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter valid roll!");//Error Message.
                        isValid = false;
                    }

                } while (!isValid);

                scoreBoard = UpdateScoreboard(rollOne, rollTwo, scoreBoard, i);//Updating scoreboard.

                Console.Clear();
                Console.WriteLine("Current Frame: " + (i + 1) + " |" + " Score: " + scoreBoard[0, i]);//Displays current frame and score for the user.

                //Call further bonus
                if (i == 9 && isStrike)
                {
                    rollsLeft = 2;
                    scoreBoard = BonusRound(rollsLeft, scoreBoard);//Updates scoreboard after running BonusRound method.
                }
                else if (i == 9 && isSpare)
                {
                    rollsLeft = 1;
                    BonusRound(rollsLeft, scoreBoard);//Updates scoreboard after running BonusRound method.
                }
            }
            //------------Print Final Scoreboard :-)
            Console.Clear(); Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine("\n Your final score:\n");
            Console.WriteLine(" "); Console.ForegroundColor = ConsoleColor.Green;

            if (scoreBoard[0, 9] == 300)//Check if was a perfect game.
            {
                Console.WriteLine("\n \t Congratulations! Your power level is over 9,000!!!\n");//Congratulate perfect game!
                Console.WriteLine(" Score: " + " | " + scoreBoard[0, 0] + " | " + scoreBoard[0, 1] + " | " + scoreBoard[0, 2] + " | " + scoreBoard[0, 3] + " | " + scoreBoard[0, 4] + " | " + scoreBoard[0, 5] + " | " + scoreBoard[0, 6] + " | " + scoreBoard[0, 7] + " | " + scoreBoard[0, 8] + " | " + scoreBoard[0, 9] + " | ");
            }
            else//Prints normal scoreboard.
            {
                Console.WriteLine(" Score: " + " | " + scoreBoard[0, 0] + " | " + scoreBoard[0, 1] + " | " + scoreBoard[0, 2] + " | " + scoreBoard[0, 3] + " | " + scoreBoard[0, 4] + " | " + scoreBoard[0, 5] + " | " + scoreBoard[0, 6] + " | " + scoreBoard[0, 7] + " | " + scoreBoard[0, 8] + " | " + scoreBoard[0, 9] + " | ");

            }
            Console.WriteLine("\n\t\t\t Press any key to exit.");
            Console.ReadLine();
        }

        //Method called to update current score and flags.
        static int[,] UpdateScoreboard(int rollOne, int rollTwo, int[,] scoreboard, int frame)
        {

            if (frame != 0)//Checking flag to determaine bonus points
            {
                scoreboard[0, frame] = scoreboard[0, frame - 1] + rollOne + rollTwo;//Assigning bonus points for Strike and Spare.
            }
            else
            {
                scoreboard[0, frame] = rollOne + rollTwo;//Assigning normal roll points.
            }

            //Assigning flag for current frame.
            if (rollOne == 10)
            {
                scoreboard[1, frame] = Strike; //Strike == 2
            }
            else if (rollOne + rollTwo == 10)
            {
                scoreboard[1, frame] = Spare;  //Spare == 1
            }
            else
            {
                scoreboard[1, frame] = 0;  //Normal frame == 0
            }

            scoreboard = AddingBonuses(rollOne, rollTwo, scoreboard, frame);//Updating scoreboard.

            return scoreboard;
        }

        //Checks if the parameter rolls make a valid score.
        static bool IsThisScoreValid(int rollOne, int rollTwo)
        {
            if (rollOne + rollTwo >= 0 && rollOne + rollTwo <= 10 && rollOne <= 10 && rollTwo <= 10 && rollOne >= 0 && rollTwo >= 0)
            {
                return isValid = true;
            }
            else
            {
                return isValid = false;
            }

        }

        //Checks if the passed roll was a Strike.
        static bool IsThisStrike(int rollOne)
        {
            if (rollOne == 10)//Checks if roll one was Strike and if it was it sets it's bool to true.
            {
                isStrike = true;
            }
            else
            {
                isStrike = false;
            }

            return isStrike;
        }

        //Checks if the passed rolls make a Spare.
        static bool IsThisSpare(int rollOne, int rollTwo)
        {
            if (rollOne + rollTwo == 10)//Checks if both rolls combined equal a spare and if so sets bool to true.
            {
                isSpare = true;
            }
            else
            {
                isSpare = false;
            }

            return isSpare;
        }

        //Adds bonus points for Spares and Strikes.
        static int[,] AddingBonuses(int rollOne, int rollTwo, int[,] scoreBoard, int frame)
        {
            //Adding bonuses to previous frames
            if (frame >= 1)
            {
                if (scoreBoard[1, frame - 1] == Strike)  //Last frame was a Strike
                {
                    if (rollOne == 10)  //This frame was a Strike, add first roll
                    {
                        scoreBoard[0, frame - 1] += rollOne;
                        scoreBoard[0, frame] += rollOne;
                        scoreBoard[1, frame - 1]--;

                        if (frame >= 2)
                        {
                            if (scoreBoard[1, frame - 2] == 1)  //Two frames ago was a Strike, add first roll.
                            {
                                scoreBoard[0, frame - 2] += rollOne;
                                scoreBoard[0, frame] += rollOne;
                                scoreBoard[0, frame - 1] += rollOne;
                                scoreBoard[1, frame - 2] = 0;
                            }
                        }
                    }
                    else  //Current frame wasn't a strike, add both rolls.
                    {
                        scoreBoard[0, frame - 1] += rollOne + rollTwo;
                        scoreBoard[0, frame] += rollOne + rollTwo;
                        scoreBoard[1, frame - 1] = 0;
                    }
                }
                else if (scoreBoard[1, frame - 1] == Spare)  //Last frame was a Spare, add first roll.
                {
                    scoreBoard[0, frame - 1] += rollOne;
                    scoreBoard[0, frame] += rollOne;
                    scoreBoard[1, frame - 1] = 0;
                }
            }

            return scoreBoard;
        }

        //Adds bonus points for any extra rolls in the last frame.
        static int[,] BonusRound(int rollsLeft, int[,] scoreBoard)
        {
            int rollOne = 0; int rollTwo = 0;//Declaring variables to be used in this method.

            if (rollsLeft == 2)//Checking to see how many bonus rolls are left.
            {
                do
                {
                    Console.WriteLine("Enter bonus roll.");
                    isValid = int.TryParse(Console.ReadLine(), out rollOne);//Returns true if the parse was successful and overwrites rollOne.
                    if (isValid)
                    {

                        if (IsThisScoreValid(rollOne, 0))//Checking the last roll.
                        {
                            isStrike = IsThisStrike(rollOne);//If last roll was a Strike, breaks loop if it is.
                            if (!isStrike)
                            {
                                rollsLeft = 0;//Takes the roll left to try and roll a Spare.
                                Console.WriteLine("Enter bonus roll.");
                                isValid = int.TryParse(Console.ReadLine(), out rollTwo);//Returns true if the parse was successful and overwrites rollTwo.

                                if (isValid)
                                {
                                    if (IsThisScoreValid(rollOne, rollTwo))//Validate score.
                                    {
                                        isSpare = IsThisSpare(rollOne, rollTwo);//If roll is valid runs the Spare method to reuse for last Strike.
                                    }
                                    else
                                    {
                                        Console.WriteLine("Score not possible please try again");//Error message.
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Please enter valid roll!");//Error message.
                                    isValid = false;
                                }
                            }
                            else
                            {
                                rollTwo = 0;//Strike!!!
                                rollsLeft--;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Score not possible please try again");//Error message.
                            isValid = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter valid roll!");//Error message.
                        isValid = false;
                    }

                } while (!isValid);

                if (rollOne == 10)//Adds bonus rolls for Strikes
                {
                    scoreBoard[0, 9] += rollOne;
                    scoreBoard = BonusRound(rollsLeft, scoreBoard);
                }
                else
                {
                    scoreBoard[0, 9] += rollOne + rollTwo;//Updates scoreboard.
                }

            }
            else if (rollsLeft == 1)//Runs bonus rolls for a Spare.
            {
                do
                {
                    Console.WriteLine("Enter bonus roll.");
                    isValid = int.TryParse(Console.ReadLine(), out rollOne);//Returns true if the parse was successful and overwrites rollOne.
                    if (isValid)
                    {
                        isValid = IsThisScoreValid(rollOne, 0);//Validating roll.
                        if (isValid)
                        {
                            scoreBoard[0, 9] += rollOne;//Adding roll to the scoreboard.
                        }
                        else
                        {
                            Console.WriteLine("Score not possible please try again");//Error message.
                            isValid = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid roll!");//Error message.
                        isValid = false;
                    }
                } while (!isValid);

            }

            if (scoreBoard[1, 8] >= 1)//Updating Scoreboard and removing flag.
            {
                scoreBoard[0, 8] += rollOne;
                scoreBoard[0, 9] += rollOne;
                scoreBoard[1, 8]--;
            }

            return scoreBoard;
        }

    }
}
