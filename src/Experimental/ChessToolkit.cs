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
            if (p.IsWhite)
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
    
        public static byte GetPosition(this Byte[] structure, Position pos)
        {
            if (structure.Length != 64)
            {
                throw new Exception("Unable to get position from board. Board was of length " + structure.Length.ToString() + ", not 64");
            }
            switch (pos)
            {
                case Position.A1:
                    return structure[0];
                case Position.A2:
                    return structure[1];
                case Position.A3:
                    return structure[2];
                case Position.A4:
                    return structure[3];
                case Position.A5:
                    return structure[4];
                case Position.A6:
                    return structure[5];
                case Position.A7:
                    return structure[6];
                case Position.A8:
                    return structure[7];
                case Position.B1:
                    return structure[8];
                case Position.B2:
                    return structure[9];
                case Position.B3:
                    return structure[10];
                case Position.B4:
                    return structure[11];
                case Position.B5:
                    return structure[12];
                case Position.B6:
                    return structure[13];
                case Position.B7:
                    return structure[14];
                case Position.B8:
                    return structure[15];
                case Position.C1:
                    return structure[16];
                case Position.C2:
                    return structure[17];
                case Position.C3:
                    return structure[18];
                case Position.C4:
                    return structure[19];
                case Position.C5:
                    return structure[20];
                case Position.C6:
                    return structure[21];
                case Position.C7:
                    return structure[22];
                case Position.C8:
                    return structure[23];
                case Position.D1:
                    return structure[24];
                case Position.D2:
                    return structure[25];
                case Position.D3:
                    return structure[26];
                case Position.D4:
                    return structure[27];
                case Position.D5:
                    return structure[28];
                case Position.D6:
                    return structure[29];
                case Position.D7:
                    return structure[30];
                case Position.D8:
                    return structure[31];
                case Position.E1:
                    return structure[32];
                case Position.E2:
                    return structure[33];
                case Position.E3:
                    return structure[34];
                case Position.E4:
                    return structure[35];
                case Position.E5:
                    return structure[36];
                case Position.E6:
                    return structure[37];
                case Position.E7:
                    return structure[38];
                case Position.E8:
                    return structure[39];
                case Position.F1:
                    return structure[40];
                case Position.F2:
                    return structure[41];
                case Position.F3:
                    return structure[42];
                case Position.F4:
                    return structure[43];
                case Position.F5:
                    return structure[44];
                case Position.F6:
                    return structure[45];
                case Position.F7:
                    return structure[46];
                case Position.F8:
                    return structure[47];
                case Position.G1:
                    return structure[48];
                case Position.G2:
                    return structure[49];
                case Position.G3:
                    return structure[50];
                case Position.G4:
                    return structure[51];
                case Position.G5:
                    return structure[52];
                case Position.G6:
                    return structure[53];
                case Position.G7:
                    return structure[54];
                case Position.G8:
                    return structure[55];
                case Position.H1:
                    return structure[56];
                case Position.H2:
                    return structure[57];
                case Position.H3:
                    return structure[58];
                case Position.H4:
                    return structure[59];
                case Position.H5:
                    return structure[60];
                case Position.H6:
                    return structure[61];
                case Position.H7:
                    return structure[62];
                case Position.H8:
                    return structure[63];
                default:
                    throw new Exception("Position '" + pos.ToString() + "' not recognized");
            }
        }
    }
}   