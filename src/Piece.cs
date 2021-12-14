using System;
using System.Collections.Generic;

namespace TimHanewich.Chess
{
    public struct Piece
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
                        Piece? PotCap = board.FindOccupyingPiece(Position.UpLeft());
                        if (PotCap.HasValue)
                        {
                            if (PotCap.Value.Color == Color.Black)
                            {
                                ToReturn.Add(PotCap.Value.Position);
                            }
                        }
                    }

                    //Capture right?
                    if (Position.File() != 'H')
                    {
                        Piece? PotCap = board.FindOccupyingPiece(Position.UpRight());
                        if (PotCap.HasValue)
                        {
                            if (PotCap.Value.Color == Color.Black)
                            {
                                ToReturn.Add(PotCap.Value.Position);
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
                        Piece? PotCap = board.FindOccupyingPiece(Position.DownLeft());
                        if (PotCap.HasValue)
                        {
                            if (PotCap.Value.Color == Color.White)
                            {
                                ToReturn.Add(PotCap.Value.Position);
                            }
                        }
                    }

                    //Capture right?
                    if (Position.File() != 'H')
                    {
                        Piece? PotCap = board.FindOccupyingPiece(Position.DownRight());
                        if (PotCap.HasValue)
                        {
                            if (PotCap.Value.Color == Color.White)
                            {
                                ToReturn.Add(PotCap.Value.Position);
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
                    AddPositionIfMoveable(ref ToReturn, board, Position.Up().UpLeft());
                }

                //Up 2, right 1
                if (Position.Rank() < 7 && Position.File() != 'H')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.Up().UpRight());
                }

                //Up 1, left 2
                if (Position.Rank() < 8 && Position.File() != 'A' && Position.File() != 'B')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.UpLeft().Left());
                }

                //Up 1, right 2
                if (Position.Rank() < 8 && Position.File() != 'G' && Position.File() != 'H')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.Up().Right().Right());
                }

                //Down 1, left 2
                if (Position.Rank() > 1 && Position.File() != 'A' && Position.File() != 'B')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.DownLeft().Left());
                }

                //Down 1, right 2
                if (Position.Rank() > 1 && Position.File() != 'G' && Position.File() != 'H')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.DownRight().Right());
                }

                //Down 2, left 1
                if (Position.Rank() > 2 && Position.File() != 'A')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.Down().DownLeft());
                }

                //Down 2, right 1
                if (Position.Rank() > 2 && Position.File() != 'H')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.Down().DownRight());
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
                    AddPositionIfMoveable(ref ToReturn, board, Position.Up());                    
                }

                //Up right?
                if (Position.Rank() < 8 && Position.File() != 'H')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.UpRight()); 
                }

                //Right?
                if (Position.File() != 'H')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.Right()); 
                }

                //Down, right?
                if (Position.Rank() > 1 && Position.File() != 'H')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.DownRight()); 
                }

                //Down?
                if (Position.Rank() > 1)
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.Down()); 
                }

                //Down, left?
                if (Position.Rank() > 1 && Position.File() != 'A')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.DownLeft()); 
                }

                //Left?
                if (Position.File() != 'A')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.Left()); 
                }

                //Up, left?
                if (Position.Rank() < 8 && Position.File() != 'A')
                {
                    AddPositionIfMoveable(ref ToReturn, board, Position.UpLeft()); 
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

        #region "property setting"

        public void SetPosition(Position pos)
        {
            Position = pos;
        }

        public void SetPieceType(PieceType pt)
        {
            Type = pt;
        }

        #endregion

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
                    Piece? OccupyingPiece = board.FindOccupyingPiece(OnPosition);
                    if (OccupyingPiece == null) //if the position is empty, add it and move on
                    {
                        ToReturn.Add(OnPosition);
                    }
                    else
                    {
                        if (OccupyingPiece.Value.Color != Color) //It is occupied by an opposing piece. So we can capture it. Add it
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
    
        private void AddPositionIfMoveable(ref List<Position> ToAddTo, BoardPosition board, Position pos)
        {
            Piece? BeingOccupied = board.FindOccupyingPiece(pos);
            if (BeingOccupied.HasValue == false) //it is empty. So it is moveable. Add it!
            {
                ToAddTo.Add(pos);
            }
            else
            {
                if (BeingOccupied.Value.Color != this.Color)
                {
                    ToAddTo.Add(pos); //Add it becuase it is not our color. We can capture it.
                }
            }
        }
    
    
    }
}