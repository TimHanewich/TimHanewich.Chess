using System;
using System.Collections.Generic;
using TimHanewich.Chess;

namespace TimHanewich.Chess.Experimental
{
    public struct Piece
    {
        public bool IsWhite {get; set;}
        public PieceType Type {get; set;}

        public Piece(bool is_white, PieceType type)
        {
            IsWhite = is_white;
            Type = type;
        }
    
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

        public static Position[] AvailableMoves(GameState gs, Piece p, Position from_position)
        {
            List<Position> ToReturn = new List<Position>();

            if (p.Type == PieceType.Pawn)
            {
                if (p.IsWhite)
                {
                    //Up 1?
                    if (gs.PositionIsOccupied(from_position.Up()) == false)
                    {
                        ToReturn.Add(from_position.Up());
                    }

                    //Up 2?
                    if (from_position.Rank() == 2)
                    {
                        if (gs.PositionIsOccupied(from_position.Up().Up()) == false)
                        {
                            ToReturn.Add(from_position.Up().Up());
                        }
                    }

                    //Capture left?
                    if (from_position.File() != 'A')
                    {
                        Piece? PotCap = gs.FindOccupyingPiece(from_position.UpLeft());
                        if (PotCap.HasValue)
                        {
                            if (PotCap.Value.IsWhite == false)
                            {
                                ToReturn.Add(from_position.UpLeft());
                            }
                        }
                    }

                    //Capture right?
                    if (from_position.File() != 'H')
                    {
                        Piece? PotCap = gs.FindOccupyingPiece(from_position.UpRight());
                        if (PotCap.HasValue == true)
                        {
                            if (PotCap.Value.IsWhite == false)
                            {
                                ToReturn.Add(from_position.UpRight());
                            }
                        }
                    }
                }
                else
                {
                    //Down 1?
                    if (gs.PositionIsOccupied(from_position.Down()) == false)
                    {
                        ToReturn.Add(from_position.Down());
                    }

                    //Down 2?
                    if (from_position.Rank() == 7)
                    {
                        if (gs.PositionIsOccupied(from_position.Down().Down()) == false)
                        {
                            ToReturn.Add(from_position.Down().Down());
                        }
                    }

                    //Capture left?
                    if (from_position.File() != 'A')
                    {
                        Piece? PotCap = gs.FindOccupyingPiece(from_position.DownLeft());
                        if (PotCap.HasValue)
                        {
                            if (PotCap.Value.IsWhite)
                            {
                                ToReturn.Add(from_position.DownLeft());
                            }
                        }
                    }

                    //Capture right?
                    if (from_position.File() != 'H')
                    {
                        Piece? PotCap = gs.FindOccupyingPiece(from_position.DownRight());
                        if (PotCap.HasValue)
                        {
                            if (PotCap.Value.IsWhite)
                            {
                                ToReturn.Add(from_position.DownRight());
                            }
                        }
                    }
                }
            }
            else if (p.Type == PieceType.Knight)
            {
                //Up 2, left 1
                if (from_position.Rank() < 7 && from_position.File() != 'A')
                {
                    Position PotMove = from_position.Up().UpLeft();
                    Piece? Occ = gs.FindOccupyingPiece(PotMove);
                    if (Occ.HasValue == false) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Value.IsWhite != p.IsWhite) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Up 2, right 1
                if (from_position.Rank() < 7 && from_position.File() != 'H')
                {
                    Position PotMove = from_position.Up().UpRight();
                    Piece? Occ = gs.FindOccupyingPiece(PotMove);
                    if (Occ.HasValue == false) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Value.IsWhite != p.IsWhite) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Up 1, left 2
                if (from_position.Rank() < 8 && from_position.File() != 'A' && from_position.File() != 'B')
                {
                    Position PotMove = from_position.UpLeft().Left();
                    Piece? Occ = gs.FindOccupyingPiece(PotMove);
                    if (Occ.HasValue == false) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Value.IsWhite != p.IsWhite) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Up 1, right 2
                if (from_position.Rank() < 8 && from_position.File() != 'G' && from_position.File() != 'H')
                {
                    Position PotMove = from_position.UpRight().Right();
                    Piece? Occ = gs.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Value.IsWhite != p.IsWhite) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Down 1, left 2
                if (from_position.Rank() > 1 && from_position.File() != 'A' && from_position.File() != 'B')
                {
                    Position PotMove = from_position.DownLeft().Left();
                    Piece? Occ = gs.FindOccupyingPiece(PotMove);
                    if (Occ.HasValue == false) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Value.IsWhite != p.IsWhite) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Down 1, right 2
                if (from_position.Rank() > 1 && from_position.File() != 'G' && from_position.File() != 'H')
                {
                    Position PotMove = from_position.DownRight().Right();
                    Piece? Occ = gs.FindOccupyingPiece(PotMove);
                    if (Occ == null) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Value.IsWhite != p.IsWhite) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Down 2, left 1
                if (from_position.Rank() > 2 && from_position.File() != 'A')
                {
                    Position PotMove = from_position.Down().DownLeft();
                    Piece? Occ = gs.FindOccupyingPiece(PotMove);
                    if (Occ.HasValue == false) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Value.IsWhite != p.IsWhite) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }

                //Down 2, right 1
                if (from_position.Rank() > 2 && from_position.File() != 'H')
                {
                    Position PotMove = from_position.Down().DownRight();
                    Piece? Occ = gs.FindOccupyingPiece(PotMove);
                    if (Occ.HasValue == false) //If it is empty we can jump to it
                    {
                        ToReturn.Add(PotMove);
                    }
                    else if (Occ.Value.IsWhite != p.IsWhite) //If it is an opposing color, we can take
                    {
                        ToReturn.Add(PotMove);
                    }
                }
            }
            else if (p.Type == PieceType.Bishop)
            {
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 0));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 1));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 2));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 3));
            }
            else if (p.Type == PieceType.Rook)
            {
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 4));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 5));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 6));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 7));
            }
            else if (p.Type == PieceType.Queen)
            {
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 0));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 1));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 2));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 3));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 4));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 5));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 6));
                ToReturn.AddRange(PotentialLinearMoves(gs, p.IsWhite, from_position, 7));
            }
            else if (p.Type == PieceType.King)
            {
                //This algorithm is incomplete. Obviously the King cannot place himself in check. So need to evaluate each potential move here and ensure it is not putting the knight in check. if it isn't, add it as a potential move!

                //Up?
                if (from_position.Rank() < 8)
                {
                    Position PotMove = from_position.Up();
                    Piece? OccPiece = gs.FindOccupyingPiece(PotMove);
                    if (OccPiece == null)
                    {
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Value.IsWhite != p.IsWhite)
                        {
                            ToReturn.Add(PotMove);
                        }
                    }
                }

                //Up right?
                if (from_position.Rank() < 8 && from_position.File() != 'H')
                {
                    Position PotMove = from_position.UpRight();
                    Piece? OccPiece = gs.FindOccupyingPiece(PotMove);
                    if (OccPiece.HasValue == false)
                    {
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Value.IsWhite != p.IsWhite)
                        {
                            ToReturn.Add(PotMove);
                        }
                    }
                }

                //Right?
                if (from_position.File() != 'H')
                {
                    Position PotMove = from_position.Right();
                    Piece? OccPiece = gs.FindOccupyingPiece(PotMove);
                    if (OccPiece.HasValue == false)
                    {
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Value.IsWhite != p.IsWhite)
                        {
                            ToReturn.Add(PotMove);
                        }
                    }
                }

                //Down, right?
                if (from_position.Rank() > 1 && from_position.File() != 'H')
                {
                    Position PotMove = from_position.DownRight();
                    Piece? OccPiece = gs.FindOccupyingPiece(PotMove);
                    if (OccPiece.HasValue == false)
                    {
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Value.IsWhite != p.IsWhite)
                        {
                            ToReturn.Add(PotMove);
                        }
                    }
                }

                //Down?
                if (from_position.Rank() > 1)
                {
                    Position PotMove = from_position.Down();
                    Piece? OccPiece = gs.FindOccupyingPiece(PotMove);
                    if (OccPiece.HasValue == false)
                    {
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Value.IsWhite != p.IsWhite)
                        {
                            ToReturn.Add(PotMove);
                        }
                    }
                }

                //Down, left?
                if (from_position.Rank() > 1 && from_position.File() != 'A')
                {
                    Position PotMove = from_position.DownLeft();
                    Piece? OccPiece = gs.FindOccupyingPiece(PotMove);
                    if (OccPiece.HasValue == false)
                    {
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Value.IsWhite != p.IsWhite)
                        {
                            ToReturn.Add(PotMove);
                        }
                    }
                }

                //Left?
                if (from_position.File() != 'A')
                {
                    Position PotMove = from_position.Left();
                    Piece? OccPiece = gs.FindOccupyingPiece(PotMove);
                    if (OccPiece.HasValue == false)
                    {
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Value.IsWhite != p.IsWhite)
                        {
                            ToReturn.Add(PotMove);
                        }
                    }
                }

                //Up, left?
                if (from_position.Rank() < 8 && from_position.File() != 'A')
                {
                    Position PotMove = from_position.UpLeft();
                    Piece? OccPiece = gs.FindOccupyingPiece(PotMove);
                    if (OccPiece.HasValue == false)
                    {
                        ToReturn.Add(PotMove);
                    }
                    else
                    {
                        if (OccPiece.Value.IsWhite != p.IsWhite)
                        {
                            ToReturn.Add(PotMove);
                        }
                    }
                }

            }

            // //Filter out any moves that would be illegal
            // if (EnsureLegality)
            // {
            //     return FilterOutIllegalMoves(board, ToReturn.ToArray());
            // }
            // else
            // {
            //     return ToReturn.ToArray();
            // }

            return ToReturn.ToArray();

        }



        //TOOLKIT


        //For example, filter out moves that put the king in check
        // private Position[] FilterOutIllegalMoves(GameState board, Position origin, Position[] moves_to)
        // {
        //     List<Position> ToReturn = new List<Position>();
        //     foreach (Position pos in moves_to)
        //     {
        //         bool IsLegal = MoveIsLegal(board, origin, pos);
        //         if (IsLegal)
        //         {
        //             ToReturn.Add(pos);
        //         }
        //     }
        //     return ToReturn.ToArray();
        // }

        // private bool MoveIsLegal(GameState board, Position origin, Position destination)
        // {
        //     BoardPosition copy = board.Copy();
        //     Move m = new Move();
        //     m.FromPosition = origin;
        //     m.ToPosition = destination;
        //     copy.ExecuteMove(m);

        //     //Executing the move above flips the color.
        //     //To test if the previous color that made the move was put in check, we must flip the color BACK to what it was before executing this
        //     //This is because the "IsCheck" method checkes if the color to move is in check (being threatened)
        //     if (copy.ToMove == Color.White)
        //     {
        //         copy.ToMove = Color.Black;
        //     }
        //     else if (copy.ToMove == Color.Black)
        //     {
        //         copy.ToMove = Color.White;
        //     }
        
        //     if (copy.IsCheck())
        //     {
        //         return false; //If this move put that color in check, say the move is not legal
        //     }
        //     else
        //     {
        //         return true; //if it isn't in check, this move is legal, so return true;
        //     }

        // }








        //For a bishop, rook or queen to use. Direction:
        //0 = up, right
        //1 = down, right
        //2 = down, left
        //3 = up, left
        //4 = up
        //5 = right
        //6 = down
        //7 = left
        private static Position[] PotentialLinearMoves(GameState board, bool is_white, Position from, int direction)
        {
            List<Position> ToReturn = new List<Position>();
            bool StopCollecting = false;
            Position OnPosition = from; //Starting position
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
                    Piece? OccupyingPiece = board.FindOccupyingPiece(OnPosition);
                    if (OccupyingPiece.HasValue == false) //if the position is empty, add it and move on
                    {
                        ToReturn.Add(OnPosition);
                    }
                    else
                    {
                        if (OccupyingPiece.Value.IsWhite != is_white) //It is occupied by an opposing piece. So we can capture it. Add it
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