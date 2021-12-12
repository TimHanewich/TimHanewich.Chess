using System;

namespace TimHanewich.Chess.Experimental
{
    public static class ChessToolkit
    {

        //Piece byte code
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


        public static byte ToByte(this Piece p)
        {
            if (p.Color == Color.White)
            {
                if (p.Type == PieceType.King)
                {
                    return 1;
                }
                else if (p.Type == PieceType.Queen)
                {
                    return 2;
                }
                else if (p.Type == PieceType.Rook)
                {
                    return 3;
                }
                else if (p.Type == PieceType.Bishop)
                {
                    return 4;
                }
                else if (p.Type == PieceType.Knight)
                {
                    return 5;
                }
                else if (p.Type == PieceType.Pawn)
                {
                    return 6;
                }
            }
            else
            {
                if (p.Type == PieceType.King)
                {
                    return 7;
                }
                else if (p.Type == PieceType.Queen)
                {
                    return 8;
                }
                else if (p.Type == PieceType.Rook)
                {
                    return 9;
                }
                else if (p.Type == PieceType.Bishop)
                {
                    return 10;
                }
                else if (p.Type == PieceType.Knight)
                {
                    return 11;
                }
                else if (p.Type == PieceType.Pawn)
                {
                    return 12;
                }
            }
            throw new Exception("Fatal error while converting piece to byte.");
        }
    }
}