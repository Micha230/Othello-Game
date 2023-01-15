using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02_Othelo
{
    public class OthelloGame
    {
        private Player[] m_Players;
        private OthelloBoard m_BoardGame;
        private bool m_AgainstComputer;
        private List<string> m_ValidMoves;

        public OthelloGame(int i_ChosenSize, bool i_IsAgainstComputer, Player[] i_PlayersArray)
        {
            m_BoardGame = new OthelloBoard(i_ChosenSize);
            m_Players = i_PlayersArray;
            m_AgainstComputer = i_IsAgainstComputer;
            m_ValidMoves = new List<string>();
            UpdateScore(); 
        }

        public List<string> ValidMoves
        {
            get { return m_ValidMoves; }
            set { m_ValidMoves = value; }
        }

        public OthelloBoard BoardGame
        {
            get { return m_BoardGame; }
            set { m_BoardGame = value; }
        }

        public Player[] PlayersArray
        {
            get { return m_Players; }
            set { m_Players = value; }
        }

        public void UpdateScore()
        {
            m_Players[0].PlayerScore = m_BoardGame.BlackScore;
            m_Players[1].PlayerScore = m_BoardGame.WhiteScore;
        }

        public bool AgainstComputer
        {
            get { return m_AgainstComputer; }
            set { m_AgainstComputer = value; }
        }

        public bool IsValidMove(string i_Move, Player i_Player)
        {
            int col = i_Move[0] - 'A';
            int row = int.Parse(i_Move[1].ToString()) - 1;
            return m_BoardGame.IsValidMove(row, col, i_Player);
        }

        public void GetValidMoves(Player i_Player)
        {
            m_ValidMoves.Clear(); // Clear the list of valid moves from previous turns

            for (int j = 0; j < m_BoardGame.Board.GetLength(0); j++)
            {
                for (int i = 0; i < m_BoardGame.Board.GetLength(0); i++)
                {
                    string move = string.Format("{0}{1}", (char)('A' + j), i + 1);
                    if (m_BoardGame.IsValidMove(i, j, i_Player))
                    {
                        ValidMoves.Add(move);
                    }
                }
            }
        }

        public bool HasValidMoves(Player i_Player)
        {
            // Loop through the board and check if any position is a valid move for the player
            for (int i = 0; i < m_BoardGame.Size; i++)
            {
                for (int j = 0; j < m_BoardGame.Size; j++)
                {
                    if (m_BoardGame.IsValidMove(i, j, i_Player))
                    {
                        // We found a valid move, so the player has at least one valid move remaining
                        return true;
                    }
                }
            }

            // If we get here, the player has no valid moves remaining
            return false;
        }

        public bool IsSyntacticValidation(string i_Move)
        {
            bool isValid = false;

            // Check if the input is in the correct format
            if (i_Move.Length == 2)
            {
                char col = i_Move[0];
                char row = i_Move[1];

                // Check if the column is a letter between 'A' and 'Z' (uppercase or lowercase)
                if (col >= 'A' && col <= 'Z')
                {
                    // Check if the row is a number between 1 and the size of the board
                    if (row >= '1' && row <= '0' + m_BoardGame.Size)
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

        public void MakeMove(Player i_Player, string i_Move)
        {
            int col = i_Move[0] - 'A';
            int row = int.Parse(i_Move[1].ToString()) - 1;
            m_BoardGame.MakeMove(row, col, i_Player);
        }
    }
}