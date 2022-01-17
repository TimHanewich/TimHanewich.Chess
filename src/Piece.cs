using System;
using System.Collections.Generic;

namespace TimHanewich.Chess
{
    public class Piece
    {
        public Color Color {get; set;}
        public PieceType Type {get; set;}
        public Position Position {get; set;}
        
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
    
        public Position[] AvailableMoves(BoardPosition board, bool EnsureLegality)
        {
            List<Position> ToReturn = new List<Position>();

            if (Type == PieceType.Pawn)
            {
                if (Color == Color.White)
                {
                    //Up 1?
                    if (board.PositionIsOccupied(Position.Up()) == false)
                    {
                        ToReturn.Add(Position.Up());
                    }

                    //Up 2?
                    if (Position.Rank() == 2)
                    {
                        if (board.PositionIsOccupied(Position.Up().Up()) == false)
                        {
                            ToReturn.Add(Position.Up().Up());
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
                                ToReturn.Add(PotCap.Position);
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
                                ToReturn.Add(PotCap.Position);
                            }
                        }
                    }
                }
                else
                {
                    //Down 1?
                    if (board.PositionIsOccupied(Position.Down()) == false)
                    {
                        ToReturn.Add(Position.Down());
                    }

                    //Down 2?
                    if (Position.Rank() == 7)
                    {
                        if (board.PositionIsOccupied(Position.Down().Down()) == false)
                        {
                            ToReturn.Add(Position.Down().Down());
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
                                ToReturn.Add(PotCap.Position);
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
                                ToReturn.Add(PotCap.Position);
                            }
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
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Up 2, right 1
                if (Position.Rank() < 7 && Position.File() != 'H')
                {
                    Position PotMove = Position.Up().UpRight();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Up 1, left 2
                if (Position.Rank() < 8 && Position.File() != 'A' && Position.File() != 'B')
                {
                    Position PotMove = Position.UpLeft().Left();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Up 1, right 2
                if (Position.Rank() < 8 && Position.File() != 'G' && Position.File() != 'H')
                {
                    Position PotMove = Position.UpRight().Right();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Down 1, left 2
                if (Position.Rank() > 1 && Position.File() != 'A' && Position.File() != 'B')
                {
                    Position PotMove = Position.DownLeft().Left();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Down 1, right 2
                if (Position.Rank() > 1 && Position.File() != 'G' && Position.File() != 'H')
                {
                    Position PotMove = Position.DownRight().Right();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Down 2, left 1
                if (Position.Rank() > 2 && Position.File() != 'A')
                {
                    Position PotMove = Position.Down().DownLeft();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Down 2, right 1
                if (Position.Rank() > 2 && Position.File() != 'H')
                {
                    Position PotMove = Position.Down().DownRight();
                    Piece Occ = board.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Color != Color) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }
            }
            else if (Type == PieceType.Bishop)
            {
                ToReturn.AddRange(PotentialLinearMoves(board, 0));
                ToReturn.AddRange(PotentialLinearMoves(board, 1));
                ToReturn.AddRange(PotentialLinearMoves(board, 2));
                ToReturn.AddRange(PotentialLinearMoves(board, 3));
            }
            else if (Type == PieceType.Rook)
            {
                ToReturn.AddRange(PotentialLinearMoves(board, 4));
                ToReturn.AddRange(PotentialLinearMoves(board, 5));
                ToReturn.AddRange(PotentialLinearMoves(board, 6));
                ToReturn.AddRange(PotentialLinearMoves(board, 7));
            }
            else if (Type == PieceType.Queen)
            {
                ToReturn.AddRange(PotentialLinearMoves(board, 0));
                ToReturn.AddRange(PotentialLinearMoves(board, 1));
                ToReturn.AddRange(PotentialLinearMoves(board, 2));
                ToReturn.AddRange(PotentialLinearMoves(board, 3));
                ToReturn.AddRange(PotentialLinearMoves(board, 4));
                ToReturn.AddRange(PotentialLinearMoves(board, 5));
                ToReturn.AddRange(PotentialLinearMoves(board, 6));
                ToReturn.AddRange(PotentialLinearMoves(board, 7));
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
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(PotMove);
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
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(PotMove);
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
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(PotMove);
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
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(PotMove);
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
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(PotMove);
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
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(PotMove);
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
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(PotMove);
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
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Color != Color)
                        {
                            ToReturn.Add(PotMove);
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

        public int ToCode()
        {
            
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

        //For example, filter out moves that put the king in check
        private Position[] FilterOutIllegalMoves(BoardPosition board, Position[] moves)
        {
            List<Position> ToReturn = new List<Position>();
            foreach (Position pos in moves)
            {
                bool IsLegal = MoveIsLegal(board, pos);
                if (IsLegal)
                {
                    ToReturn.Add(pos);
                }
            }
            return ToReturn.ToArray();
        }

        private bool MoveIsLegal(BoardPosition board, Position destination)
        {
            BoardPosition copy = board.Copy();
            Move m = new Move();
            m.FromPosition = Position;
            m.ToPosition = destination;
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
        private Position[] PotentialLinearMoves(BoardPosition board, int direction)
        {
            List<Position> ToReturn = new List<Position>();
            bool StopCollecting = false;
            Position OnPosition = Position; //Starting position
            while (StopCollecting == false)
            {
                //Increment to next position
                if (direction == 0)
                {
                    if (OnPosition.Rank() < 8 && OnPosition.File() != 'H')
                    {
                        OnPosition = OnPosition.UpRight();
                    }
                    else
                    {
                        StopCollecting = true;
                    }
                }
                else if (direction == 1)
                {
                    if (OnPosition.Rank() > 1 && OnPosition.File() != 'H')
                    {
                        OnPosition = OnPosition.DownRight();
                    }
                    else
                    {
                        StopCollecting = true;
                    }
                }
                else if (direction == 2)
                {
                    if (OnPosition.Rank() > 1 && OnPosition.File() != 'A')
                    {
                        OnPosition = OnPosition.DownLeft();
                    }
                    else
                    {
                        StopCollecting = true;
                    }
                }
                else if (direction == 3)
                {
                    if (OnPosition.Rank() < 8 && OnPosition.File() != 'A')
                    {
                        OnPosition = OnPosition.UpLeft();
                    }
                    else
                    {
                        StopCollecting = true;
                    }
                }
                else if (direction == 4)
                {
                    if (OnPosition.Rank() < 8)
                    {
                        OnPosition = OnPosition.Up();
                    }
                    else
                    {
                        StopCollecting = true;
                    }
                }
                else if (direction == 5)
                {
                    if (OnPosition.File() != 'H')
                    {
                        OnPosition = OnPosition.Right();
                    }
                    else
                    {
                        StopCollecting = true;
                    }
                }
                else if (direction == 6)
                {
                    if (OnPosition.Rank() > 1)
                    {
                        OnPosition = OnPosition.Down();
                    }
                    else
                    {
                        StopCollecting = true;
                    }
                }
                else if (direction == 7)
                {
                    if (OnPosition.File() != 'A')
                    {
                        OnPosition = OnPosition.Left();
                    }
                    else
                    {
                        StopCollecting = true;
                    }
                }


                //Add and move on or stop here?
                if (StopCollecting == false)
                {
                    Piece OccupyingPiece = board.FindOccupyingPiece(OnPosition);
                    if (OccupyingPiece == null) //if the position is empty, add it and move on
                    {
                        ToReturn.Add(OnPosition);
                    }
                    else
                    {
                        if (OccupyingPiece.Color != Color) //It is occupied by an opposing piece. So we can capture it. Add it
                        {
                            ToReturn.Add(OnPosition);
                            StopCollecting = true; //Stop collecting (don't go further because we cannot pass the piece)
                        }
                        else //It is occupied by the same color. We can't take our own piece so stop.
                        {
                            StopCollecting = true;
                        }
                    }
                }
            }
            return ToReturn.ToArray();            
        }
    
        private Position[] PotentialLinearMovesV2(BoardPosition board, int direction)
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



                    default:
                        throw new Exception("Unable to find linear up-right move from position " + Position.ToString());

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

                    default:
                        throw new Exception("Unable to get down-right moves from position " + Position.ToString());

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

                    default:
                        throw new Exception("Unable to get down-left move from position " + Position.ToString());
                        
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

                    default:
                        throw new Exception("Unable to get up-left move from position " + Position.ToString());
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

                    default:
                        throw new Exception("Unable to get up move from position " + Position.ToString());

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

                    default:
                        throw new Exception("Unable to get right move for Position " + Position.ToString());


                    
                }
            }
        }
    }
}