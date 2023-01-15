using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02_Othelo
{
    public class UserInterface
    {
        private const string k_Black = "X";
        private const string k_White = "O";
        private readonly Random r_ComputerRandomMove;
        private int m_BlackNumOfWins;
        private int m_WhiteNumOfWins;
        private bool m_IsAgainstComputer;
        private OthelloGame m_Game;
        private int m_GameBoardSize;
        private Player m_CurrentPlayer;
        private Player[] m_CurrentPlayersArray;

        public UserInterface()
        {
            m_CurrentPlayersArray = new Player[2];
            m_BlackNumOfWins = 0;
            m_WhiteNumOfWins = 0;
            r_ComputerRandomMove = new Random();

            startNewGame();
        }

        private void startNewGame()
        {
            ////in case it's the first game we want to get user input
            if (m_BlackNumOfWins == 0 && m_WhiteNumOfWins == 0)
            {
                Console.WriteLine("Enter your name: ");
                string firstPlayerName = Console.ReadLine();
                Console.WriteLine("Do you want to play against another player or against the computer? " +
                    "(Enter 1 for player or 2 for computer)");

                int playerOrComputer = int.Parse(Console.ReadLine());
                string secondPlayerName = "Computer";
                if (playerOrComputer == 1)
                {
                    Console.WriteLine("Enter second player name: ");
                    secondPlayerName = Console.ReadLine();
                    m_IsAgainstComputer = true;
                }

                Player[] playersArray = new Player[2];

                playersArray[0] = new Player(firstPlayerName, k_Black);
                if (m_IsAgainstComputer)
                {
                    playersArray[1] = new Player(secondPlayerName, k_White);
                }
                else
                {
                    playersArray[1] = new Player(secondPlayerName, k_White);
                }

                m_CurrentPlayersArray = playersArray;
                m_CurrentPlayer = playersArray[0];

                Console.WriteLine("Enter the size of the game board (6 or 8): ");
                m_GameBoardSize = int.Parse(Console.ReadLine());
               while(m_GameBoardSize!=6 && m_GameBoardSize!=8)
                {
                    Console.WriteLine("Invalid input! Please choose between 6 or 8.");
                    m_GameBoardSize = int.Parse(Console.ReadLine());
                }
            }

            m_Game = new OthelloGame(m_GameBoardSize, m_IsAgainstComputer, m_CurrentPlayersArray);
            PrintBoard();
            playGame();
        }

        private void playGame()
        {
            setValidMoves();
            printText(string.Format("Othello - {0}'s turn", m_CurrentPlayer.PlayerName));
            if (m_Game.HasValidMoves(m_Game.PlayersArray[0]) || m_Game.HasValidMoves(m_Game.PlayersArray[1]))
            {
                if (hasValidMoveForCurrentPlayer())
                {
                    if (m_CurrentPlayer.PlayerName == "Computer")
                    {
                        makeComputerMove();
                    }
                    else
                    {
                        string move = getUserMove();
                        makePlayerMove(move);
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("{0} doesn't have any valid moves! The turn goes to the opponant", m_CurrentPlayer.PlayerName));
                    switchPlayer();
                    playGame();
                }
            }
            else
            {
                getWinner();
            }
        }

        private string getUserMove()
        {
            string move;
            while (true)
            {
                // Prompt the user for a move
                Console.WriteLine("Enter your move: (a letter for the column and number for the row i.e A3)");
                 move = Console.ReadLine();

                if (move.ToUpper() == "Q")
                {
                    // The user wants to quit the game
                    Console.WriteLine("Bye for now ! See you in the next game (:");
                    Environment.Exit(0);
                }

                if (m_Game.IsSyntacticValidation(move) && m_Game.IsValidMove(move, m_CurrentPlayer))
                {
                    // The input is syntactically valid, so we can exit the loop
                    break;
                }

                // The input is not syntactically valid, so we need to try again
                Console.WriteLine("Invalid input. Please try again.");
            }

            return move;
        }

        private void printText(string text)
        {
            Console.WriteLine(text);
        }

        public void PrintBoard()
        {
            Ex02.ConsoleUtils.Screen.Clear();

            string boardString = string.Empty;

            // Display the column labels
            boardString += "       ";
            for (int i = 0; i < m_GameBoardSize; i++)
            {
                boardString += (char)('A' + i) + "     ";
            }

            boardString += "\n";
            if (m_GameBoardSize == 8)
            {
                boardString += "     =================================================\n";
            }
            else if (m_GameBoardSize == 6)
            {
                boardString += "     =====================================\n";
            }
            //// Display the rows of the board
            for (int i = 0; i < m_GameBoardSize; i++)
            {
                // Display the row label
                boardString += string.Format(" {0}   ", i + 1);

                // Display the cells of the row
                for (int j = 0; j < m_GameBoardSize; j++)
                {
                    boardString += "|  ";
                    string cell = m_Game.BoardGame.Board[i, j];
                    if (cell == k_Black)
                    {
                        boardString += k_Black;
                    }
                    else if (cell == k_White)
                    {
                        boardString += k_White;
                    }
                    else
                    {
                        boardString += " ";
                    }

                    boardString += "  ";
                }

                boardString += "|\n";

                // Display the separator row
                boardString += "     ";
                for (int j = 0; j < m_GameBoardSize; j++)
                {
                    boardString += "======";
                }

                boardString += "\n";
            }

            boardString += "\n";
            Console.WriteLine(boardString);
        }
        
        private void setValidMoves()
        {
            m_Game.GetValidMoves(m_CurrentPlayer);
            if (m_Game.ValidMoves.Count == 0)
            {
                switchPlayer();
                m_Game.GetValidMoves(m_CurrentPlayer);
                if (m_Game.ValidMoves.Count == 0)
                {
                    getWinner();
                }
            }
        }

        private bool hasValidMoveForCurrentPlayer()
        {
            return m_Game.ValidMoves.Count > 0;
        }

        private void makePlayerMove(string i_Move)
        {
            m_Game.MakeMove(m_CurrentPlayer, i_Move);
            PrintBoard();
            switchPlayer();
            playGame();
        }

        private void makeComputerMove()
        {
            m_Game.GetValidMoves(m_CurrentPlayer);
            int randomIndex = r_ComputerRandomMove.Next(0, m_Game.ValidMoves.Count);
            m_Game.MakeMove(m_Game.PlayersArray[1], m_Game.ValidMoves[randomIndex]);
            PrintBoard();
            switchPlayer();
            playGame();
        }

        private void switchPlayer()
        {
            if (m_CurrentPlayer.Sign == "X")
            {
                m_CurrentPlayer = m_Game.PlayersArray[1];
            }
            else
            {
                m_CurrentPlayer = m_Game.PlayersArray[0];
            }
        }

        private void getWinner()
        {
            if (m_Game.PlayersArray[0].PlayerScore > m_Game.PlayersArray[1].PlayerScore)
            {
                m_BlackNumOfWins++;
                getWinnerMessage(m_Game.PlayersArray[0]);
            }
            else if (m_Game.PlayersArray[0].PlayerScore < m_Game.PlayersArray[1].PlayerScore)
            {
                m_WhiteNumOfWins++;
                getWinnerMessage(m_Game.PlayersArray[1]);
            }
            else
            {
                getTieMessage();
            }
        }

        private void getWinnerMessage(Player i_Winner)
        {
            string message = string.Format(
@"{0} Won!! ({1}/{2}) ({3}/{4})
Would you like another round?",
                i_Winner.PlayerName,
                m_Game.BoardGame.BlackScore,
                m_Game.BoardGame.WhiteScore,
                m_BlackNumOfWins,
                m_WhiteNumOfWins);

            endGame(message);
        }

        private void getTieMessage()
        {
            string message = string.Format(
@"Game ends in tie! 
Would you like another round?");

            endGame(message);
        }

        private void endGame(string i_Message)
        {
            Console.WriteLine(i_Message);
            Console.WriteLine("(Enter 'Y' for yes or 'N' for no");
            string answer = Console.ReadLine();

            if (answer == "Y")
            {
                startNewGame();
            }
            else if (answer == "N")
            {
                Console.WriteLine("Bye for now ! See you in the next game (:");
                Environment.Exit(0);
            }
        }
    }
}