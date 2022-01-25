using System;
using System.Collections.Generic;

namespace TimHanewich.Chess
{
    public class Piece
    {
        public Color Color {get; set;}
        public PieceType Type {get; set;}
        public Position Position {get; set;}
        
        #region "Constructors"

        public Piece()
        {

        }

        public Piece(Color c, PieceType type)
        {
            Color = c;
            Type = type;
        }

        public Piece(Color c, PieceType type, Position p)
        {
            Color = c;
            Type = type;
            Position = p;
        }

        #endregion


        public float Value
        {
            get
            {
                if (Type == PieceType.Pawn)
                {
                    return 1f;
                }
                else if (Type == PieceType.Knight)
                {
                    return 3f;
                }
                else if (Type == PieceType.Bishop)
                {
                    return 3f;
                }
                else if (Type == PieceType.Rook)
                {
                    return 5f;
                }
                else if (Type == PieceType.Queen)
                {
                    return 10f;
                }
                else if (Type == PieceType.King)
                {
                    return 15f;
                }
                else
                {
                    return 0f;
                }
            }
        }
    
        public Move[] AvailableMoves(BoardPosition board, bool EnsureLegality)
        {
            List<Move> ToReturn = new List<Move>();

            if (Type == PieceType.Pawn)
            {
                if (Color == Color.White)
                {
                    //Up 1 or 2?
                    Position Up1 = Position.Up();
                    if (board.PositionIsOccupied(Up1) == false)
                    {

                        //Up 1?
                        if (Up1.Rank() == 8) //Is promotion for white
                        {
                            ToReturn.Add(new Move(Position, Up1, PieceType.Knight));
                            ToReturn.Add(new Move(Position, Up1, PieceType.Bishop));
                            ToReturn.Add(new Move(Position, Up1, PieceType.Queen));
                            ToReturn.Add(new Move(Position, Up1, PieceType.Rook));
                        }
                        else //Is not a promotion, just a normal move
                        {
                            ToReturn.Add(new Move(Position, Up1));
                        }

                        //Up 2?
                        if (Position.Rank() == 2)
                        {
                            Position Up2 = Position.Up().Up();
                            if (board.PositionIsOccupied(Position.Up().Up()) == false)
                            {
                                ToReturn.Add(new Move(Position, Up2));
                            }
                        }

                    }

                    //Capture left?
                    if (Position.File() != 'A')
                    {
                        Piece PotCap = board.FindOccupyingPiece(Position.UpLeft());
                        if (PotCap != null)
                        {
                            if (PotCap.Color == Color.Black)
                            {
                                if (PotCap.Position.Rank() == 8) //It is also a promotion
                                {
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Knight));  
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Bishop));  
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Queen));  
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Rook));  
                                }
                                else //It is not a promotion too
                                {
                                    ToReturn.Add(new Move(Position, PotCap.Position));  
                                }
                            }
                        }
                    }

                    //Capture right?
                    if (Position.File() != 'H')
                    {
                        Piece PotCap = board.FindOccupyingPiece(Position.UpRight());
                        if (PotCap != null)
                        {
                            if (PotCap.Color == Color.Black)
                            {
                                if (PotCap.Position.Rank() == 8)
                                {
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Knight));  
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Bishop));  
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Queen));  
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Rook));
                                }
                                else
                                {
                                    ToReturn.Add(new Move(Position, PotCap.Position));
                                }
                            }
                        }
                    }
                
                    //En passant capture?
                    if (board.EnPassantTarget.HasValue)
                    {                        
                        //Set up variables
                        //Just default to the current position
                        Position ForwardRight = Position;
                        Position ForwardLeft = Position;

                        //Get variables
                        if (Position.File() != 'A')
                        {
                            ForwardLeft = Position.UpLeft();
                        }
                        if (Position.File() != 'H')
                        {
                            ForwardRight = Position.UpRight();
                        }

                        //Can we reach that?
                        if (ForwardRight == board.EnPassantTarget.Value || ForwardLeft == board.EnPassantTarget.Value)
                        {
                            Move PotCap = new Move();
                            PotCap.FromPosition = Position;
                            PotCap.ToPosition = board.EnPassantTarget.Value;
                            ToReturn.Add(PotCap);
                        }
                    }
                }
                else
                {
                    //Down 1 or 2?
                    Position Down1 = Position.Down();
                    if (board.PositionIsOccupied(Down1) == false)
                    {

                        //Down 1?
                        if (Down1.Rank() == 1) //This would be a pawn promotion for black
                        {
                            ToReturn.Add(new Move(Position, Down1, PieceType.Bishop));
                            ToReturn.Add(new Move(Position, Down1, PieceType.Knight));
                            ToReturn.Add(new Move(Position, Down1, PieceType.Rook));
                            ToReturn.Add(new Move(Position, Down1, PieceType.Queen));
                        }
                        else //It is a normal pawn move (not promotion)
                        {
                            ToReturn.Add(new Move(Position, Down1));
                        }

                        //Down 2?
                        if (Position.Rank() == 7)
                        {
                            Position Down2 = Position.Down().Down();
                            if (board.PositionIsOccupied(Down2) == false)
                            {
                                ToReturn.Add(new Move(Position, Down2));
                            }
                        }
                    }

                    //Capture left?
                    if (Position.File() != 'A')
                    {
                        Piece PotCap = board.FindOccupyingPiece(Position.DownLeft());
                        if (PotCap != null)
                        {
                            if (PotCap.Color == Color.White)
                            {
                                if (PotCap.Position.Rank() == 1)
                                {
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Bishop));
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Knight));
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Rook));
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Queen));
                                }
                                else
                                {
                                    ToReturn.Add(new Move(Position, PotCap.Position));  
                                }
                            }
                        }
                    }

                    //Capture right?
                    if (Position.File() != 'H')
                    {
                        Piece PotCap = board.FindOccupyingPiece(Position.DownRight());
                        if (PotCap != null)
                        {
                            if (PotCap.Color == Color.White)
                            {
                                if (PotCap.Position.Rank() == 1)
                                {
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Bishop));
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Knight));
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Rook));
                                    ToReturn.Add(new Move(Position, PotCap.Position, PieceType.Queen));
                                }
                                else
                                {
                                    ToReturn.Add(new Move(Position, PotCap.Position));
                                }
                            }
                        }
                    }
                
                    //En passant capture?
                    if (board.EnPassantTarget.HasValue)
                    {                        
                        //Set up variables
                        //Just default to the current position
                        Position ForwardRight = Position;
                        Position ForwardLeft = Position;

                        //Get variables
                        if (Position.File() != 'A')
                        {
                            ForwardLeft = Position.DownLeft();
                        }
                        if (Position.File() != 'H')
                        {
                            ForwardRight = Position.DownRight();
                        }

                        //Can we reach that?
                        if (ForwardRight == board.EnPassantTarget.Value || ForwardLeft == board.EnPassantTarget.Value)
                        {
                            Move PotCap = new Move();
                            PotCap.FromPosition = Position;
                            PotCap.ToPosition = board.EnPassantTarget.Value;
                            ToReturn.Add(PotCap);
                        }
                    }
                
                }
            }
            else if (Type == PieceType.Knight)
            {
                //Up 2, left 1
                if (Position.Rank() < 7 && Position.File() != 'A')
                {
                    Position PotMove = Position.Up().UpLeft();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                }

                //Up 2, right 1
                if (Position.Rank() < 7 && Position.File() != 'H')
                {
                    Position PotMove = Position.Up().UpRight();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                }

                //Up 1, left 2
                if (Position.Rank() < 8 && Position.File() != 'A' && Position.File() != 'B')
                {
                    Position PotMove = Position.UpLeft().Left();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                }

                //Up 1, right 2
                if (Position.Rank() < 8 && Position.File() != 'G' && Position.File() != 'H')
                {
                    Position PotMove = Position.UpRight().Right();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                }

                //Down 1, left 2
                if (Position.Rank() > 1 && Position.File() != 'A' && Position.File() != 'B')
                {
                    Position PotMove = Position.DownLeft().Left();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                }

                //Down 1, right 2
                if (Position.Rank() > 1 && Position.File() != 'G' && Position.File() != 'H')
                {
                    Position PotMove = Position.DownRight().Right();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                }

                //Down 2, left 1
                if (Position.Rank() > 2 && Position.File() != 'A')
                {
                    Position PotMove = Position.Down().DownLeft();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                }

                //Down 2, right 1
                if (Position.Rank() > 2 && Position.File() != 'H')
                {
                    Position PotMove = Position.Down().DownRight();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                }
            }
            else if (Type == PieceType.Bishop)
            {
                List<Position> MPs = new List<Position>();
                MPs.AddRange(LinearMoves(board, 0));
                MPs.AddRange(LinearMoves(board, 1));
                MPs.AddRange(LinearMoves(board, 2));
                MPs.AddRange(LinearMoves(board, 3));
                foreach (Position pos in MPs)
                {
                    ToReturn.Add(new Move(Position, pos));
                }
            }
            else if (Type == PieceType.Rook)
            {
                List<Position> MPs = new List<Position>();
                MPs.AddRange(LinearMoves(board, 4));
                MPs.AddRange(LinearMoves(board, 5));
                MPs.AddRange(LinearMoves(board, 6));
                MPs.AddRange(LinearMoves(board, 7));
                foreach (Position pos in MPs)
                {
                    ToReturn.Add(new Move(Position, pos));
                }
            }
            else if (Type == PieceType.Queen)
            {
                List<Position> MPs = new List<Position>();
                MPs.AddRange(LinearMoves(board, 0));
                MPs.AddRange(LinearMoves(board, 1));
                MPs.AddRange(LinearMoves(board, 2));
                MPs.AddRange(LinearMoves(board, 3));
                MPs.AddRange(LinearMoves(board, 4));
                MPs.AddRange(LinearMoves(board, 5));
                MPs.AddRange(LinearMoves(board, 6));
                MPs.AddRange(LinearMoves(board, 7));
                foreach (Position pos in MPs)
                {
                    ToReturn.Add(new Move(Position, pos));
                }
            }
            else if (Type == PieceType.King)
            {
                //This algorithm is incomplete. Obviously the King cannot place himself in check. So need to evaluate each potential move here and ensure it is not putting the knight in check. if it isn't, add it as a potential move!

                //Up?
                if (Position.Rank() < 8)
                {
                    Position PotMove = Position.Up();
                    Piece OccPiece = board.FindOccupyingPiece(PotMove);
                    if (OccPiece == null)
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(new Move(Position, PotMove));
                        }
                    }
                }

                //Up right?
                if (Position.Rank() < 8 && Position.File() != 'H')
                {
                    Position PotMove = Position.UpRight();
                    Piece OccPiece = board.FindOccupyingPiece(PotMove);
                    if (OccPiece == null)
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(new Move(Position, PotMove));
                        }
                    }
                }

                //Right?
                if (Position.File() != 'H')
                {
                    Position PotMove = Position.Right();
                    Piece OccPiece = board.FindOccupyingPiece(PotMove);
                    if (OccPiece == null)
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(new Move(Position, PotMove));
                        }
                    }
                }

                //Down, right?
                if (Position.Rank() > 1 && Position.File() != 'H')
                {
                    Position PotMove = Position.DownRight();
                    Piece OccPiece = board.FindOccupyingPiece(PotMove);
                    if (OccPiece == null)
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(new Move(Position, PotMove));
                        }
                    }
                }

                //Down?
                if (Position.Rank() > 1)
                {
                    Position PotMove = Position.Down();
                    Piece OccPiece = board.FindOccupyingPiece(PotMove);
                    if (OccPiece == null)
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(new Move(Position, PotMove));
                        }
                    }
                }

                //Down, left?
                if (Position.Rank() > 1 && Position.File() != 'A')
                {
                    Position PotMove = Position.DownLeft();
                    Piece OccPiece = board.FindOccupyingPiece(PotMove);
                    if (OccPiece == null)
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(new Move(Position, PotMove));
                        }
                    }
                }

                //Left?
                if (Position.File() != 'A')
                {
                    Position PotMove = Position.Left();
                    Piece OccPiece = board.FindOccupyingPiece(PotMove);
                    if (OccPiece == null)
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(new Move(Position, PotMove));
                        }
                    }
                }

                //Up, left?
                if (Position.Rank() < 8 && Position.File() != 'A')
                {
                    Position PotMove = Position.UpLeft();
                    Piece OccPiece = board.FindOccupyingPiece(PotMove);
                    if (OccPiece == null)
                    {
                        ToReturn.Add(new Move(Position, PotMove));
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(new Move(Position, PotMove));
                        }
                    }
                }

            }

            //Filter out any moves that would be illegal
            if (EnsureLegality)
            {
                return FilterOutIllegalMoves(board, ToReturn.ToArray());
            }
            else
            {
                return ToReturn.ToArray();
            }
        }

        #region "Encoding"

        //Piece code
        //0 = empty
        //1 = White king
        //2 = White queen
        //3 = White rook
        //4 = White bishop
        //5 = White knight
        //6 = White pawn
        //7 = Black king
        //8 = Black queen
        //9 = Black rook
        //10 = Black bishop
        //11 = Black knight
        //12 = Black pawn

        public int ToCode()
        {
            
            if (Color == Color.White)
            {
                if (Type == PieceType.King)
                {
                    return 1;
                }
                else if (Type == PieceType.Queen)
                {
                    return 2;
                }
                else if (Type == PieceType.Rook)
                {
                    return 3;
                }
                else if (Type == PieceType.Bishop)
                {
                    return 4;
                }
                else if (Type == PieceType.Knight)
                {
                    return 5;
                }
                else if (Type == PieceType.Pawn)
                {
                    return 6;
                }
            }
            else
            {
                if (Type == PieceType.King)
                {
                    return 7;
                }
                else if (Type == PieceType.Queen)
                {
                    return 8;
                }
                else if (Type == PieceType.Rook)
                {
                    return 9;
                }
                else if (Type == PieceType.Bishop)
                {
                    return 10;
                }
                else if (Type == PieceType.Knight)
                {
                    return 11;
                }
                else if (Type == PieceType.Pawn)
                {
                    return 12;
                }
            }
            throw new Exception("Fatal error while converting piece to byte.");
        }

        public static Piece FromCode(int code)
        {
            switch (code)
            {
                case 0:
                    return null;
                case 1:
                    return new Piece(Color.White, PieceType.King);
                case 2:
                    return new Piece(Color.White, PieceType.Queen);
                case 3:
                    return new Piece(Color.White, PieceType.Rook);
                case 4:
                    return new Piece(Color.White, PieceType.Bishop);
                case 5:
                    return new Piece(Color.White, PieceType.Knight);
                case 6:
                    return new Piece(Color.White, PieceType.Pawn);
                case 7:
                    return new Piece(Color.Black, PieceType.King);
                case 8:
                    return new Piece(Color.Black, PieceType.Queen);
                case 9:
                    return new Piece(Color.Black, PieceType.Rook);
                case 10:
                    return new Piece(Color.Black, PieceType.Bishop);
                case 11:
                    return new Piece(Color.Black, PieceType.Knight);
                case 12:
                    return new Piece(Color.Black, PieceType.Pawn);
                default:
                    throw new Exception("Code '" + code.ToString() + "' not a valid piece code.");
            }
        }

        #endregion




        //For example, filter out moves that put the king in check
        private Move[] FilterOutIllegalMoves(BoardPosition board, Move[] moves)
        {
            List<Move> ToReturn = new List<Move>();
            foreach (Move m in moves)
            {
                bool IsLegal = MoveIsLegal(board, m);
                if (IsLegal)
                {
                    ToReturn.Add(m);
                }
            }
            return ToReturn.ToArray();
        }

        private bool MoveIsLegal(BoardPosition board, Move m)
        {
            BoardPosition copy = board.Copy();
            copy.ExecuteMove(m);

            //Executing the move above flips the color.
            //To test if the previous color that made the move was put in check, we must flip the color BACK to what it was before executing this
            //This is because the "IsCheck" method checkes if the color to move is in check (being threatened)
            if (copy.ToMove == Color.White)
            {
                copy.ToMove = Color.Black;
            }
            else if (copy.ToMove == Color.Black)
            {
                copy.ToMove = Color.White;
            }
        
            if (copy.IsCheck())
            {
                return false; //If this move put that color in check, say the move is not legal
            }
            else
            {
                return true; //if it isn't in check, this move is legal, so return true;
            }

        }


        //TOOLKIT

        //For a bishop, rook or queen to use. Direction:
        //0 = up, right
        //1 = down, right
        //2 = down, left
        //3 = up, left
        //4 = up
        //5 = right
        //6 = down
        //7 = left
        
        //Gets every position in a linear direction
        private Position[] PotentialLinearMoves(int direction)
        {
            List<Position> ToReturn = new List<Position>();

            if (direction == 0)
            {
                switch (Position)
                {

                    //A file
                    case Position.A1:
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H8);
                        break;
                    case Position.A2:
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.G8);
                        break;
                    case Position.A3:
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.F8);
                        break;
                    case Position.A4:
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.E8);
                        break;
                    case Position.A5:
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.D8);
                        break;
                    case Position.A6:
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.C8);
                        break;
                    case Position.A7:
                        ToReturn.Add(Position.B8);
                        break;

                    //B file
                    case Position.B1:
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.B2:
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H8);
                        break;
                    case Position.B3:
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.G8);
                        break;
                    case Position.B4:
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.F8);
                        break;
                    case Position.B5:
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.E8);
                        break;
                    case Position.B6:
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.D8);
                        break;
                    case Position.B7:
                        ToReturn.Add(Position.C8);
                        break;


                    //C file
                    case Position.C1:
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.C2:
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.C3:
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H8);
                        break;
                    case Position.C4:
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.G8);
                        break;
                    case Position.C5:
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.F8);
                        break;
                    case Position.C6:
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.E8);
                        break;
                    case Position.C7:
                        ToReturn.Add(Position.D8);
                        break;

                    //D file
                    case Position.D1:
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.D2:
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.D3:
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.D4:
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H8);
                        break;
                    case Position.D5:
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.G8);
                        break;
                    case Position.D6:
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.F8);
                        break;
                    case Position.D7:
                        ToReturn.Add(Position.E8);
                        break;

                    
                    //E file
                    case Position.E1:
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.E2:
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.E3:
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.E4:
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.E5:
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H8);
                        break;
                    case Position.E6:
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.G8);
                        break;
                    case Position.E7:
                        ToReturn.Add(Position.F8);
                        break;

                    //F file
                    case Position.F1:
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.F2:
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.F3:
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.F4:
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.F5:
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.F6:
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H8);
                        break;
                    case Position.F7:
                        ToReturn.Add(Position.G8);
                        break;

                    //G file
                    case Position.G1:
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.G2:
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.G3:
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.G4:
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.G5:
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.G6:
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.G7:
                        ToReturn.Add(Position.H8);
                        break;



                    //default:
                        //throw new Exception("Unable to find linear up-right move from position " + Position.ToString());

                }
            }
            else if (direction == 1)
            {
                switch (Position)
                {

                    //A file
                    case Position.A2:
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.A3:
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.A4:
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.A5:
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.A6:
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.A7:
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.A8:
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H1);
                        break;


                    //B file
                    case Position.B2:
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.B3:
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.B4:
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.B5:
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.B6:
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.B7:
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.B8:
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H2);
                        break;

                    
                    //C file
                    case Position.C2:
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.C3:
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.C4:
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.C5:
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.C6:
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.C7:
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.C8:
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H3);
                        break;

                    //D file
                    case Position.D2:
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.D3:
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.D4:
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.D5:
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.D6:
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.D7:
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.D8:
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H4);
                        break;


                    //E file
                    case Position.E2:
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.E3:
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.E4:
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.E5:
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.E6:
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.E7:
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.E8:
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H5);
                        break;

                    
                    //F file
                    case Position.F2:
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.F3:
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.F4:
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.F5:
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.F6:
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.F7:
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.F8:
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H6);
                        break;

                    //G file
                    case Position.G2:
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.G3:
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.G4:
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.G5:
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.G6:
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.G7:
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.G8:
                        ToReturn.Add(Position.H7);
                        break;

                    //default:
                        //throw new Exception("Unable to get down-right moves from position " + Position.ToString());

                }
            }
            else if (direction == 2)
            {
                switch (Position)
                {
                    //B file
                    case Position.B2:
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.B3:
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.B4:
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.B5:
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.B6:
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.B7:
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.B8:
                        ToReturn.Add(Position.A7);
                        break;

                    //C file
                    case Position.C2:
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.C3:
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.C4:
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.C5:
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.C6:
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.C7:
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.C8:
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A6);
                        break;


                    //D file
                    case Position.D2:
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.D3:
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.D4:
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.D5:
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.D6:
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.D7:
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.D8:
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A5);
                        break;

                    //E file
                    case Position.E2:
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.E3:
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.E4:
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.E5:
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.E6:
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.E7:
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.E8:
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A4);
                        break;

                    //F file
                    case Position.F2:
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.F3:
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.F4:
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.F5:
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.F6:
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.F7:
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.F8:
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A3);
                        break;


                    //G file
                    case Position.G2:
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.G3:
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.G4:
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.G5:
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.G6:
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.G7:
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.G8:
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A2);
                        break;


                    //H file
                    case Position.H2:
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.H3:
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.H4:
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.H5:
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.H6:
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.H7:
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.H8:
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A1);
                        break;

                    //default:
                        //throw new Exception("Unable to get down-left move from position " + Position.ToString());
                        
                }
            }
            else if (direction == 3)
            {
                switch  (Position)
                {

                    //B file
                    case Position.B1:
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.B2:
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.B3:
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.B4:
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.B5:
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.B6:
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.B7:
                        ToReturn.Add(Position.A8);
                        break;

                    //C file
                    case Position.C1:
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.C2:
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.C3:
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.C4:
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.C5:
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.C6:
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.C7:
                        ToReturn.Add(Position.B8);
                        break;

                    //D file
                    case Position.D1:
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.D2:
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.D3:
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.D4:
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.D5:
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.D6:
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.B8);
                        break;
                    case Position.D7:
                        ToReturn.Add(Position.C8);
                        break;


                    //E file
                    case Position.E1:
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.E2:
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.E3:
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.E4:
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.E5:
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.B8);
                        break;
                    case Position.E6:
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.C8);
                        break;
                    case Position.E7:
                        ToReturn.Add(Position.D8);
                        break;


                    //F file
                    case Position.F1:
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.F2:
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.F3:
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.F4:
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.B8);
                        break;
                    case Position.F5:
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.C8);
                        break;
                    case Position.F6:
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.D8);
                        break;
                    case Position.F7:
                        ToReturn.Add(Position.E8);
                        break;

                    //G file
                    case Position.G1:
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.G2:
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.G3:
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.B8);
                        break;
                    case Position.G4:
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.C8);
                        break;
                    case Position.G5:
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.D8);
                        break;
                    case Position.G6:
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.E8);
                        break;
                    case Position.G7:
                        ToReturn.Add(Position.F8);
                        break;



                    //H file
                    case Position.H1:
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.H2:
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.B8);
                        break;
                    case Position.H3:
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.C8);
                        break;
                    case Position.H4:
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.D8);
                        break;
                    case Position.H5:
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.E8);
                        break;
                    case Position.H6:
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.F8);
                        break;
                    case Position.H7:
                        ToReturn.Add(Position.G8);
                        break;

                    //default:
                        //throw new Exception("Unable to get up-left move from position " + Position.ToString());
                }
            }
            else if (direction == 4)
            {
                switch (Position)
                {

                    //Rank 1
                    case Position.A1:
                        ToReturn.Add(Position.A2);
                        ToReturn.Add(Position.A3);
                        ToReturn.Add(Position.A4);
                        ToReturn.Add(Position.A5);
                        ToReturn.Add(Position.A6);
                        ToReturn.Add(Position.A7);
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.B1:
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.B8);
                        break;
                    case Position.C1:
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.C8);
                        break;
                    case Position.D1:
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.D8);
                        break;
                    case Position.E1:
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.E8);
                        break;
                    case Position.F1:
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.F8);
                        break;
                    case Position.G1:
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.G8);
                        break;
                    case Position.H1:
                        ToReturn.Add(Position.H2);
                        ToReturn.Add(Position.H3);
                        ToReturn.Add(Position.H4);
                        ToReturn.Add(Position.H5);
                        ToReturn.Add(Position.H6);
                        ToReturn.Add(Position.H7);
                        ToReturn.Add(Position.H8);
                        break;

                    //Rank 2
                    case Position.A2:
                        ToReturn.Add(Position.A3);
                        ToReturn.Add(Position.A4);
                        ToReturn.Add(Position.A5);
                        ToReturn.Add(Position.A6);
                        ToReturn.Add(Position.A7);
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.B2:
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.B8);
                        break;
                    case Position.C2:
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.C8);
                        break;
                    case Position.D2:
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.D8);
                        break;
                    case Position.E2:
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.E8);
                        break;
                    case Position.F2:
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.F8);
                        break;
                    case Position.G2:
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.G8);
                        break;
                    case Position.H2:
                        ToReturn.Add(Position.H3);
                        ToReturn.Add(Position.H4);
                        ToReturn.Add(Position.H5);
                        ToReturn.Add(Position.H6);
                        ToReturn.Add(Position.H7);
                        ToReturn.Add(Position.H8);
                        break;


                    //Rank 3
                    case Position.A3:
                        ToReturn.Add(Position.A4);
                        ToReturn.Add(Position.A5);
                        ToReturn.Add(Position.A6);
                        ToReturn.Add(Position.A7);
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.B3:
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.B8);
                        break;
                    case Position.C3:
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.C8);
                        break;
                    case Position.D3:
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.D8);
                        break;
                    case Position.E3:
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.E8);
                        break;
                    case Position.F3:
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.F8);
                        break;
                    case Position.G3:
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.G8);
                        break;
                    case Position.H3:
                        ToReturn.Add(Position.H4);
                        ToReturn.Add(Position.H5);
                        ToReturn.Add(Position.H6);
                        ToReturn.Add(Position.H7);
                        ToReturn.Add(Position.H8);
                        break;



                    //Rank 4
                    case Position.A4:
                        ToReturn.Add(Position.A5);
                        ToReturn.Add(Position.A6);
                        ToReturn.Add(Position.A7);
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.B4:
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.B8);
                        break;
                    case Position.C4:
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.C8);
                        break;
                    case Position.D4:
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.D8);
                        break;
                    case Position.E4:
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.E8);
                        break;
                    case Position.F4:
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.F8);
                        break;
                    case Position.G4:
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.G8);
                        break;
                    case Position.H4:
                        ToReturn.Add(Position.H5);
                        ToReturn.Add(Position.H6);
                        ToReturn.Add(Position.H7);
                        ToReturn.Add(Position.H8);
                        break;

                    //Rank 5
                    case Position.A5:
                        ToReturn.Add(Position.A6);
                        ToReturn.Add(Position.A7);
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.B5:
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.B8);
                        break;
                    case Position.C5:
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.C8);
                        break;
                    case Position.D5:
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.D8);
                        break;
                    case Position.E5:
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.E8);
                        break;
                    case Position.F5:
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.F8);
                        break;
                    case Position.G5:
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.G8);
                        break;
                    case Position.H5:
                        ToReturn.Add(Position.H6);
                        ToReturn.Add(Position.H7);
                        ToReturn.Add(Position.H8);
                        break;



                    //Rank 6
                    case Position.A6:
                        ToReturn.Add(Position.A7);
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.B6:
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.B8);
                        break;
                    case Position.C6:
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.C8);
                        break;
                    case Position.D6:
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.D8);
                        break;
                    case Position.E6:
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.E8);
                        break;
                    case Position.F6:
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.F8);
                        break;
                    case Position.G6:
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.G8);
                        break;
                    case Position.H6:
                        ToReturn.Add(Position.H7);
                        ToReturn.Add(Position.H8);
                        break;

                    //Rank 7
                    case Position.A7:
                        ToReturn.Add(Position.A8);
                        break;
                    case Position.B7:
                        ToReturn.Add(Position.B8);
                        break;
                    case Position.C7:
                        ToReturn.Add(Position.C8);
                        break;
                    case Position.D7:
                        ToReturn.Add(Position.D8);
                        break;
                    case Position.E7:
                        ToReturn.Add(Position.E8);
                        break;
                    case Position.F7:
                        ToReturn.Add(Position.F8);
                        break;
                    case Position.G7:
                        ToReturn.Add(Position.G8);
                        break;
                    case Position.H7:
                        ToReturn.Add(Position.H8);
                        break;

                    //default:
                        //throw new Exception("Unable to get up move from position " + Position.ToString());

                }
            }
            else if (direction == 5)
            {
                switch (Position)
                {

                    //A file
                    case Position.A1:
                        ToReturn.Add(Position.B1);
                        ToReturn.Add(Position.C1);
                        ToReturn.Add(Position.D1);
                        ToReturn.Add(Position.E1);
                        ToReturn.Add(Position.F1);
                        ToReturn.Add(Position.G1);
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.A2:
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.A3:
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.A4:
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.A5:
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.A6:
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.A7:
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.A8:
                        ToReturn.Add(Position.B8);
                        ToReturn.Add(Position.C8);
                        ToReturn.Add(Position.D8);
                        ToReturn.Add(Position.E8);
                        ToReturn.Add(Position.F8);
                        ToReturn.Add(Position.G8);
                        ToReturn.Add(Position.H8);
                        break;

                    //B file
                    case Position.B1:
                        ToReturn.Add(Position.C1);
                        ToReturn.Add(Position.D1);
                        ToReturn.Add(Position.E1);
                        ToReturn.Add(Position.F1);
                        ToReturn.Add(Position.G1);
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.B2:
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.B3:
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.B4:
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.B5:
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.B6:
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.B7:
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.B8:
                        ToReturn.Add(Position.C8);
                        ToReturn.Add(Position.D8);
                        ToReturn.Add(Position.E8);
                        ToReturn.Add(Position.F8);
                        ToReturn.Add(Position.G8);
                        ToReturn.Add(Position.H8);
                        break;

                    //C file
                    case Position.C1:
                        ToReturn.Add(Position.D1);
                        ToReturn.Add(Position.E1);
                        ToReturn.Add(Position.F1);
                        ToReturn.Add(Position.G1);
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.C2:
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.C3:
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.C4:
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.C5:
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.C6:
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.C7:
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.C8:
                        ToReturn.Add(Position.D8);
                        ToReturn.Add(Position.E8);
                        ToReturn.Add(Position.F8);
                        ToReturn.Add(Position.G8);
                        ToReturn.Add(Position.H8);
                        break;

                    //D file
                    case Position.D1:
                        ToReturn.Add(Position.E1);
                        ToReturn.Add(Position.F1);
                        ToReturn.Add(Position.G1);
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.D2:
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.D3:
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.D4:
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.D5:
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.D6:
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.D7:
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.D8:
                        ToReturn.Add(Position.E8);
                        ToReturn.Add(Position.F8);
                        ToReturn.Add(Position.G8);
                        ToReturn.Add(Position.H8);
                        break;

                    //E file
                    case Position.E1:
                        ToReturn.Add(Position.F1);
                        ToReturn.Add(Position.G1);
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.E2:
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.E3:
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.E4:
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.E5:
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.E6:
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.E7:
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.E8:
                        ToReturn.Add(Position.F8);
                        ToReturn.Add(Position.G8);
                        ToReturn.Add(Position.H8);
                        break;


                    //F file
                    case Position.F1:
                        ToReturn.Add(Position.G1);
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.F2:
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.F3:
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.F4:
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.F5:
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.F6:
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.F7:
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.F8:
                        ToReturn.Add(Position.G8);
                        ToReturn.Add(Position.H8);
                        break;

                    //G file
                    case Position.G1:
                        ToReturn.Add(Position.H1);
                        break;
                    case Position.G2:
                        ToReturn.Add(Position.H2);
                        break;
                    case Position.G3:
                        ToReturn.Add(Position.H3);
                        break;
                    case Position.G4:
                        ToReturn.Add(Position.H4);
                        break;
                    case Position.G5:
                        ToReturn.Add(Position.H5);
                        break;
                    case Position.G6:
                        ToReturn.Add(Position.H6);
                        break;
                    case Position.G7:
                        ToReturn.Add(Position.H7);
                        break;
                    case Position.G8:
                        ToReturn.Add(Position.H8);
                        break;

                    //default:
                        //throw new Exception("Unable to get right move for Position " + Position.ToString());


                    
                }
            }
            else if (direction == 6)
            {
                switch (Position)
                {

                    //Rank 2
                    case Position.A2:
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.B2:
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.C2:
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.D2:
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.E2:
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.F2:
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.G2:
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.H2:
                        ToReturn.Add(Position.H1);
                        break;

                    //Rank 3
                    case Position.A3:
                        ToReturn.Add(Position.A2);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.B3:
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.C3:
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.D3:
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.E3: 
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.F3:
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.G3:
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.H3:
                        ToReturn.Add(Position.H2);
                        ToReturn.Add(Position.H1);
                        break;

                    //Rank 4
                    case Position.A4:
                        ToReturn.Add(Position.A3);
                        ToReturn.Add(Position.A2);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.B4:
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.C4:
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.D4:
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.E4: 
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.F4:
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.G4:
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.H4:
                        ToReturn.Add(Position.H3);
                        ToReturn.Add(Position.H2);
                        ToReturn.Add(Position.H1);
                        break;

                    //Rank 5
                    case Position.A5:
                        ToReturn.Add(Position.A4);
                        ToReturn.Add(Position.A3);
                        ToReturn.Add(Position.A2);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.B5:
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.C5:
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.D5:
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.E5: 
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.F5:
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.G5:
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.H5:
                        ToReturn.Add(Position.H4);
                        ToReturn.Add(Position.H3);
                        ToReturn.Add(Position.H2);
                        ToReturn.Add(Position.H1);
                        break;

                    //Rank 6
                    case Position.A6:
                        ToReturn.Add(Position.A5);
                        ToReturn.Add(Position.A4);
                        ToReturn.Add(Position.A3);
                        ToReturn.Add(Position.A2);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.B6:
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.C6:
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.D6:
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.E6: 
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.F6:
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.G6:
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.H6:
                        ToReturn.Add(Position.H5);
                        ToReturn.Add(Position.H4);
                        ToReturn.Add(Position.H3);
                        ToReturn.Add(Position.H2);
                        ToReturn.Add(Position.H1);
                        break;

                    //Rank 7
                    case Position.A7:
                        ToReturn.Add(Position.A6);
                        ToReturn.Add(Position.A5);
                        ToReturn.Add(Position.A4);
                        ToReturn.Add(Position.A3);
                        ToReturn.Add(Position.A2);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.B7:
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.C7:
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.D7:
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.E7: 
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.F7:
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.G7:
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.H7:
                        ToReturn.Add(Position.H6);
                        ToReturn.Add(Position.H5);
                        ToReturn.Add(Position.H4);
                        ToReturn.Add(Position.H3);
                        ToReturn.Add(Position.H2);
                        ToReturn.Add(Position.H1);
                        break;

                    //Rank 8
                    case Position.A8:
                        ToReturn.Add(Position.A7);
                        ToReturn.Add(Position.A6);
                        ToReturn.Add(Position.A5);
                        ToReturn.Add(Position.A4);
                        ToReturn.Add(Position.A3);
                        ToReturn.Add(Position.A2);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.B8:
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.B1);
                        break;
                    case Position.C8:
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.C1);
                        break;
                    case Position.D8:
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.D1);
                        break;
                    case Position.E8: 
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.E1);
                        break;
                    case Position.F8:
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.F1);
                        break;
                    case Position.G8:
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.G1);
                        break;
                    case Position.H8:
                        ToReturn.Add(Position.H7);
                        ToReturn.Add(Position.H6);
                        ToReturn.Add(Position.H5);
                        ToReturn.Add(Position.H4);
                        ToReturn.Add(Position.H3);
                        ToReturn.Add(Position.H2);
                        ToReturn.Add(Position.H1);
                        break;

                    //default:
                        //throw new Exception("Unable to get down move from position " + Position.ToString());
                }
            }
            else if (direction == 7)
            {
                switch (Position)
                {

                    //File H
                    case Position.H1:
                        ToReturn.Add(Position.G1);
                        ToReturn.Add(Position.F1);
                        ToReturn.Add(Position.E1);
                        ToReturn.Add(Position.D1);
                        ToReturn.Add(Position.C1);
                        ToReturn.Add(Position.B1);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.H2:
                        ToReturn.Add(Position.G2);
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.H3:
                        ToReturn.Add(Position.G3);
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.H4:
                        ToReturn.Add(Position.G4);
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.H5:
                        ToReturn.Add(Position.G5);
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.H6:
                        ToReturn.Add(Position.G6);
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.H7:
                        ToReturn.Add(Position.G7);
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.H8:
                        ToReturn.Add(Position.G8);
                        ToReturn.Add(Position.F8);
                        ToReturn.Add(Position.E8);
                        ToReturn.Add(Position.D8);
                        ToReturn.Add(Position.C8);
                        ToReturn.Add(Position.B8);
                        ToReturn.Add(Position.A8);
                        break;


                    //File G
                    case Position.G1:
                        ToReturn.Add(Position.F1);
                        ToReturn.Add(Position.E1);
                        ToReturn.Add(Position.D1);
                        ToReturn.Add(Position.C1);
                        ToReturn.Add(Position.B1);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.G2:
                        ToReturn.Add(Position.F2);
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.G3:
                        ToReturn.Add(Position.F3);
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.G4:
                        ToReturn.Add(Position.F4);
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.G5:
                        ToReturn.Add(Position.F5);
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.G6:
                        ToReturn.Add(Position.F6);
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.G7:
                        ToReturn.Add(Position.F7);
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.G8:
                        ToReturn.Add(Position.F8);
                        ToReturn.Add(Position.E8);
                        ToReturn.Add(Position.D8);
                        ToReturn.Add(Position.C8);
                        ToReturn.Add(Position.B8);
                        ToReturn.Add(Position.A8);
                        break;

                    
                    //File F
                    case Position.F1:
                        ToReturn.Add(Position.E1);
                        ToReturn.Add(Position.D1);
                        ToReturn.Add(Position.C1);
                        ToReturn.Add(Position.B1);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.F2:
                        ToReturn.Add(Position.E2);
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.F3:
                        ToReturn.Add(Position.E3);
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.F4:
                        ToReturn.Add(Position.E4);
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.F5:
                        ToReturn.Add(Position.E5);
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.F6:
                        ToReturn.Add(Position.E6);
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.F7:
                        ToReturn.Add(Position.E7);
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.F8:
                        ToReturn.Add(Position.E8);
                        ToReturn.Add(Position.D8);
                        ToReturn.Add(Position.C8);
                        ToReturn.Add(Position.B8);
                        ToReturn.Add(Position.A8);
                        break;

                    //File E
                    case Position.E1:
                        ToReturn.Add(Position.D1);
                        ToReturn.Add(Position.C1);
                        ToReturn.Add(Position.B1);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.E2:
                        ToReturn.Add(Position.D2);
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.E3:
                        ToReturn.Add(Position.D3);
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.E4:
                        ToReturn.Add(Position.D4);
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.E5:
                        ToReturn.Add(Position.D5);
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.E6:
                        ToReturn.Add(Position.D6);
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.E7:
                        ToReturn.Add(Position.D7);
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.E8:
                        ToReturn.Add(Position.D8);
                        ToReturn.Add(Position.C8);
                        ToReturn.Add(Position.B8);
                        ToReturn.Add(Position.A8);
                        break;

                    //File D
                    case Position.D1:
                        ToReturn.Add(Position.C1);
                        ToReturn.Add(Position.B1);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.D2:
                        ToReturn.Add(Position.C2);
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.D3:
                        ToReturn.Add(Position.C3);
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.D4:
                        ToReturn.Add(Position.C4);
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.D5:
                        ToReturn.Add(Position.C5);
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.D6:
                        ToReturn.Add(Position.C6);
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.D7:
                        ToReturn.Add(Position.C7);
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.D8:
                        ToReturn.Add(Position.C8);
                        ToReturn.Add(Position.B8);
                        ToReturn.Add(Position.A8);
                        break;

                    //File C
                    case Position.C1:
                        ToReturn.Add(Position.B1);
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.C2:
                        ToReturn.Add(Position.B2);
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.C3:
                        ToReturn.Add(Position.B3);
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.C4:
                        ToReturn.Add(Position.B4);
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.C5:
                        ToReturn.Add(Position.B5);
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.C6:
                        ToReturn.Add(Position.B6);
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.C7:
                        ToReturn.Add(Position.B7);
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.C8:
                        ToReturn.Add(Position.B8);
                        ToReturn.Add(Position.A8);
                        break;

                    //File B
                    case Position.B1:
                        ToReturn.Add(Position.A1);
                        break;
                    case Position.B2:
                        ToReturn.Add(Position.A2);
                        break;
                    case Position.B3:
                        ToReturn.Add(Position.A3);
                        break;
                    case Position.B4:
                        ToReturn.Add(Position.A4);
                        break;
                    case Position.B5:
                        ToReturn.Add(Position.A5);
                        break;
                    case Position.B6:
                        ToReturn.Add(Position.A6);
                        break;
                    case Position.B7:
                        ToReturn.Add(Position.A7);
                        break;
                    case Position.B8:
                        ToReturn.Add(Position.A8);
                        break;

                    //default:
                        //throw new Exception("Unable to get left move from Position " + Position.ToString());

                }
            }
            else
            {
                throw new Exception("Direction '" + direction.ToString() + "' is not a valid direction code.");
            }
            return ToReturn.ToArray();
        }

        //Get the positions this piece can move to in a linear direction
        private Position[] LinearMoves(BoardPosition board, int direction)
        {
            List<Position> ToReturn = new List<Position>();
            Position[] Line = PotentialLinearMoves(direction);
            foreach (Position p in Line)
            {
                Piece occP = board.FindOccupyingPiece(p);
                if (occP == null) //If it is empty, we can move to it
                {
                    ToReturn.Add(p);
                }
                else
                {
                    if (occP.Color != Color) //If it is an enemy color, we can go to this position. BUT can go no further. 
                    {
                        ToReturn.Add(p);
                        break;
                    }
                    else //It is one of our pieces. We can't occupy this position nor can we go any further. So break.
                    {
                        break;
                    }
                }
            }
            return ToReturn.ToArray();
        }
    
    }
}