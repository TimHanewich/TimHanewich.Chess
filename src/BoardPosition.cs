using System;
using System.Collections.Generic;

namespace TimHanewich.Chess
{
    public class BoardPosition
    {
        //To move
        public Color ToMove {get; set;}

        //Castling record
        public bool WhiteKingSideCastlingAvailable {get; set;}
        public bool WhiteQueenSideCastlingAvailable {get; set;}
        public bool BlackKingSideCastlingAvailable {get; set;}
        public bool BlackQueenSideCastlingAvailable {get; set;}

        //En passant target square
        public Position? EnPassantTarget {get; set;}

        //Pieces list
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

            //En passant target?
            if (FenComponents.Length >= 4)
            {
                string EnPassantPart = FenComponents[3];
                if (EnPassantPart != "-")
                {
                    EnPassantTarget = PositionToolkit.Parse(EnPassantPart);
                }
                else
                {
                    EnPassantTarget = null;
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

            //Add en passant
            if (EnPassantTarget.HasValue)
            {
                ToReturn = ToReturn + " " + EnPassantTarget.Value.ToString().ToLower();
            }
            else
            {
                ToReturn = ToReturn + " -";
            }

            return ToReturn;
        }

        public override string ToString()
        {
            return ToFEN();
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
                    Move[] PotMovesForPiece = p.AvailableMoves(this, CheckLegality);
                    ToReturn.AddRange(PotMovesForPiece);
                }
            }

            //Castling potentially?
            if (by_color == Color.White)
            {
                Piece KingP = FindOccupyingPiece(Position.E1);
                if (KingP != null)
                {
                    if (KingP.Color == Color.White && KingP.Type == PieceType.King)
                    {
                        Piece KSRook = FindOccupyingPiece(Position.H1);
                        Piece QSRook = FindOccupyingPiece(Position.A1);
                        if (KSRook != null)
                        {
                            if (WhiteKingSideCastlingAvailable && FindOccupyingPiece(Position.F1) == null && FindOccupyingPiece(Position.G1) == null && KSRook.Color == Color.White && KSRook.Type == PieceType.Rook)
                            {
                                //Last step: ensure nothing is attacking the squares the king moves through
                                bool PathInAttack = PositionsAttackable(Color.Black, Position.E1, Position.F1, Position.G1);
                                if (PathInAttack == false)
                                {
                                    Move m = new Move();
                                    m.Castling = CastlingType.KingSide;

                                    if (CheckLegality) //If we are being asked to check legality...
                                    {
                                        if (MoveIsLegal(m)) //Only add it if it is legal
                                        {
                                            ToReturn.Add(m);
                                        }
                                    }
                                    else //If we aren't being asked to check legality, just add it
                                    {
                                        ToReturn.Add(m);
                                    }
                                }
                            }
                        }
                        if (QSRook != null)
                        {
                            if (WhiteQueenSideCastlingAvailable && FindOccupyingPiece(Position.B1) == null && FindOccupyingPiece(Position.C1) == null && FindOccupyingPiece(Position.D1) == null && QSRook.Color == Color.White && QSRook.Type == PieceType.Rook)
                            {
                                bool PathInAttack = PositionsAttackable(Color.Black, Position.E1, Position.D1, Position.C1);
                                if (PathInAttack == false)
                                {
                                    Move m = new Move();
                                    m.Castling = CastlingType.QueenSide;
                                    
                                    if (CheckLegality) //If we are being asked to check legality...
                                    {
                                        if (MoveIsLegal(m)) //Only add it if it is legal
                                        {
                                            ToReturn.Add(m);
                                        }
                                    }
                                    else //If we aren't being asked to check legality, just add it
                                    {
                                        ToReturn.Add(m);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (by_color == Color.Black)
            {
                Piece KingP = FindOccupyingPiece(Position.E8);
                if (KingP != null)
                {
                    if (KingP.Color == Color.Black && KingP.Type == PieceType.King)
                    {
                        Piece KSRook = FindOccupyingPiece(Position.H8);
                        Piece QSRook = FindOccupyingPiece(Position.A8);
                        if (KSRook != null)
                        {
                            if (BlackKingSideCastlingAvailable && FindOccupyingPiece(Position.F8) == null && FindOccupyingPiece(Position.G8) == null && KSRook.Color == Color.Black && KSRook.Type == PieceType.Rook)
                            {
                                bool PathInAttack = PositionsAttackable(Color.White, Position.E8, Position.F8, Position.G8);
                                if (PathInAttack == false)
                                {
                                    Move m = new Move();
                                    m.Castling = CastlingType.KingSide;
                                    
                                    if (CheckLegality) //If we are being asked to check legality...
                                    {
                                        if (MoveIsLegal(m)) //Only add it if it is legal
                                        {
                                            ToReturn.Add(m);
                                        }
                                    }
                                    else //If we aren't being asked to check legality, just add it
                                    {
                                        ToReturn.Add(m);
                                    }
                                }
                            }
                        }
                        if (QSRook != null)
                        {
                            if (BlackQueenSideCastlingAvailable && FindOccupyingPiece(Position.B8) == null && FindOccupyingPiece(Position.C8) == null && FindOccupyingPiece(Position.D8) == null && QSRook.Color == Color.Black && QSRook.Type == PieceType.Rook)
                            {
                                bool PathInAttack = PositionsAttackable(Color.White, Position.E8, Position.D8, Position.C8);
                                if (PathInAttack == false)
                                {
                                    Move m = new Move();
                                    m.Castling = CastlingType.QueenSide;
                                    
                                    if (CheckLegality) //If we are being asked to check legality...
                                    {
                                        if (MoveIsLegal(m)) //Only add it if it is legal
                                        {
                                            ToReturn.Add(m);
                                        }
                                    }
                                    else //If we aren't being asked to check legality, just add it
                                    {
                                        ToReturn.Add(m);
                                    }
                                }
                            }
                        }
                    }
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

            //Copy en passant
            ToReturn.EnPassantTarget = EnPassantTarget;

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

        public void ExecuteMove(Move m)
        {
            //Is it castling? If so, take care of it separately
            if (m.Castling.HasValue)
            {
                if (m.Castling.Value == CastlingType.KingSide && ToMove == Color.White)
                {
                    //Ensure the space between is clear
                    if (FindOccupyingPiece(Position.F1) != null || FindOccupyingPiece(Position.G1) != null)
                    {
                        throw new Exception("Unable to execute white king-side castle: there are pieces in between.");
                    }

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
                    //Ensure the space between is clear
                    if (FindOccupyingPiece(Position.B1) != null || FindOccupyingPiece(Position.C1) != null || FindOccupyingPiece(Position.D1) != null)
                    {
                        throw new Exception("Unable to execute white queen-side castle: there are pieces in between.");
                    }

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
                    //Ensure the space between is clear
                    if (FindOccupyingPiece(Position.F8) != null || FindOccupyingPiece(Position.G8) != null)
                    {
                        throw new Exception("Unable to execute black king-side castle: there are pieces in between.");
                    }

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
                    //Ensure the space between is clear
                    if (FindOccupyingPiece(Position.B8) != null || FindOccupyingPiece(Position.C8) != null || FindOccupyingPiece(Position.D8) != null)
                    {
                        throw new Exception("Unable to execute black queen-side castle: there are pieces in between.");
                    }

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

            //If they moved the king or one of the rooks, make castling unavailable
            if (PieceToMove.Type == PieceType.King) //If they moved the king, casting is now TOTALLY unavailable
            {
                if (PieceToMove.Color == Color.White)
                {
                    WhiteKingSideCastlingAvailable = false;
                    WhiteQueenSideCastlingAvailable = false;
                }
                else if (PieceToMove.Color == Color.Black)
                {
                    BlackKingSideCastlingAvailable = false;
                    BlackQueenSideCastlingAvailable = false;
                }
            }
            else if (PieceToMove.Type == PieceType.Rook) //The only other move that can cause castling availability to go away is a rook move. So was it a rook move?
            {
                if (PieceToMove.Color == Color.White)
                {
                    if (PieceToMove.Position == Position.H1) //The king side rook was moved
                    {
                        WhiteKingSideCastlingAvailable = false;
                    }
                    else if (PieceToMove.Position == Position.A1) //The queen side rook was moved
                    {
                        WhiteQueenSideCastlingAvailable = false;
                    }
                }
                else if (PieceToMove.Color == Color.Black)
                {
                    if (PieceToMove.Position == Position.H8)
                    {
                        BlackKingSideCastlingAvailable = false;
                    }
                    else if (PieceToMove.Position == Position.A8)
                    {
                        BlackQueenSideCastlingAvailable = false;
                    }
                }
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
                RemovePiece(Occ); //it was a capture, so remove it from the board

                //If the piece that was captured was a rook, check if the rook was in a castling position. And if it was, turn off castling availability
                if (Occ.Type == PieceType.Rook)
                {
                    if (Occ.Color == Color.White)
                    {
                        if (Occ.Position == Position.A1)
                        {
                            WhiteQueenSideCastlingAvailable = false;
                        }
                        else if (Occ.Position == Position.H1)
                        {
                            WhiteKingSideCastlingAvailable = false;
                        }
                    }
                    else if (Occ.Color == Color.Black)
                    {
                        if (Occ.Position == Position.A8)
                        {
                            BlackQueenSideCastlingAvailable = false;
                        }
                        else if (Occ.Position == Position.H8)
                        {
                            BlackKingSideCastlingAvailable = false;
                        }
                    }
                }
            }
            PieceToMove.Position = m.ToPosition;

            
            //Is this was a pawn promotion, now promote the pawn
            if (IsPawnPromo)
            {

                //Check for illegal
                if (m.PromotePawnTo == PieceType.Pawn)
                {
                    throw new Exception("Invalid move. Unable to promote pawn to pawn.");
                }
                else if (m.PromotePawnTo == PieceType.King)
                {
                    throw new Exception("Invalid move. Unable to promote pawn to king.");
                }
                
                //Do the promotion
                PieceToMove.Type = m.PromotePawnTo;
            }


            //Is it a pawn move that potentiall could cause an en passant target square?
            bool EnPassantTargetCreated = false;
            if (PieceToMove.Type == PieceType.Pawn)
            {
                if (Math.Abs(m.ToPosition.Rank() - m.FromPosition.Rank()) == 2) //If it was a move of two
                {
                    //Get the positions that may have an enemy pawn that could attack via en passant
                    Position[] PotentialEnPassantAttackOrigins = PotentialEnPassantAttackerPositions(m.ToPosition);
                    foreach (Position pos in PotentialEnPassantAttackOrigins)
                    {
                        Piece PotentialEnPassantAttacker = FindOccupyingPiece(pos);
                        if (PotentialEnPassantAttacker != null)
                        {
                            if (PotentialEnPassantAttacker.Type == PieceType.Pawn) //It is a pawn
                            {
                                if (PotentialEnPassantAttacker.Color != ToMove) //It is an enemy pawn
                                {

                                    //There is a potential en passant capture available!

                                    if (ToMove == Color.White) // if it is white, the en passant capture would be one below where the pawn that moved two forward moved to
                                    {
                                        EnPassantTarget = m.ToPosition.Down();
                                    }
                                    else if (ToMove == Color.Black) //Inverse for black
                                    {
                                        EnPassantTarget = m.ToPosition.Up();
                                    }

                                    //Flag that an en passant square was created
                                    EnPassantTargetCreated = true;

                                }
                            }
                        }
                    }
                }
            }

            //Detect if this was an en passant capture? If it was an en passant move, find and remove the captured pawn
            if (EnPassantTarget.HasValue)
            {
                if (PieceToMove.Type == PieceType.Pawn)
                {
                    if (m.ToPosition == EnPassantTarget.Value) //If this pawn moved to the en passant target square, it is an en passant move
                    {

                        //We are here, so it was an en passant move.
                        //We must find the position of the pawn that was taken as a result of en passant

                        //Get the position to search for the pawn that will be taken as a result of En Passant
                        Position PositionOfPawnBeingTaken = m.ToPosition;
                        if (ToMove == Color.White)
                        {
                            PositionOfPawnBeingTaken = m.ToPosition.Down();
                        }
                        else if (ToMove == Color.Black)
                        {
                            PositionOfPawnBeingTaken = m.ToPosition.Up();
                        }

                        //Find the pawn
                        Piece CapturedPawn = FindOccupyingPiece(PositionOfPawnBeingTaken);
                        if (CapturedPawn == null)
                        {
                            throw new Exception("Unable to execute detect en passant move. Unable to find captured piece.");
                        }

                        //Remove the pawn
                        RemovePiece(CapturedPawn);
                    }
                }
            }


            //If an en passant square was not CREATED by this move, turn it off.
            if (EnPassantTargetCreated == false)
            {
                EnPassantTarget = null;
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
                BoardPosition ThisMoveBoard = this.Copy();
                ThisMoveBoard.ExecuteMove(m);
                ToReturn.Add(ThisMoveBoard); 
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
            return Evaluate(depth, tt);
        }

        public float Evaluate(int depth, TranspositionTable tt)
        {
            float eval = EvaluateWithPruning(depth, float.MinValue, float.MaxValue, tt);
            return eval;
        }

        private float EvaluateWithPruning(int depth, float alpha, float beta, TranspositionTable tt)
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
                    float eval = bp.EvaluateWithPruning(depth - 1, alpha, beta, tt);
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
                    float eval = bp.EvaluateWithPruning(depth - 1, alpha, beta, tt);
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

        public static BoardPosition FromBoardRepresentation(int[] rep)
        {
            BoardPosition ToReturn = new BoardPosition();

            //Get a list of pieces
            for (int pos = 0; pos < 64; pos++) //Go through the first 64 integers in the array (64 squares)
            {
                Piece ThisDecodedPiece = Piece.FromCode(rep[pos]);
                if (ThisDecodedPiece != null) //This is actually a piece. NOT a blank square (If it is null, it is a blank square)
                {
                    ThisDecodedPiece.Position = (Position)pos;
                    ToReturn._Pieces.Add(ThisDecodedPiece);
                }
            }

            //Who is to move?
            if (rep[64] == 0)
            {
                ToReturn.ToMove = Color.White;
            }
            else if (rep[64] == 1)
            {
                ToReturn.ToMove = Color.Black;
            }

            return ToReturn;
        }

        #endregion

        #region "Legality check"

        //Only checks if the move puts the king in check (cannot make that move)
        public bool MoveIsLegal(Move m)
        {
            BoardPosition test = this.Copy();
            test.ExecuteMove(m);

            //Flip the to-move (because the IsCheck method only checks if the current color to move is being theatened. We want to check if the move that was just made puts that color in check)
            if (test.ToMove == Color.White)
            {
                test.ToMove = Color.Black;
            }
            else if (test.ToMove == Color.Black)
            {
                test.ToMove = Color.White;
            }

            //Is the color that just moved in check?
            bool InCheck = test.IsCheck();

            return !InCheck;
        }

        #endregion


        #region "Add/Remove pieces"

        public void RemovePiece(Piece p)
        {
            _Pieces.Remove(p);
        }

        public void AddPiece(Piece p)
        {
            _Pieces.Add(p);
        }

        #endregion


        #region "toolkit"

        

        //For example, for a white pawn that just moved two squares forward to square B4, this would return A4 and C4, because those are the two squares that need to be checked for black pawns that could threaten this white pawn
        private Position[] PotentialEnPassantAttackerPositions(Position ForPawnOn)
        {
            switch (ForPawnOn)
            {
                case Position.A4:
                    return new Position[]{Position.B4};
                case Position.B4:
                    return new Position[]{Position.A4, Position.C4};
                case Position.C4:
                    return new Position[]{Position.B4, Position.D4};
                case Position.D4:
                    return new Position[]{Position.C4, Position.E4};
                case Position.E4:
                    return new Position[]{Position.D4, Position.F4};
                case Position.F4:
                    return new Position[]{Position.E4, Position.G4};
                case Position.G4:
                    return new Position[]{Position.F4, Position.H4};
                case Position.H4:
                    return new Position[]{Position.G4};
                case Position.A5:
                    return new Position[]{Position.B5};
                case Position.B5:
                    return new Position[]{Position.A5, Position.C5};
                case Position.C5:
                    return new Position[]{Position.B5, Position.D5};
                case Position.D5:
                    return new Position[]{Position.C5, Position.E5};
                case Position.E5:
                    return new Position[]{Position.D5, Position.F5};
                case Position.F5:
                    return new Position[]{Position.E5, Position.G5};
                case Position.G5:
                    return new Position[]{Position.F5, Position.H5};
                case Position.H5:
                    return new Position[]{Position.G5};
                default:
                    throw new Exception("There are no en passant risk squares for a pawn on " + ForPawnOn.ToString());
            }

        }

        //If any of the positions are attackable by the color, it will return true.
        //If none are attackable by the color, it will return false
        private bool PositionsAttackable(Color by_color, params Position[] positions)
        {
            foreach (Piece p in Pieces)
            {
                if (p.Color == by_color)
                {
                    //Get moves
                    Move[] moves = null;
                    try
                    {
                        moves = p.AvailableMoves(this, false); //False to check legality, because a white long castle is not allowed in this position: r3k2r/8/2r5/1B6/8/8/8/R3K2R w KQkq - 0 1
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("There was a fatal error while generating moves to check if positions are attackble: " + ex.Message);
                    }
                    
                    //Check each
                    foreach (Position pos in positions)
                    {
                        foreach (Move m in moves)
                        {
                            if (m.ToPosition == pos) //A move by this piece of this color can attack one of the positions that was listed to test. So return true
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            //We made it here... so false!
            return false;
        }


        #endregion

    }
}