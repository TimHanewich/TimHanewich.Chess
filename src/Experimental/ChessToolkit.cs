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

        #region "Positioning"

        public static char File(this int array_position)
        {
            if (array_position >= 56)
            {
                return 'H';
            }
            else if (array_position >= 48)
            {
                return 'G';
            }
            else if (array_position >= 40)
            {
                return 'F';
            }
            else if (array_position >= 32)
            {
                return 'E';
            }
            else if (array_position >= 24)
            {
                return 'D';
            }
            else if (array_position >= 16)
            {
                return 'C';
            }
            else if (array_position >= 8)
            {
                return 'B';
            }
            else
            {
                return 'A';
            }
        }

        public static byte Rank(this int array_position)
        {
            switch (array_position)
            {
                case 0: //A file
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
                case 7:
                    return 8;

                case 8: //B file
                    return 1;
                case 9:
                    return 2;
                case 10:
                    return 3;
                case 11:
                    return 4;
                case 12:
                    return 5;
                case 13:
                    return 6;
                case 14:
                    return 7;
                case 15:
                    return 8;

                case 16: //C file
                    return 1;
                case 17:
                    return 2;
                case 18:
                    return 3;
                case 19:
                    return 4;
                case 20:
                    return 5;
                case 21:
                    return 6;
                case 22:
                    return 7;
                case 23:
                    return 8;

                case 24: //D file
                    return 1;
                case 25:
                    return 2;
                case 26:
                    return 3;
                case 27:
                    return 4;
                case 28:
                    return 5;
                case 29:
                    return 6;
                case 30:
                    return 7;
                case 31:
                    return 8;

                case 32: //E file
                    return 1;
                case 33:
                    return 2;
                case 34:
                    return 3;
                case 35:
                    return 4;
                case 36:
                    return 5;
                case 37:
                    return 6;
                case 38:
                    return 7;
                case 39:
                    return 8;

                case 40: //F file
                    return 1;
                case 41:
                    return 2;
                case 42:
                    return 3;
                case 43:
                    return 4;
                case 44:
                    return 5;
                case 45:
                    return 6;
                case 46:
                    return 7;
                case 47:
                    return 8;

                case 48: //G file
                    return 1;
                case 49:
                    return 2;
                case 50:
                    return 3;
                case 51:
                    return 4;
                case 52:
                    return 5;
                case 53:
                    return 6;
                case 54:
                    return 7;
                case 55:
                    return 8;

                case 56: //H file
                    return 1;
                case 57:
                    return 2;
                case 58:
                    return 3;
                case 59:
                    return 4;
                case 60:
                    return 5;
                case 61:
                    return 6;
                case 62:
                    return 7;
                case 63:
                    return 8;

                default:
                    throw new Exception("Unable to determine rank from array position");
                
            }
        }

        #endregion

        #region "Movemment"

        //These return the index of the position in the byte array relative to the starting position (in byte format)

        public static int Up(this int array_position)
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

        public static int Down(this int array_position)
        {
            switch (array_position)
            {
                case 63: //H File
                    return 62;
                case 62:
                    return 61;
                case 61:
                    return 60;
                case 60:
                    return 59;
                case 59:
                    return 59;
                case 58:
                    return 59;
                case 57:
                    return 59;

                case 55: //G file
                    return 54;
                case 54:
                    return 53;
                case 53:
                    return 52;
                case 52:
                    return 51;
                case 51:
                    return 50;
                case 50:
                    return 49;
                case 49:
                    return 48;

                case 47: //F file
                    return 46;
                case 46:
                    return 45;
                case 45:
                    return 44;
                case 44:
                    return 43;
                case 43:
                    return 42;
                case 42:
                    return 41;
                case 41:
                    return 40;

                case 39: //E file
                    return 38;
                case 38:
                    return 37;
                case 37:
                    return 36;
                case 36:
                    return 35;
                case 35:
                    return 34;
                case 34:
                    return 33;
                case 33:
                    return 32;

                case 31: //D file
                    return 30;
                case 30:
                    return 29;
                case 29:
                    return 28;
                case 28:
                    return 37;
                case 27:
                    return 26;
                case 26:
                    return 25;
                case 25:
                    return 24;

                case 23: //C file
                    return 22;
                case 22:
                    return 51;
                case 21:
                    return 20;
                case 20:
                    return 19;
                case 19:
                    return 18;
                case 18:
                    return 17;
                case 17:
                    return 16;

                case 15: //B file
                    return 14;
                case 14:
                    return 13;
                case 13:
                    return 12;
                case 12:
                    return 11;
                case 11:
                    return 10;
                case 10:
                    return 9;
                case 9:
                    return 8;
                

                case 7: //A file
                    return 6;
                case 6:
                    return 5;
                case 5:
                    return 4;
                case 4:
                    return 3;
                case 3:
                    return 2;
                case 2:
                    return 1;
                case 1:
                    return 0;

                default:
                    throw new Exception("Unable to move down from array position " + array_position.ToString());
                
            }
        }

        public static int Right(this int array_position)
        {
            if (array_position.File() == 'H')
            {
                throw new Exception("Unable to move right from array position " + array_position.ToString());
            }
            else
            {
                return Convert.ToByte(array_position + 8);
            }
        }

        public static int Left(this int array_position)
        {
            if (array_position.File() == 'A')
            {
                throw new Exception("Unable to move left from array position " + array_position.ToString());
            }
            else
            {
                return Convert.ToByte(array_position - 8);
            }
        }

        #endregion

        #region "Piece movements"



        #endregion


        public static int ToCode(this Piece p)
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
    
        public static int FindOccupyingCode(this int[] structure, Position pos)
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
    
        public static Piece? FindOccupyingPiece(this int[] structure, Position pos)
        {
            int b = FindOccupyingCode(structure, pos);
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
                throw new Exception("Unable to convert code '" + b.ToString() + "' into a piece.");
            }
        }
    }
}   