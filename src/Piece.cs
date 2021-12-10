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

            return ToReturn.ToArray();
        }
    }
}