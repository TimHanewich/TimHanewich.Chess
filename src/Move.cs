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
    
        public string ToAlgebraicNotation(BoardPosition position)
        {
            Piece MovingPiece = position.FindOccupyingPiece(FromPosition);
            if (MovingPiece == null)
            {
                throw new Exception("Unable to convert move to algebraic notation. Piece not found on from position square.");
            }

            //Get the beginning piece notation
            string PieceNotation = "";
            if (MovingPiece.Type == PieceType.Rook)
            {
                PieceNotation = "R";
            }
            else if (MovingPiece.Type ==  PieceType.King)
            {
                PieceNotation = "K";
            }
            else if (MovingPiece.Type == PieceType.Queen)
            {
                PieceNotation = "Q";
            }
            else if (MovingPiece.Type == PieceType.Bishop)
            {
                PieceNotation = "B";
            }
            else if (MovingPiece.Type == PieceType.Knight)
            {
                PieceNotation = "N";
            }

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


            //Get the Position part
            string PositionNotation = ToPosition.ToString().ToLower();

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
            string ToReturn = PieceNotation + CaptureNotation + DisambiguatingNotation + PositionNotation + CheckCheckMateNotation;
            return ToReturn;
        }
    }
}