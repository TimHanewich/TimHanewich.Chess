using System;
using System.Collections.Generic;
using TimHanewich.Chess;

namespace PlayEngine
{
    public class MoveHistory
    {
        List<string> _Moves = new List<string>();

        public MoveHistory()
        {
            _Moves = new List<string>();
        }

        public string[] Moves
        {
            get
            {
                return _Moves.ToArray();
            }
        }

        public void AddNextMove(string move)
        {
            _Moves.Add(move);
        }

        public string[] WhiteMoves
        {
            get
            {
                return MovesByColor(Color.White);
            }
        }

        public string[] BlackMoves
        {
            get
            {
                return MovesByColor(Color.Black);
            }
        }



        private string[] MovesByColor(Color by)
        {
            List<string> White = new List<string>();
            List<string> Black = new List<string>();
            Color OnColor = Color.White;
            foreach (string s in Moves)
            {
                //Add
                if (OnColor == Color.White)
                {
                    White.Add(s);
                }
                else if (OnColor == Color.Black)
                {
                    Black.Add(s);
                }
                
                //Switch the on color
                if (OnColor == Color.White)
                {
                    OnColor = Color.Black;
                }
                else if (OnColor == Color.Black)
                {
                    OnColor = Color.White;
                }
            }

            if (by == Color.White)
            {
                return White.ToArray();
            }
            else if (by == Color.Black)
            {
                return Black.ToArray();
            }
            else
            {
                throw new Exception("I do not understand color '" + by.ToString() + "'");
            }
        }

    }
}