using System;

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
    }
}