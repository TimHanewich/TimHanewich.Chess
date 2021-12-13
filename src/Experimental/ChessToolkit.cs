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

        #region "Movemment"

        //These return the index of the position in the byte array relative to the starting position (in byte format)

        public static byte Up(this byte array_position)
        {
            switch (array_position)
            {
                case 0:
                    return 1;   
                case 1:
                    return 2;  
                case 2:
                    return 3;  
                case 3:
                    return 4;  
                case 4:
                    return 5;  
                case 5:
                    return 6;  
                case 6:
                    return 7;  
                case 8:
                    return 9;
                case 9:
                    return 10; 
                case 10:
                    return 11;  
                case 11:
                    return 12;  
                case 12:
                    return 13;  
                case 13:
                    return 14;  
                case 14:
                    return 15; 
                case 16: //File C
                    return 17; 
                case 17:
                    return 18;
                case 18:
                    return 19; 
                case 19:
                    return 20; 
                case 20:
                    return 21; 
                case 21:
                    return 22; 
                case 22:
                    return 23;  
                case 24: //File D
                    return 25; 
                case 25:
                    return 26;
                case 26:
                    return 27; 
                case 27:
                    return 28; 
                case 28:
                    return 29; 
                case 29:
                    return 30; 
                case 30:
                    return 31;  
                case 32: //File E
                    return 33; 
                case 33:
                    return 34;
                case 34:
                    return 35; 
                case 35:
                    return 36; 
                case 36:
                    return 37; 
                case 37:
                    return 38; 
                case 38:
                    return 39;  
                case 40: //File F
                    return 41; 
                case 41:
                    return 42;
                case 42:
                    return 43; 
                case 43:
                    return 44; 
                case 44:
                    return 45; 
                case 45:
                    return 46; 
                case 46:
                    return 47;   
                case 48: //File G
                    return 49; 
                case 49:
                    return 50;
                case 50:
                    return 51; 
                case 51:
                    return 52; 
                case 52:
                    return 53; 
                case 53:
                    return 54; 
                case 54:
                    return 55; 
                case 56: //File H
                    return 57; 
                case 57:
                    return 58;
                case 58:
                    return 59; 
                case 59:
                    return 60; 
                case 60:
                    return 61; 
                case 61:
                    return 62; 
                case 62:
                    return 63; 
                default:
                    throw new Exception("It is impossible to move up from array position " + array_position.ToString());                               
                
            }
        }

        #endregion


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
    
        public static byte GetPositionByte(this Byte[] structure, Position pos)
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
    
        public static Piece? GetPositionPiece(this Byte[] structure, Position pos)
        {
            byte b = GetPositionByte(structure, pos);
            if (b == 0)
            {
                return null;
            }
            else if (b == 1)
            {
                return new Piece(true, PieceType.King);
            }
            else if (b == 2)
            {
                return new Piece(true, PieceType.Queen);
            }
            else if (b == 3)
            {
                return new Piece(true, PieceType.Rook);
            }
            else if (b == 4)
            {
                return new Piece(true, PieceType.Bishop);
            }
            else if (b == 5)
            {
                return new Piece(true, PieceType.Knight);
            }
            else if (b == 6)
            {
                return new Piece(true, PieceType.Pawn);
            }
            else if (b == 7)
            {
                return new Piece(false, PieceType.King);
            }
            else if (b == 8)
            {
                return new Piece(false, PieceType.Queen);
            }
            else if (b == 9)
            {
                return new Piece(false, PieceType.Rook);
            }
            else if (b == 10)
            {
                return new Piece(false, PieceType.Bishop);
            }
            else if (b == 11)
            {
                return new Piece(false, PieceType.Knight);
            }
            else if (b == 12)
            {
                return new Piece(false, PieceType.Pawn);
            }
            else
            {
                throw new Exception("Unable to convert byte '" + b.ToString() + "' into a piece.");
            }
        }
    }
}   