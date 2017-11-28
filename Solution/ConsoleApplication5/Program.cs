using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ConsoleApplication5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int[] player1 = new int[2] {0,0 }; // games|sets 
            int[] player2 = new int[2] {0,0 };

            bool play = true;
            while (play)
            {
                bool setIsOver = false;
                int countGames = 0;
                int countSet = 0;
                while (!(setIsOver))
                {
                    string input = Console.ReadLine(); //Get input from user

                    if (input == "-" )  // Points: 0-0
                    {
                        Console.WriteLine("Love all");
                        continue;
                    }

                    List<int> inputPoints = IsUserInputCorrect(input);

                    if (!(inputPoints[0] != -1 )) 
                    {
                        continue;  // if incorect dont increment set but ask for new input
                    }

                    string resultPoints = GetResultPoints(inputPoints);

                    if (!(IsGameOver(resultPoints))) //user input is not enough in order to finish the game
                    {
                        Console.WriteLine("Current Game:");
                        Console.WriteLine(resultPoints);
                        Console.WriteLine($"Status of Sets: { player1[1] }:{ player2[1] }");
                        continue;
                    }
                    else // get winner of game and increment the winners result of gameCounter
                    {
                        int winnerInCurrentGame = GetWinnerInCurretGame(resultPoints);
                        if (winnerInCurrentGame == 1)
                        {
                            player1[0]++;
                        }
                        else
                        {
                            player2[0]++;
                        }

                        PrintOutputCurrectGame(resultPoints);
                    }

                    countGames++;

                    int gameCounterPlayer1 = player1[0];
                    int gameCounterPlayer2 = player2[0];

                    if ( IsSetOver(gameCounterPlayer1, gameCounterPlayer2) ) // check if set is over + check if tie of set is over
                    {
                        int winnerOfSet = GetWinnerOfCurrentSet(gameCounterPlayer1, gameCounterPlayer2);
                        resetPlayerGameCounter(ref player1[0], ref player2[0]); // set is over. Need to reset the game counter

                        if (winnerOfSet == 1)
                        {
                            player1[1]++;  // increase setcounter of player1
                        }
                        else 
                        {
                            player2[1]++; // increase setcounter of player2
                        }
                        setIsOver = true; // set is over so need to get out of current while loop 
                    }
                    
                    
                    PrintOutputMatchInformation(player1,player2);

                }
                countSet++;
                int setCountPlayer1 = player1[1];
                int setCountPlayer2 = player2[1];

                if ( IsMatchOver(setCountPlayer1, setCountPlayer2) )
                {
                    string winnerMatch = GetMatchWinner(setCountPlayer1, setCountPlayer2);
                     
                    Console.WriteLine($"Match Winner: {winnerMatch}");
                    play = false;
                }
            }
        }

        public static string GetMatchWinner(int setCountPlayer1, int setCountPlayer2)
        {
            string winner = "Player1";
            if (setCountPlayer2 > setCountPlayer1)
            {
                winner = "Player2";
            }
            return winner;
        }

        public static bool IsMatchOver(int setCountPlayer1, int setCountPlayer2)
        {
            bool isMatchOver = false;
            if (setCountPlayer1 > 1 || setCountPlayer2 > 1)
            {
                isMatchOver = true;
            }
            return isMatchOver;
        }

        static void resetPlayerGameCounter(ref int v1, ref int v2)
        {
            v1 = 0;
            v2 = 0;
        }

        static void PrintOutputMatchInformation(int[] player1, int[] player2)
        {
            Console.WriteLine($"Status of Games: { player1[0] }:{ player2[0] }");
            Console.WriteLine($"Status of Sets: { player1[1] }:{ player2[1] }");
        }

        static void PrintOutputCurrectGame(string resultPoints)
        {
            Console.WriteLine(resultPoints);
        }

        static int GetWinnerOfCurrentSet(int gameCounterPlayer1, int gameCounterPlayer2)
        {
            int winnerSet = 1; // default Player1
            if (gameCounterPlayer2 > gameCounterPlayer1)
            {
                winnerSet = 2;
            }
            return winnerSet;
        }

        static bool IsSetOver(int gameCounterPlayer1, int gameCounterPlayer2)
        {
            bool isGameOver = false;

            if (gameCounterPlayer1 >= 6 || gameCounterPlayer2 >= 6)
            {
                if ( Math.Abs( gameCounterPlayer1 - gameCounterPlayer2) > 1 )
                {
                    isGameOver = true;
                }
            }
            return isGameOver;
        }

        static int GetWinnerInCurretGame(string resultPoints)
        {
            string[] token = resultPoints.Split(' ');
            int winnerOfGame = 1; // default winner is Player1
            if (!(token.Length > 1 ))
            {
                string winnerTie = token[0];
                if (winnerTie == "Player2")
                {
                    winnerOfGame = 2;
                }
            }
            else
            {
                string resultOfPlayer2 = token[1];
                if (resultOfPlayer2 == "40")
                {
                    winnerOfGame = 2;
                }
            }
            return winnerOfGame;
        }

        public static bool IsGameOver(string resultPoints)
        {
            string[] token = resultPoints.Split(' ');
            bool gameIsOver = false;
            if (token.Length < 2)  // found bug, quick fix
            {
                if (token[0] != "Deuce")
                {
                    gameIsOver = true; // Set is won by someone
                }
                //default dont do anything -> will be false if  Deuce
            }
            else if (token[1] != "all" && token[0] == "40")
            {
                gameIsOver = true;
            }
            else if (token[0] != "all" && token[1] == "40")
            {
                gameIsOver = true;
            }
                return gameIsOver;
        }

        static string GetResultPoints(List<int> inputPoints)
        {
            int minThresholdGame = 3; // threshold for game is min 3 points to win
              
            string resultFinalNotTie = string.Empty;
            Dictionary<int, string> pointName = new Dictionary<int, string>()
            {
                { 0 ,"Love" },
                { 1 ,"15" },
                { 2 ,"30" },
                { 3 ,"40" }
            };

            if ( isGameTied(inputPoints, minThresholdGame)  ) 
            {
                 int[] tieResult = GetWinnerFromTie(inputPoints, minThresholdGame);
                string resultToReturnFromTie = GetTennisScoreName(tieResult);
                return resultToReturnFromTie;
            }
            else
            {
                int[] resultIndividualPlayersNotTieRow = GetResultWithoutTie(inputPoints);
                string[] resultIndividualPlayersNotTie = new string[2];

                int player1RowResult = resultIndividualPlayersNotTieRow[0];
                int player2RowResult = resultIndividualPlayersNotTieRow[1];

                resultIndividualPlayersNotTie[0] = pointName[player1RowResult];
                resultIndividualPlayersNotTie[1] = pointName[player2RowResult];

                if (resultIndividualPlayersNotTie[0] == resultIndividualPlayersNotTie[1])
                {
                    resultIndividualPlayersNotTie[1] = "all";
                }

                resultFinalNotTie = string.Format($"{resultIndividualPlayersNotTie[0] } {resultIndividualPlayersNotTie[1] }");
            }

            return resultFinalNotTie;
        }

        static int[] GetResultWithoutTie(List<int> inputPoints)
        {
            int[] resultToReturn = new int[2];

            for (int i = 0; i < inputPoints.Count; i++)
            {
                int temp = inputPoints[i];
                resultToReturn[temp - 1]++; 

                if (resultToReturn[0] > 3 || resultToReturn[1] > 3)
                {
                    resultToReturn = resultToReturn.Select(x => x = x > 3 ? 3 : x).ToArray(); 
                    break;
                }
            }
           
            return resultToReturn;
        }

        static int[] GetWinnerFromTie(List<int> inputPoints, int threshold)
        {
            int[] playersPointsInTie = new int[] { 0, 0 }; // indexer 0 : P1 , index 1: P2
            
            for (int i = 0 ; i < inputPoints.Count; i++)
            {
                int winnerCurrentService = inputPoints[i];
                playersPointsInTie[winnerCurrentService - 1]++;

                if ( 
                    (playersPointsInTie[0] >= threshold || playersPointsInTie[1] >= threshold)
                    && ( Math.Abs(playersPointsInTie[0] - playersPointsInTie[1]) > 1  )
                   )
                {
                    break;
                }
            }
            playersPointsInTie = GetWinnerFromPointsResultToTieResult(playersPointsInTie);
            return playersPointsInTie;
        }

        static string GetTennisScoreName(int[] playersPointsInTie)
        {
            string result = string.Empty;
            Dictionary<int, Dictionary<int, string>> tieScenarioNames = new Dictionary<int, Dictionary<int, string>>();
            tieScenarioNames.Add(0, new Dictionary<int, string>());
            tieScenarioNames.Add(1, new Dictionary<int, string>());
            tieScenarioNames[0].Add(0, "Deuce");
            tieScenarioNames[0].Add(1, "Advantage Out");
            tieScenarioNames[0].Add(2, "Player2"); //Winner Player2 
            tieScenarioNames[1].Add(0, "Advantage In");
            tieScenarioNames.Add(2, new Dictionary<int, string>());
            tieScenarioNames[2].Add(0, "Player1"); //Winner Player1 

            int pointsPlayer1 = playersPointsInTie[0];
            int pointsPlayer2 = playersPointsInTie[1];
            result = tieScenarioNames[pointsPlayer1][pointsPlayer2];
            return result;
        }

        static int[] GetWinnerFromPointsResultToTieResult(  int[] playersPointsInTie)
        {
            int[] result = new int[] {0,0 };  //default Player 1 is winner

            if ( (Math.Abs(playersPointsInTie[0] - playersPointsInTie[1]) > 1 )  && (playersPointsInTie[0] > 3 || playersPointsInTie[1] > 3) ) // found bug so fast fix.. 3- min threshold 
            {
                if (playersPointsInTie[0] < playersPointsInTie[1])
                {
                    result = new int[] { 0, 2 };
                }
                else 
                {
                    result = new int[] { 2, 0 };
                }
            }
            else 
            {
                if (playersPointsInTie[0] < playersPointsInTie[1])
                {
                    result = new int[] { 0, 1 };
                }
                else if (playersPointsInTie[0] > playersPointsInTie[1])
                {
                    result = new int[] { 1, 0 };
                }
            }
            return result;
        }

        static bool isGameTied( List<int> inputValues, int minThreshold)
        {
            bool isTie = false; // if find that there is tie change to true
            if (inputValues.Count >= minThreshold)
            {
                int[] playersPoints= new int[2] { 0,0 }; // index:0 - Player1 , index:1 - Player2 . Used to temp store Players score

                for (int i = 0; i < inputValues.Count; i++) // iterate in order to find if there is a tie
                {
                    playersPoints[inputValues[i] - 1]++; // increment with 1 the according player
                    if ( 
                        (playersPoints[0] >= minThreshold || playersPoints[1] >= minThreshold)
                        && (
                            ( playersPoints[0] == playersPoints[1]) || ( Math.Abs( playersPoints[0] - playersPoints[1]) < 2 ) )
                           ) // 1. Check for minThreshold is met AND 2.result must be equal or the difference between players < 2
                    {
                        isTie = true;
                        return isTie;  // found a tie 
                    }
                }
            }
            return isTie;
        }

        static List<int> IsUserInputCorrect(string input) //if correct resturn Points else  (-1) to indicate error input
        {
            List<int> result = new List<int>();

            if (input.Length == 0)
            {
                result.Add(-1);
                return result;
            }

            foreach (char item in input.Where(x => (int)x != 44) )
            {
                if ( (int)item < 49 || (int)item > 50) //using asci table to check if value is 1  or 2
                {
                    result.Add(-1);
                    return result;
                }
            }
            result = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToList();
            return result; // if all values correct reurn true - input is correct
        }
    }
}