using System;
using System.Collections.Generic;

namespace TimHanewich.Chess.Experimental
{
    public struct GameState
    {
        private int[] BoardState;
        public bool WhiteToMove {get; set;}

        public GameState(int[] board_state, bool white_to_move)
        {
            BoardState = board_state;
            WhiteToMove = white_to_move;
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
            BoardState = new int[64];
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
                                BoardState[OnPosition.PositionToArrayPosition()] = 0;

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
                        BoardState[OnPosition.PositionToArrayPosition()] = pieceCode;

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
                WhiteToMove = true;
            }
            else if (ToMoveStr.ToLower() == "b")
            {
                WhiteToMove = false;
            }
            else
            {
                throw new Exception("Active color '" + ToMoveStr + "' not recognized in FEN.");
            }
        }
    
        public string Print()
        {
            string ToReturn = "┌────────┐" + Environment.NewLine + "|";
            int OnRank = 8;
            foreach (Position p in PositionToolkit.FenOrder())
            {

                //If we are on a new row now, add a new line
                if (p.Rank() != OnRank)
                {
                    ToReturn = ToReturn + "|" + " " + (p.Rank() + 1).ToString("#,##0") + Environment.NewLine + "|";
                    OnRank = p.Rank();
                }


                int Code = BoardState[p.PositionToArrayPosition()];
                Piece? ThisPiece = Code.ToPiece();
                if (ThisPiece.HasValue)
                {
                    string PieceNotation = ChessToolkit.GetPieceNotation(ThisPiece.Value.Type);

                    //If the piece notation is blank, it means it is a pawn. this is because in algebraic notation, a pawn has not piece notation.
                    if (PieceNotation == "")
                    {
                        PieceNotation = "p";
                    }

                    //Capital or lowercase based on color
                    if (ThisPiece.Value.IsWhite)
                    {
                        PieceNotation = PieceNotation.ToUpper();
                    }
                    else
                    {
                        PieceNotation = PieceNotation.ToLower();
                    }

                    //Mark it
                    ToReturn = ToReturn + PieceNotation;
                }
                else
                {
                    ToReturn = ToReturn + " ";
                }
            }

            //Finish it off
            ToReturn = ToReturn + "| 1" + Environment.NewLine + "└────────┘";

            //Lower letters
            ToReturn = ToReturn + Environment.NewLine + " ABCDEFGH ";

            return ToReturn;
        }
    
        public Piece? FindOccupyingPiece(Position p)
        {
            return BoardState[p.PositionToArrayPosition()].ToPiece();
        }
    
        public bool PositionIsOccupied(Position p)
        {
            return FindOccupyingPiece(p).HasValue;
        }
    
        public Move[] PotentialNextMoves()
        {
            List<Move> ToReturn = new List<Move>();
            for (int arrPos = 0; arrPos < BoardState.Length; arrPos++)
            {
                Piece? ThisPiece = BoardState[arrPos].ToPiece();
                if (ThisPiece.HasValue)
                {
                    if (ThisPiece.Value.IsWhite == WhiteToMove)
                    {
                        Position[] PotentialMovesForThisPiece = Piece.AvailableMoves(this, ThisPiece.Value, arrPos.ArrayPositionToPosition());
                        foreach (Position potMove in PotentialMovesForThisPiece)
                        {
                            Move PotMoveToMake = new Move(arrPos.ArrayPositionToPosition(), potMove);
                            ToReturn.Add(PotMoveToMake);
                        }
                    }
                }
            }
            return ToReturn.ToArray();
        }

        public GameState[] PotentialNextStates()
        {
            List<GameState> ToReturn = new List<GameState>();
            foreach (Move m in PotentialNextMoves())
            {
                GameState NGS = this.Clone();
                NGS.ExecuteMove(m);
                ToReturn.Add(NGS);
            }
            return ToReturn.ToArray();
        }

        public float MaterialDisparity()
        {
            float White = 0f;
            float Black = 0f;
            foreach (int i in BoardState)
            {
                Piece? p = i.ToPiece();
                if (p.HasValue)
                {
                    if (p.Value.IsWhite)
                    {
                        White = White + p.Value.Value;
                    }
                    else
                    {
                        Black = Black + p.Value.Value;
                    }
                }
            }
            return White - Black;
        }


        public void ExecuteMove(Move m)
        {
            Piece? ToMove = FindOccupyingPiece(m.Origin);
            if (ToMove.HasValue == false)
            {
                throw new Exception("Unable to execute move. Move is invalid! A piece is not occupying the origin position");
            }

            //Mark the origin as empty now
            BoardState[m.Origin.PositionToArrayPosition()] = 0; //0 means an empty square

            //Move the piece to the new square
            BoardState[m.Destination.PositionToArrayPosition()] = ToMove.Value.ToCode();
        }

        public GameState Clone()
        {
            int[] NewBoardState = (int[])BoardState.Clone();
            return new GameState(NewBoardState, WhiteToMove);
        }

    }
}