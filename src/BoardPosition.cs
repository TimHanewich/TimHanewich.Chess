using System;
using System.Collections.Generic;

namespace TimHanewich.Chess
{
    public class BoardPosition
    {
        public Color ToMove {get; set;}
        public bool WhiteKingSideCastlingAvailable {get; set;}
        public bool WhiteQueenSideCastlingAvailable {get; set;}
        public bool BlackKingSideCastlingAvailable {get; set;}
        public bool BlackQueenSideCastlingAvailable {get; set;}
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
        
            //Split up the pieces by space
            string[] FenComponents = FEN.Split(new String[] {" "}, StringSplitOptions.None);

            //Get castling availability
            WhiteKingSideCastlingAvailable = false;
            WhiteQueenSideCastlingAvailable = false;
            BlackKingSideCastlingAvailable = false;
            BlackQueenSideCastlingAvailable = false;
            if (FenComponents.Length >= 3)
            {
                string CastlingComponent = FenComponents[2];
                if (CastlingComponent.Contains("K"))
                {
                    WhiteKingSideCastlingAvailable = true;
                }
                if (CastlingComponent.Contains("Q"))
                {
                    WhiteQueenSideCastlingAvailable = true;
                }
                if (CastlingComponent.Contains("k"))
                {
                    BlackKingSideCastlingAvailable = true;
                }
                if (CastlingComponent.Contains("q"))
                {
                    BlackQueenSideCastlingAvailable = true;
                }
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

            //Add castling availability
            if (WhiteKingSideCastlingAvailable == false && WhiteQueenSideCastlingAvailable == false && BlackKingSideCastlingAvailable == false && BlackQueenSideCastlingAvailable == false)
            {
                ToReturn = ToReturn + " -";
            }
            else
            {
                ToReturn = ToReturn + " ";
                if (WhiteKingSideCastlingAvailable)
                {
                    ToReturn = ToReturn + "K";
                }
                if (WhiteQueenSideCastlingAvailable)
                {
                    ToReturn = ToReturn + "Q";
                }
                if (BlackKingSideCastlingAvailable)
                {
                    ToReturn = ToReturn + "k";
                }
                if (BlackQueenSideCastlingAvailable)
                {
                    ToReturn = ToReturn + "q";
                }
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
            foreach (Piece p in _Pieces)
            {
                if (p.Position == pos)
                {
                    return true;
                }
            }
            return false;
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
                else
                {
                    Black = Black + p.Value;
                }
            }
            return White - Black;
        }

        public Move[] AvailableMoves()
        {
            return AvailableMoves(ToMove, true, false);
        }

        public Move[] AvailableMoves(bool order_moves)
        {
            return AvailableMoves(ToMove, true, true);
        }

        public Move[] AvailableMoves(Color by_color, bool CheckLegality, bool order_moves)
        {
            List<Move> ToReturn = new List<Move>();

            //Get possible moves for each piece
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

            //Castling potentially?
            if (by_color == Color.White)
            {
                if (WhiteKingSideCastlingAvailable)
                {
                    Move m = new Move();
                    m.Castling = CastlingType.KingSide;
                    ToReturn.Add(m);
                }
                if (WhiteQueenSideCastlingAvailable)
                {
                    Move m = new Move();
                    m.Castling = CastlingType.QueenSide;
                    ToReturn.Add(m);
                }
            }
            else if (by_color == Color.Black)
            {
                if (BlackKingSideCastlingAvailable)
                {
                    Move m = new Move();
                    m.Castling = CastlingType.KingSide;
                    ToReturn.Add(m);
                }
                if (BlackQueenSideCastlingAvailable)
                {
                    Move m = new Move();
                    m.Castling = CastlingType.QueenSide;
                    ToReturn.Add(m);
                }
            }

            //Order them if asked to
            if (order_moves)
            {
                return Move.OrderMovesForEvaluation(this, ToReturn.ToArray());
            }
            else
            {
                return ToReturn.ToArray();
            }
        }

        public BoardPosition Copy()
        {
            BoardPosition ToReturn = new BoardPosition();

            //Copy ToMove
            ToReturn.ToMove = ToMove;

            //Copy castling availability
            ToReturn.WhiteKingSideCastlingAvailable = WhiteKingSideCastlingAvailable;
            ToReturn.WhiteQueenSideCastlingAvailable = WhiteQueenSideCastlingAvailable;
            ToReturn.BlackKingSideCastlingAvailable = BlackKingSideCastlingAvailable;
            ToReturn.BlackQueenSideCastlingAvailable = BlackQueenSideCastlingAvailable;

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
            //Is it castling? If so, take care of it separately
            if (m.Castling.HasValue)
            {
                if (m.Castling.Value == CastlingType.KingSide && ToMove == Color.White)
                {
                    try
                    {
                        FindOccupyingPiece(Position.E1).Position = Position.G1; //Move the king
                        FindOccupyingPiece(Position.H1).Position = Position.F1;; //move the king-side rook
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unable to execute castling move: " + ex.Message);
                    }
                    WhiteKingSideCastlingAvailable = false;
                    WhiteQueenSideCastlingAvailable = false;
                }
                else if (m.Castling.Value == CastlingType.QueenSide && ToMove == Color.White)
                {
                    try
                    {
                        FindOccupyingPiece(Position.E1).Position = Position.C1; //Move the king
                        FindOccupyingPiece(Position.A1).Position = Position.D1;; //Move the queen-side rook
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unable to execute castling move: " + ex.Message);
                    }
                    WhiteKingSideCastlingAvailable = false;
                    WhiteQueenSideCastlingAvailable = false;
                }
                else if (m.Castling.Value == CastlingType.KingSide && ToMove == Color.Black)
                {
                    try
                    {
                        FindOccupyingPiece(Position.E8).Position = Position.G8; //Move the king
                        FindOccupyingPiece(Position.H8).Position = Position.F8;; //Move the king-side rook
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unable to execute castling move: " + ex.Message);
                    }
                    BlackKingSideCastlingAvailable = false;
                    BlackQueenSideCastlingAvailable = false;
                }
                else if (m.Castling.Value == CastlingType.QueenSide && ToMove == Color.Black)
                {
                    try
                    {
                        FindOccupyingPiece(Position.E8).Position = Position.C8; //Move the king
                        FindOccupyingPiece(Position.A8).Position = Position.D8;; //Move the queen-side rook
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unable to execute castling move: " + ex.Message);
                    }
                    BlackKingSideCastlingAvailable = false;
                    BlackQueenSideCastlingAvailable = false;
                }

                //Flip to-move
                if (ToMove == Color.White)
                {
                    ToMove = Color.Black;
                }
                else if (ToMove == Color.Black)
                {
                    ToMove = Color.White;
                }
                return;
            }

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
            else
            {
                ToMove = Color.White;
            }
        }
        
        public BoardPosition[] AvailableMovePositions()
        {
            Move[] moves = AvailableMoves(true);
            List<BoardPosition> ToReturn = new List<BoardPosition>();
            foreach (Move m in moves)
            {

                //If this is a pawn promotion, add all of the potential promotions (rook, bishop, knight, queen)
                if (m.IsPawnPromotion(this))
                {
                    //Create boards
                    BoardPosition PromoQueen = this.Copy();
                    BoardPosition PromoRook = this.Copy();
                    BoardPosition PromoBishop = this.Copy();
                    BoardPosition PromoKnight = this.Copy();
                    
                    //Execute
                    PromoQueen.ExecuteMove(m, PieceType.Queen);
                    PromoRook.ExecuteMove(m, PieceType.Rook);
                    PromoBishop.ExecuteMove(m, PieceType.Bishop);
                    PromoKnight.ExecuteMove(m, PieceType.Knight);

                    //Add
                    ToReturn.Add(PromoQueen);
                    ToReturn.Add(PromoRook);
                    ToReturn.Add(PromoBishop);
                    ToReturn.Add(PromoKnight);
                }
                else //If it is not a pawn promotion, execute it
                {
                    BoardPosition ThisMoveBoard = this.Copy();
                    ThisMoveBoard.ExecuteMove(m);
                    ToReturn.Add(ThisMoveBoard);
                }
                
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
                            PotentialMovesByOpponent = AvailableMoves(Color.Black, false, false);
                        }
                        else
                        {
                            PotentialMovesByOpponent = AvailableMoves(Color.White, false, false);
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
            TranspositionTable tt = new TranspositionTable();
            return Evaluate(depth, ref tt);
        }

        public float Evaluate(int depth, ref TranspositionTable tt)
        {
            float eval = EvaluateWithPruning(depth, float.MinValue, float.MaxValue, ref tt);
            return eval;
        }

        private float EvaluateWithPruning(int depth, float alpha, float beta, ref TranspositionTable tt)
        {
            //If depth is 0, return this evaluation via material difference
            if (depth == 0)
            {
                return MaterialDisparity();
            }


            //First try to retrieve from the TranspositionTable
            EvaluationPackage RetrievedEP = tt.Find(this.BoardRepresentation());
            if (RetrievedEP != null) //If we have it
            {
                if (RetrievedEP.Depth >= depth) //If the depth of what we have is equal to or deeper than what is being asked for right now
                {
                    return RetrievedEP.Evaluation;
                }
            }

            //Get to return
            float ToReturn = float.NaN;
            if (ToMove == Color.White)
            {
                float MaxEvaluationSeen = float.MinValue;
                foreach (BoardPosition bp in AvailableMovePositions())
                {
                    float eval = bp.EvaluateWithPruning(depth - 1, alpha, beta, ref tt);
                    MaxEvaluationSeen = Math.Max(MaxEvaluationSeen, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                ToReturn = MaxEvaluationSeen;
            }
            else //Black
            {
                float MinEvaluationSeen = float.MaxValue;
                foreach (BoardPosition bp in AvailableMovePositions())
                {
                    float eval = bp.EvaluateWithPruning(depth - 1, alpha, beta, ref tt);
                    MinEvaluationSeen = Math.Min(MinEvaluationSeen, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                ToReturn = MinEvaluationSeen;
            }

            //Save this position and evaluation to the TranspositionTable
            EvaluationPackage ep = new EvaluationPackage();
            ep.Depth = depth;
            ep.Evaluation = ToReturn;
            tt.Add(this.BoardRepresentation(), ep);

            //Return it!
            return ToReturn;
        }

        #endregion

        #region "Board representation generation"

        //Returns a 65-integer array. The first 64 int's are piece codes, arranged by position (A1, A2, etc). The last byte indicates the next to move. White = 0, Black = 1.
        public int[] BoardRepresentation()
        {
            //Create and prepare what to return
            int[] ToReturn = new int[65];
            for (int t = 0; t < ToReturn.Length; t++) //Set all 0 by default (nothing)
            {
                ToReturn[t] = 0;
            }

            //Populate for each piece
            foreach (Piece p in _Pieces)
            {
                ToReturn[Convert.ToInt32(p.Position)] = p.ToCode();
            }

            //To move code
            if (ToMove == Color.White)
            {
                ToReturn[64] = 0;
            }
            else
            {
                ToReturn[64] = 1;
            }

            return ToReturn;
        }

        #endregion





        /// TOOLKIT BELOW
        
        private void RemovePiece(Piece p)
        {
            _Pieces.Remove(p);
        }

    }
}