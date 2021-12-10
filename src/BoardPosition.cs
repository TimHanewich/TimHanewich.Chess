using System;
using System.Collections.Generic;

namespace TimHanewich.Chess
{
    public class BoardPosition
    {
        public Color ToMove {get; set;}


        private List<Piece> Pieces;

        public BoardPosition()
        {
            Pieces = new List<Piece>();
        }

        public BoardPosition(string FEN)
        {
            int loc1 = FEN.IndexOf(" ");
            if (loc1 == -1)
            {
                throw new Exception("Supplied FEN is invalid.");
            }

            //Get the positon part
            string PositionPortion = FEN.Substring(0, loc1 - 1);
            
            //Split it up
            string[] Rows = PositionPortion.Split(new string[] {"/"}, StringSplitOptions.None);

            //Assemble the pieces
            

        }

    }
}