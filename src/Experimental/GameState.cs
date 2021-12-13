using System;
using System.Collections.Generic;

namespace TimHanewich.Chess.Experimental
{
    public class GameState
    {
        private int[] BoardState;
        public bool WhiteToMove {get; set;}

        public GameState()
        {

        }

        public GameState(string FEN)
        {
            int loc1 = FEN.IndexOf(" ");
            if (loc1 == -1)
            {
                throw new Exception("Supplied FEN is invalid.");
            }

            //Get the positon part
            string PositionPortion = FEN.Substring(0, loc1);
            
            //Split it up
            string[] Rows = PositionPortion.Split(new string[] {"/"}, StringSplitOptions.None);

            //Assemble the board state
            List<int> BuildingBoardState = new List<int>();
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
                                //Add empty space to the board state
                                BuildingBoardState.Add(0);

                                //Increment to the right
                                OnPosition = OnPosition.Right();
                            }
                        }
                    }
                    else
                    {

                        PieceType p = PieceType.Pawn;

                        //Piece type
                        if (c.ToString().ToLower() == "r")
                        {
                            p = PieceType.Rook;
                        }
                        else if (c.ToString().ToLower() == "n")
                        {
                            p = PieceType.Knight;
                        }
                        else if (c.ToString().ToLower() == "b")
                        {
                            p = PieceType.Bishop;
                        }
                        else if (c.ToString().ToLower() == "q")
                        {
                            p = PieceType.Queen;
                        }
                        else if (c.ToString().ToLower() == "k")
                        {
                            p = PieceType.King;
                        }
                        else if (c.ToString().ToLower() == "p")
                        {
                            p = PieceType.Pawn;
                        }

                        //Piece color
                        bool ColorIsWhite = false;
                        if (c.ToString().ToUpper() == c.ToString()) //If the to upper version is the same as the notation itself, it is capital. And if it is capital, it is white.
                        {
                            ColorIsWhite = true;
                        }
                        else
                        {
                            ColorIsWhite = false;
                        }

                        //Make the piece and get a code for it
                        Piece tp = new Piece(ColorIsWhite, p);
                        int pieceCode = tp.ToCode();


                        //Add the piece
                        BuildingBoardState.Add(pieceCode);

                        //Advance the on position by 1
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
            string ToMoveStr = "";
            loc1 = FEN.IndexOf(" ");
            int loc2 = FEN.IndexOf(" ", loc1 + 1);
            if (loc2 > -1)
            {
               ToMoveStr = FEN.Substring(loc1 + 1, loc2 - loc1 - 1); 
            }
            else
            {
                string LastChar = FEN.Substring(FEN.Length - 1, 1);
                if (LastChar.ToLower() != "w" && LastChar.ToLower() != "b")
                {
                    throw new Exception("FEN is invalid. Last character not recognized as active color.");
                }
                ToMoveStr = LastChar;
            }
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
    }
}