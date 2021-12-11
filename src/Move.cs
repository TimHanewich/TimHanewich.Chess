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
    }
}