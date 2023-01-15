using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02_Othelo
{
    public class OthelloBoard
    {
        private const string k_Black = "X";
        private const string k_White = "O";

        private static readonly int[][] BoardDirections = 
        {
            new[] { -1, -1 }, new[] { -1, 0 }, new[] { -1, 1 },
            new[] { 0, -1 }, new[] { 0, 1 },
            new[] { 1, -1 }, new[] { 1, 0 }, new[] { 1, 1 }
        };

        private int m_BlackScore;
        private int m_WhiteScore;
        private string[,] m_Board;
        private int m_Size;

        public string[,] Board
        {
            get
            { 
                return m_Board; 
            }

            set 
            { 
                m_Board = value; 
            }
        }

        public int Size
        {
            get 
            {
                return m_Size;
            }

            set 
            {
                m_Size = value; 
            }
        }

        public int BlackScore
        {
            get { return m_BlackScore; }

            set { m_BlackScore = value; }
        }

        public int WhiteScore
        {
            get { return m_WhiteScore; }

            set { m_WhiteScore = value; }
        }

        public OthelloBoard(int i_size)
        {
            Size = i_size;
            m_Board = new string[i_size, i_size];

            // Initialize the board with the starting positions
            m_Board[(i_size / 2) - 1, (i_size / 2) - 1] = m_Board[i_size / 2, i_size / 2] = k_White;
            m_Board[(i_size / 2) - 1, i_size / 2] = m_Board[i_size / 2, (i_size / 2) - 1] = k_Black;

            m_BlackScore = 2;
            m_WhiteScore = 2;
        }

        public bool IsValidMove(int i_row, int i_col, Player i_CurrentPlayer)
        {
            // Check if the position is already occupied
            if (this.m_Board[i_row, i_col] != null)
            {
                return false;
            }

            // Check if the move captures any pieces
            foreach (int[] direction in BoardDirections)
            {
                int capturedPieces = CheckDirection(i_row, i_col, i_CurrentPlayer, direction[0], direction[1]);
                if (capturedPieces > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private int CheckDirection(int row, int col, Player i_CurrentPlayer, int rowDelta, int colDelta)
        {
            int capturedPieces = 0;
            int currentRow = row + rowDelta;
            int currentCol = col + colDelta;

            // Check if the move captures any pieces in this direction
            while (currentRow >= 0 && currentRow < this.m_Size && currentCol >= 0 && currentCol < this.m_Size)
            {
                if (this.m_Board[currentRow, currentCol] == i_CurrentPlayer.Sign)
                {
                    // We found a piece of the same color, so we can capture all the pieces in between
                    return capturedPieces;
                }
                else if (this.m_Board[currentRow, currentCol] == null)
                {
                    // We found an empty space, so we can't capture any pieces in this direction
                    return 0;
                }
                else
                {
                    // We found a piece of the other color, so keep checking in this direction
                    capturedPieces++;
                    currentRow += rowDelta;
                    currentCol += colDelta;
                }
            }

            // If we get here, we have reached the edge of the board without finding a piece of the same color
            return 0;
        }

        public void MakeMove(int i_row, int i_col, Player i_CurrentPlayer)
        {
            // Place the piece on the board
            this.m_Board[i_row, i_col] = i_CurrentPlayer.Sign;

            // Update the score
            if (i_CurrentPlayer.Sign == k_Black)
            {
                m_BlackScore++;
                i_CurrentPlayer.PlayerScore = m_BlackScore;
            }
            else
            {
                m_WhiteScore++;
                i_CurrentPlayer.PlayerScore = m_WhiteScore;
            }

            // Capture any pieces in all directions
            foreach (int[] direction in BoardDirections)
            {
                int capturedPieces = CheckDirection(i_row, i_col, i_CurrentPlayer, direction[0], direction[1]);
                if (capturedPieces > 0)
                {
                    CapturePieces(i_row, i_col, i_CurrentPlayer, direction[0], direction[1], capturedPieces);
                }
            }
        }

        private void CapturePieces(int row, int col, Player i_CurrentPlayer, int rowDelta, int colDelta, int numPieces)
        {
            for (int i = 1; i <= numPieces; i++)
            {
                // Capture the piece by changing its color
                int capturedRow = row + (i * rowDelta);
                int capturedCol = col + (i * colDelta);
                this.m_Board[capturedRow, capturedCol] = i_CurrentPlayer.Sign;

                // Update the score
                if (i_CurrentPlayer.Sign == k_Black)
                {
                    m_BlackScore++;
                    m_WhiteScore--;
                }
                else
                {
                    m_WhiteScore++;
                    m_BlackScore--;
                }
            }
        }
    }
}