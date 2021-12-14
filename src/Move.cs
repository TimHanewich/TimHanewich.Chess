using System;

namespace TimHanewich.Chess
{
    public class Move
    {
        public Position FromPosition {get; set;}
        public Position ToPosition {get; set;}

        public bool IsPawnPromotion(BoardPosition position)
        {
            Piece? p = position.FindOccupyingPiece(FromPosition);
            if (p == null)
            {
                return false;
            }
            if (p.Value.Type != PieceType.Pawn)
            {
                return false;
            }
            
            //Check
            if (p.Value.Color == Color.White)
            {
                if (FromPosition.Rank() == 7 && ToPosition.Rank() == 8)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (FromPosition.Rank() == 2 && ToPosition.Rank() == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    
        public string ToAlgebraicNotation(BoardPosition position, PieceType promote_pawn_to = PieceType.Queen)
        {
            Piece? MovingPiece = position.FindOccupyingPiece(FromPosition);
            if (MovingPiece.HasValue == false)
            {
                throw new Exception("Move Unable to convert move " + FromPosition.ToString() + " to " + ToPosition.ToString() + " to algebraic notation. Piece not found on from position square.");
            }

            //Get the beginning piece notation
            string PieceNotation = GetPieceNotation(MovingPiece.Value.Type);
            

            //Is this a take? Is it a capture of an enemy piece?
            string CaptureNotation = "";
            Piece? CapturingPiece = position.FindOccupyingPiece(ToPosition);
            if (CapturingPiece.HasValue)
            {
                if (CapturingPiece.Value.Color != MovingPiece.Value.Color)
                {
                    CaptureNotation = "x";
                }
            }

            //Disambiguating move?
            string DisambiguatingNotation = "";
            //Check if there are any other pieces of the same type that can also move to the destination square
            foreach (Piece p in position.Pieces)
            {
                if (p.Position != FromPosition)
                {
                    if (p.Color == MovingPiece.Value.Color)
                    {
                        if (p.Type == MovingPiece.Value.Type)
                        { 
                            //Can this piece also move to the destination?
                            bool ThisPieceCanMoveThereToo = false;
                            foreach (Position ppp in p.AvailableMoves(position, true))
                            {
                                if (ppp == ToPosition)
                                {
                                    ThisPieceCanMoveThereToo = true;
                                }
                            }
                            
                            //If it can move there too, disambiguate
                            if (ThisPieceCanMoveThereToo)
                            {
                                //What is the difference? Is it rank or is it file?
                                if (p.Position.Rank() == MovingPiece.Value.Position.Rank())
                                {
                                    DisambiguatingNotation = MovingPiece.Value.Position.File().ToString().ToLower();
                                }
                                else if (p.Position.File() == MovingPiece.Value.Position.File())
                                {
                                    DisambiguatingNotation = MovingPiece.Value.Position.Rank().ToString();
                                }
                            }   
                        }
                    }
                }
            }


            //Get the Position part
            string PositionNotation = ToPosition.ToString().ToLower();

            //Pawn promotion?
            string PromotionNotation = "";
            if (IsPawnPromotion(position))
            {
                PromotionNotation = "=" + GetPieceNotation(promote_pawn_to);
            }

            //Is it it a check or check mate?
            string CheckCheckMateNotation = "";
            BoardPosition ResultingPosition = position.Copy();
            ResultingPosition.ExecuteMove(this);
            if (ResultingPosition.IsCheckMate())
            {
                CheckCheckMateNotation = "#";
            }
            else if (ResultingPosition.IsCheck())
            {
                CheckCheckMateNotation = "+";
            }


            //String it all together and return
            string ToReturn = PieceNotation + DisambiguatingNotation + CaptureNotation + PositionNotation + PromotionNotation + CheckCheckMateNotation;
            return ToReturn;
        }
    
        private string GetPieceNotation(PieceType type)
        {
            string ToReturn = "";
            if (type == PieceType.Rook)
            {
                ToReturn = "R";
            }
            else if (type ==  PieceType.King)
            {
                ToReturn = "K";
            }
            else if (type == PieceType.Queen)
            {
                ToReturn = "Q";
            }
            else if (type == PieceType.Bishop)
            {
                ToReturn = "B";
            }
            else if (type == PieceType.Knight)
            {
                ToReturn = "N";
            }
            return ToReturn;
        }
    
        public Move()
        {

        }

        public Move(string algebraic_notation, BoardPosition position)
        {
            string disecting = algebraic_notation;

            //Strip out the x
            disecting = disecting.Replace("x", "");

            //Get the to position
            int DestinationRank = FindLastNumber(disecting).Value;
            int DestinationRankPosition = disecting.LastIndexOf(DestinationRank.ToString());
            string DestinationFile = disecting.Substring(DestinationRankPosition - 1, 1);
            ToPosition = PositionToolkit.Parse(DestinationFile.Trim().ToUpper() + DestinationRank.ToString());

            //What piece is this moving?
            PieceType Moving;
            string PieceNotation = algebraic_notation.Substring(0, 1);
            if (PieceNotation == "K")
            {
                Moving = PieceType.King;
            }
            else if (PieceNotation == "Q")
            {
                Moving = PieceType.Queen;
            }
            else if (PieceNotation == "R")
            {
                Moving = PieceType.Rook;
            }
            else if (PieceNotation == "B")
            {
                Moving = PieceType.Bishop;
            }
            else if (PieceNotation == "K")
            {
                Moving = PieceType.Knight;
            }
            else
            {
                Moving = PieceType.Pawn;
            }

            //Get the from position
            foreach (Piece p in position.Pieces)
            {
                if (p.Color == position.ToMove)
                {
                    if (p.Type == Moving)
                    {
                        Position[] moves = p.AvailableMoves(position, true);
                        foreach (Position pos in moves)
                        {
                            if (pos == ToPosition)
                            {
                                FromPosition = p.Position;
                            }
                        }
                    }
                }
            }
        }

        private int? FindLastNumber(string inside)
        {
            int? ToReturn = null;
            for (int t = 1; t <= 8; t++)
            {
                int val = inside.LastIndexOf(t.ToString());
                if (val != -1)
                {
                    if (ToReturn == null)
                    {
                        ToReturn = val;
                    }
                    else
                    {
                        if (val > ToReturn.Value)
                        {
                            ToReturn = val;
                        }
                    }
                }
            }
            return ToReturn;
        }
    }
}