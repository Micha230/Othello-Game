using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02_Othelo
{
    public class Player
    {
        private const string k_Black = "X";
        private const string k_White = "O";
        private readonly string r_Name;
        private int m_Score;
        private string m_Sign;

        public Player(string i_Name, string i_Sign)
        {
            r_Name = i_Name;
            m_Sign = i_Sign;
        }

        public string Sign
        {
            get { return m_Sign; }
            set { m_Sign = value; }
        }

        public string PlayerName
        {
            get { return r_Name; }
        }

        public int PlayerScore
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public string GetOpponentSign()
        {
            string opponentSign;
            if (this.m_Sign == k_White)
            {
                opponentSign = k_Black;
            }
            else
            {
                opponentSign = k_White;
            }

            return opponentSign;
        }
    }
}
