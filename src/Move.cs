using System;

namespace TimHanewich.Chess
{
    public class Move
    {
        public Position FromPosition {get; set;}
        public Position ToPosition {get; set;}

        public bool IsPawnPromotion(BoardPosition position)
        {
            Piece p = position.FindOccupyingPiece(FromPosition);
            if (p == null)
            {
                return false;
            }
            if (p.Type != PieceType.Pawn)
            {
                return false;
            }
            
            //Check
            if (p.Color == Color.White)
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
            Piece MovingPiece = position.FindOccupyingPiece(FromPosition);
            if (MovingPiece == null)
            {
                throw new Exception("Move Unable to convert move " + FromPosition.ToString() + " to " + ToPosition.ToString() + " to algebraic notation. Piece not found on from position square.");
            }

            //Get the beginning piece notation
            string PieceNotation = GetPieceNotation(MovingPiece.Type);
            

            //Is this a take? Is it a capture of an enemy piece?
            string CaptureNotation = "";
            Piece CapturingPiece = position.FindOccupyingPiece(ToPosition);
            if (CapturingPiece != null)
            {
                if (CapturingPiece.Color != MovingPiece.Color)
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
                    if (p.Color == MovingPiece.Color)
                    {
                        if (p.Type == MovingPiece.Type)
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
                                if (p.Position.Rank() == MovingPiece.Position.Rank())
                                {
                                    DisambiguatingNotation = MovingPiece.Position.File().ToString().ToLower();
                                }
                                else if (p.Position.File() == MovingPiece.Position.File())
                                {
                                    DisambiguatingNotation = MovingPiece.Position.Rank().ToString();
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
    }
}