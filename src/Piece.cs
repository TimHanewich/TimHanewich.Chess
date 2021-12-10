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
    
        public Position[] AvailableMoves(BoardPosition board)
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
                        Piece PotCap = board.FindOccupyingPiece(Position.Up().Left());
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
                        Piece PotCap = board.FindOccupyingPiece(Position.Up().Right());
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
                        Piece PotCap = board.FindOccupyingPiece(Position.Down().Left());
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
                        Piece PotCap = board.FindOccupyingPiece(Position.Down().Right());
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
                    Position PotMove = Position.Up().Up().Left();
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
                    Position PotMove = Position.Up().Up().Right();
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
                    Position PotMove = Position.Up().Left().Left();
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
                    Position PotMove = Position.Up().Right().Right();
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
                    Position PotMove = Position.Down().Left().Left();
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
                    Position PotMove = Position.Down().Right().Right();
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
                    Position PotMove = Position.Down().Down().Left();
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
                    Position PotMove = Position.Down().Down().Right();
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

            return ToReturn.ToArray();
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
                    if (Position.Rank() < 8 && Position.File() != 'H')
                    {
                        OnPosition = OnPosition.Up().Down();
                    }
                    else
                    {
                        StopCollecting = true;
                    }
                }
                else if (direction == 1)
                {
                    if (Position.Rank() > 1 && Position.File() != 'H')
                    {
                        OnPosition = OnPosition.Down().Right();
                    }
                    else
                    {
                        StopCollecting = true;
                    }
                }
                else if (direction == 2)
                {
                    if (Position.Rank() > 1 && Position.File() != 'A')
                    {
                        OnPosition = OnPosition.Down().Left();
                    }
                    else
                    {
                        StopCollecting = true;
                    }
                }
                else if (direction == 3)
                {
                    if (Position.Rank() < 8 && Position.File() != 'A')
                    {
                        OnPosition = OnPosition.Up().Left();
                    }
                    else
                    {
                        StopCollecting = true;
                    }
                }
                else if (direction == 4)
                {
                    if (Position.Rank() < 8)
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
                    if (Position.File() != 'H')
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
                    if (Position.Rank() > 1)
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
                    if (Position.File() != 'A')
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
    }
}