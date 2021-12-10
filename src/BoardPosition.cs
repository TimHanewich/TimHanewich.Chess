using System;
using System.Collections.Generic;

namespace TimHanewich.Chess
{
    public class BoardPosition
    {
        public Color ToMove {get; set;}
        private List<Piece> _Pieces;

        public BoardPosition()
        {
            _Pieces = new List<Piece>();
        }

        public BoardPosition(string FEN)
        {
            _Pieces = new List<Piece>();

            int loc1 = FEN.IndexOf(" ");
            if (loc1 == -1)
            {
                throw new Exception("Supplied FEN is invalid.");
            }

            //Get the positon part
            string PositionPortion = FEN.Substring(0, loc1);
            
            //Split it up
            string[] Rows = PositionPortion.Split(new string[] {"/"}, StringSplitOptions.None);

            //Assemble the pieces
            Position OnPosition = Position.A8;
            foreach (string row in Rows)
            {

                //Convert each character to a piece of empty space
                foreach (char c in row)
                {

                    //Try to get numeric value
                    int? NumericValue = null; 
                    try
                    {
                        NumericValue = Convert.ToInt32(c.ToString());
                    }
                    catch
                    {
                        NumericValue = null;
                    }


                    //If it is a number, make space. If not, make it a piece
                    if (NumericValue.HasValue)
                    {
                        for (int t = 0; t < NumericValue.Value; t++)
                        {
                            if (OnPosition.File() != 'H')
                            {
                                OnPosition = OnPosition.Right();
                            }
                        }
                    }
                    else
                    {
                        Piece p = new Piece();
                        p.Position = OnPosition;

                        //Piece type
                        if (c.ToString().ToLower() == "r")
                        {
                            p.Type = PieceType.Rook;
                        }
                        else if (c.ToString().ToLower() == "n")
                        {
                            p.Type = PieceType.Knight;
                        }
                        else if (c.ToString().ToLower() == "b")
                        {
                            p.Type = PieceType.Bishop;
                        }
                        else if (c.ToString().ToLower() == "q")
                        {
                            p.Type = PieceType.Queen;
                        }
                        else if (c.ToString().ToLower() == "k")
                        {
                            p.Type = PieceType.King;
                        }
                        else if (c.ToString().ToLower() == "p")
                        {
                            p.Type = PieceType.Pawn;
                        }

                        //Piece color
                        if (c.ToString().ToUpper() == c.ToString()) //If the to upper version is the same as the notation itself, it is capital. And if it is capital, it is white.
                        {
                            p.Color = Color.White;
                        }
                        else
                        {
                            p.Color = Color.Black;
                        }


                        //Add the piece
                        _Pieces.Add(p);

                        //Advance the onposition by 1
                        if (OnPosition.File() != 'H')
                        {
                            OnPosition = OnPosition.Right();
                        }
                    }                    
                }
           
                //Advance to the next rank (1 down)
                if (OnPosition.Rank() > 1)
                {
                    OnPosition = PositionToolkit.Parse("A" + Convert.ToString(OnPosition.Rank() - 1));
                }
            }

            //Who is to move?
            loc1 = FEN.IndexOf(" ");
            int loc2 = FEN.IndexOf(" ", loc1 + 1);
            if (loc2 == -1)
            {
                throw new Exception("Supplied FEN is invalid. To move not available.");
            }
            string ToMoveStr = FEN.Substring(loc1 + 1, loc2 - loc1 - 1);
            if (ToMoveStr.ToLower() == "w")
            {
                ToMove = Color.White;
            }
            else if (ToMoveStr.ToLower() == "b")
            {
                ToMove = Color.Black;
            }
            else
            {
                throw new Exception("Active color '" + ToMoveStr + "' not recognized in FEN.");
            }
        }

        public Piece[] Pieces
        {
            get
            {
                return _Pieces.ToArray();
            }
        }

    }
}