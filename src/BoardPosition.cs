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

        public Piece[] Pieces
        {
            get
            {
                return _Pieces.ToArray();
            }
        }

        public string ToFEN()
        {
            string ToReturn = "";
            
            //Assemble
            int BlankBuffer = 0;
            int CurrentRank = 8;
            foreach (Position pos in PositionToolkit.FenOrder())
            {
                
                //If we are now on a new rank, add the buffer and then slash
                if (pos.Rank() != CurrentRank)
                {
                    if (BlankBuffer > 0)
                    {
                        ToReturn = ToReturn + BlankBuffer.ToString();
                        BlankBuffer = 0;
                    }
                    ToReturn = ToReturn + "/";
                }

                CurrentRank = pos.Rank();

                //Add to the buffer or add the piece itself.
                Piece p = FindOccupyingPiece(pos);
                if (p == null)
                {
                    BlankBuffer = BlankBuffer + 1;
                }
                else
                {
                    //First, if there is a buffer, add it
                    if (BlankBuffer > 0)
                    {
                        ToReturn = ToReturn + BlankBuffer.ToString();
                        BlankBuffer = 0;
                    }

                    //To add
                    string ToAdd = "";
                    if (p.Type == PieceType.Pawn)
                    {
                        ToAdd = "P";
                    }
                    else if (p.Type == PieceType.Knight)
                    {
                        ToAdd = "N";
                    }
                    else if (p.Type == PieceType.Bishop)
                    {
                        ToAdd = "B";
                    }
                    else if (p.Type == PieceType.Queen)
                    {
                        ToAdd = "Q";
                    }
                    else if (p.Type == PieceType.King)
                    {
                        ToAdd = "K";
                    }
                    else if (p.Type == PieceType.Rook)
                    {
                        ToAdd = "R";
                    }

                    //Convert to black?
                    if (p.Color == Color.Black)
                    {
                        ToAdd = ToAdd.ToLower();
                    }

                    //Add it
                    ToReturn = ToReturn + ToAdd;
                    
                }
            }

            //Finally, add any remaining white space at the end if some existds
            if (BlankBuffer > 0)
            {
                ToReturn = ToReturn + BlankBuffer.ToString();
                BlankBuffer = 0;
            }

            //Add next to move
            if (ToMove == Color.White)
            {
                ToReturn = ToReturn + " w";
            }
            else
            {
                ToReturn = ToReturn + " b";
            }

            return ToReturn;
        }

        public Piece FindOccupyingPiece(Position pos)
        {
            foreach (Piece p in _Pieces)
            {
                if (pos == p.Position)
                {
                    return p;
                }
            }
            return null; //Return null if nothing found.
        }

        public bool PositionIsOccupied(Position pos)
        {
            Piece p = FindOccupyingPiece(pos);
            if (p == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public float MaterialDisparity()
        {
            float White = 0f;
            float Black = 0f;
            foreach (Piece p in _Pieces)
            {
                if (p.Color == Color.White)
                {
                    White = White + p.Value;
                }
                else if (p.Color == Color.Black)
                {
                    Black = Black + p.Value;
                }
            }
            return White - Black;
        }

        public Move[] AvailableMoves()
        {
            return AvailableMoves(ToMove, true);
        }

        public Move[] AvailableMoves(Color by_color, bool CheckLegality)
        {
            List<Move> ToReturn = new List<Move>();

            foreach (Piece p in _Pieces)
            {
                if (p.Color == by_color)
                {
                    Position[] PosMovesForPiece = p.AvailableMoves(this, CheckLegality);
                    foreach (Position PotMove in PosMovesForPiece)
                    {
                        Move m = new Move();
                        m.FromPosition = p.Position;
                        m.ToPosition = PotMove;
                        ToReturn.Add(m);
                    }
                }
            }

            return ToReturn.ToArray();
        }

        public BoardPosition Copy()
        {
            BoardPosition ToReturn = new BoardPosition();

            //Copy ToMove
            ToReturn.ToMove = ToMove;

            //Copy each piece
            foreach (Piece p in _Pieces)
            {
                Piece NP = new Piece();
                NP.Color = p.Color;
                NP.Position = p.Position;
                NP.Type = p.Type;
                ToReturn._Pieces.Add(NP);
            }

            return ToReturn;
        }

        public void ExecuteMove(Move m, PieceType promote_pawn_to = PieceType.Queen)
        {
            //Get piece to move
            Piece PieceToMove = FindOccupyingPiece(m.FromPosition);
            if (PieceToMove == null)
            {
                throw new Exception("Move " + m.FromPosition.ToString() + " to " + m.ToPosition.ToString() + " is invalid. No piece was occupying " + m.FromPosition.ToString());
            }

            //First, If this is a pawn move, check if there is a pawn promotion
            bool IsPawnPromo = false;
            if (PieceToMove.Type == PieceType.Pawn)
            {
                IsPawnPromo = m.IsPawnPromotion(this);
            }

            //Move & Capture if necessary
            Piece Occ = FindOccupyingPiece(m.ToPosition);
            if (Occ != null)
            {
                RemovePiece(Occ); //it was a capture
            }
            PieceToMove.Position = m.ToPosition;

            //Is this was a pawn move, now promote the pawn
            if (IsPawnPromo)
            {

                //Check for illegal
                if (promote_pawn_to == PieceType.Pawn)
                {
                    throw new Exception("Invalid move. Unable to promote pawn to pawn.");
                }
                else if (promote_pawn_to == PieceType.King)
                {
                    throw new Exception("Invalid move. Unable to promote pawn to king.");
                }
                
                //Do the promotion
                PieceToMove.Type = promote_pawn_to;
            }


            //Flip ToMove
            if (PieceToMove.Color == Color.White)
            {
                ToMove = Color.Black;
            }
            else if (PieceToMove.Color == Color.Black)
            {
                ToMove = Color.White;
            }
        }
        
        public BoardPosition[] AvailableMovePositions()
        {
            Move[] moves = AvailableMoves();
            List<BoardPosition> ToReturn = new List<BoardPosition>();
            foreach (Move m in moves)
            {
                BoardPosition ThisMove = this.Copy();
                ThisMove.ExecuteMove(m);
                ToReturn.Add(ThisMove);
            }
            return ToReturn.ToArray();
        }

        public string[] AvailableMovePositionsFEN()
        {
            BoardPosition[] pos = AvailableMovePositions();
            List<string> ToReturn = new List<string>();
            foreach (BoardPosition bp in pos)
            {
                ToReturn.Add(bp.ToFEN());
            }
            return ToReturn.ToArray();
        }


        //Will only check if the current color to move is in check.
        public bool IsCheck()
        {
            //Is the king at risk right now? Is another piece threatening capture?
            
            //Find the king's position
            foreach (Piece p in _Pieces)
            {
                if (p.Color == ToMove)
                {
                    if (p.Type == PieceType.King)
                    {
                        Move[] PotentialMovesByOpponent = null;
                        if (ToMove == Color.White)
                        {
                            PotentialMovesByOpponent = AvailableMoves(Color.Black, false);
                        }
                        else
                        {
                            PotentialMovesByOpponent = AvailableMoves(Color.White, false);
                        }

                        //If any of the moves of the opponent are to my kinds position, I am in check
                        bool InCheck = false;
                        foreach (Move m in PotentialMovesByOpponent)
                        {
                            if (m.ToPosition == p.Position)
                            {
                                InCheck = true;
                            }
                        }
                        return InCheck;
                    }
                }
            }

            return false;
        }

        public bool IsCheckMate()
        {
            if (IsCheck() == false)
            {
                return false;
            }
            else //Since we are in check, see if there are any moves that we could play that would put us out of check.
            {
                BoardPosition[] PotentialNewPositions = AvailableMovePositions();
                bool IsWayOut = false;
                foreach (BoardPosition bp in PotentialNewPositions)
                {
                    if (bp.IsCheck() == false)
                    {
                        IsWayOut = true;
                    }
                }
                
                //if there is a way out of the current check, return false
                if (IsWayOut)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        #region "EVALUATION"

        public float Evaluate(int depth)
        {
            float eval = EvaluateWithPruning(depth, float.MinValue, float.MaxValue);
            return eval;
        }

        private float EvaluateWithPruning(int depth, float alpha, float beta)
        {
            //If depth is 0, return this evaluation via material difference
            if (depth == 0)
            {
                return MaterialDisparity();
            }

            if (ToMove == Color.White)
            {
                float MaxEvaluationSeen = float.MinValue;
                foreach (BoardPosition bp in AvailableMovePositions())
                {
                    float eval = bp.EvaluateWithPruning(depth - 1, alpha, beta);
                    MaxEvaluationSeen = Math.Max(MaxEvaluationSeen, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return MaxEvaluationSeen;
            }
            else //Black
            {
                float MinEvaluationSeen = float.MaxValue;
                foreach (BoardPosition bp in AvailableMovePositions())
                {
                    float eval = bp.EvaluateWithPruning(depth - 1, alpha, beta);
                    MinEvaluationSeen = Math.Min(MinEvaluationSeen, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return MinEvaluationSeen;
            }
        }

        #endregion


        /// TOOLKIT BELOW
        
        private void RemovePiece(Piece p)
        {
            _Pieces.Remove(p);
        }

    }
}