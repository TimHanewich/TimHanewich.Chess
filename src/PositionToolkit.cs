using System;
using System.Collections.Generic;

namespace TimHanewich.Chess
{
    public static class PositionToolkit
    {
        public static int Rank(this Position position)
        {
            switch (position)
            {
                case Position.A1:
                    return 1;
                case Position.A2:
                    return 2;
                case Position.A3:
                    return 3;
                case Position.A4:
                    return 4;
                case Position.A5:
                    return 5;
                case Position.A6:
                    return 6;
                case Position.A7:
                    return 7;
                case Position.A8:
                    return 8;
                case Position.B1:
                    return 1;
                case Position.B2:
                    return 2;
                case Position.B3:
                    return 3;
                case Position.B4:
                    return 4;
                case Position.B5:
                    return 5;
                case Position.B6:
                    return 6;
                case Position.B7:
                    return 7;
                case Position.B8:
                    return 8;
                case Position.C1:
                    return 1;
                case Position.C2:
                    return 2;
                case Position.C3:
                    return 3;
                case Position.C4:
                    return 4;
                case Position.C5:
                    return 5;
                case Position.C6:
                    return 6;
                case Position.C7:
                    return 7;
                case Position.C8:
                    return 8;
                case Position.D1:
                    return 1;
                case Position.D2:
                    return 2;
                case Position.D3:
                    return 3;
                case Position.D4:
                    return 4;
                case Position.D5:
                    return 5;
                case Position.D6:
                    return 6;
                case Position.D7:
                    return 7;
                case Position.D8:
                    return 8;
                case Position.E1:
                    return 1;
                case Position.E2:
                    return 2;
                case Position.E3:
                    return 3;
                case Position.E4:
                    return 4;
                case Position.E5:
                    return 5;
                case Position.E6:
                    return 6;
                case Position.E7:
                    return 7;
                case Position.E8:
                    return 8;
                case Position.F1:
                    return 1;
                case Position.F2:
                    return 2;
                case Position.F3:
                    return 3;
                case Position.F4:
                    return 4;
                case Position.F5:
                    return 5;
                case Position.F6:
                    return 6;
                case Position.F7:
                    return 7;
                case Position.F8:
                    return 8;
                case Position.G1:
                    return 1;
                case Position.G2:
                    return 2;
                case Position.G3:
                    return 3;
                case Position.G4:
                    return 4;
                case Position.G5:
                    return 5;
                case Position.G6:
                    return 6;
                case Position.G7:
                    return 7;
                case Position.G8:
                    return 8;
                case Position.H1:
                    return 1;
                case Position.H2:
                    return 2;
                case Position.H3:
                    return 3;
                case Position.H4:
                    return 4;
                case Position.H5:
                    return 5;
                case Position.H6:
                    return 6;
                case Position.H7:
                    return 7;
                case Position.H8:
                    return 8;
                default:
                    return 0;
                
            }
        }

        public static char File(this Position position)
        {
            switch (position)
            {
                case Position.A1:
                    return 'A';
                case Position.A2:
                    return 'A';
                case Position.A3:
                    return 'A';
                case Position.A4:
                    return 'A';
                case Position.A5:
                    return 'A';
                case Position.A6:
                    return 'A';
                case Position.A7:
                    return 'A';
                case Position.A8:
                    return 'A';
                case Position.B1:
                    return 'B';
                case Position.B2:
                    return 'B';
                case Position.B3:
                    return 'B';
                case Position.B4:
                    return 'B';
                case Position.B5:
                    return 'B';
                case Position.B6:
                    return 'B';
                case Position.B7:
                    return 'B';
                case Position.B8:
                    return 'B';
                case Position.C1:
                    return 'C';
                case Position.C2:
                    return 'C';
                case Position.C3:
                    return 'C';
                case Position.C4:
                    return 'C';
                case Position.C5:
                    return 'C';
                case Position.C6:
                    return 'C';
                case Position.C7:
                    return 'C';
                case Position.C8:
                    return 'C';
                case Position.D1:
                    return 'D';
                case Position.D2:
                    return 'D';
                case Position.D3:
                    return 'D';
                case Position.D4:
                    return 'D';
                case Position.D5:
                    return 'D';
                case Position.D6:
                    return 'D';
                case Position.D7:
                    return 'D';
                case Position.D8:
                    return 'D';
                case Position.E1:
                    return 'E';
                case Position.E2:
                    return 'E';
                case Position.E3:
                    return 'E';
                case Position.E4:
                    return 'E';
                case Position.E5:
                    return 'E';
                case Position.E6:
                    return 'E';
                case Position.E7:
                    return 'E';
                case Position.E8:
                    return 'E';
                case Position.F1:
                    return 'F';
                case Position.F2:
                    return 'F';
                case Position.F3:
                    return 'F';
                case Position.F4:
                    return 'F';
                case Position.F5:
                    return 'F';
                case Position.F6:
                    return 'F';
                case Position.F7:
                    return 'F';
                case Position.F8:
                    return 'F';
                case Position.G1:
                    return 'G';
                case Position.G2:
                    return 'G';
                case Position.G3:
                    return 'G';
                case Position.G4:
                    return 'G';
                case Position.G5:
                    return 'G';
                case Position.G6:
                    return 'G';
                case Position.G7:
                    return 'G';
                case Position.G8:
                    return 'G';
                case Position.H1:
                    return 'H';
                case Position.H2:
                    return 'H';
                case Position.H3:
                    return 'H';
                case Position.H4:
                    return 'H';
                case Position.H5:
                    return 'H';
                case Position.H6:
                    return 'H';
                case Position.H7:
                    return 'H';
                case Position.H8:
                    return 'H';
                default:
                    return 'X';
            }
        }

        public static Position Parse(string position)
        {
            foreach (Position p in Enum.GetValues(typeof(Position)))
            {
                if (p.ToString().ToUpper() == position.ToUpper().Trim())
                {
                    return p;
                }
            }
            throw new Exception("Unable to find position for '" + position + "'");
        }

        public static Position Up(this Position position)
        {
            switch (position)
            {
                case Position.A7:
                    return Position.A8;
                case Position.B7:
                    return Position.B8;
                case Position.C7:
                    return Position.C8;
                case Position.D7:
                    return Position.D8;
                case Position.E7:
                    return Position.E8;
                case Position.F7:
                    return Position.F8;
                case Position.G7:
                    return Position.G8;
                case Position.H7:
                    return Position.H8;
                case Position.A6:
                    return Position.A7;
                case Position.B6:
                    return Position.B7;
                case Position.C6:
                    return Position.C7;
                case Position.D6:
                    return Position.D7;
                case Position.E6:
                    return Position.E7;
                case Position.F6:
                    return Position.F7;
                case Position.G6:
                    return Position.G7;
                case Position.H6:
                    return Position.H7;
                case Position.A5:
                    return Position.A6;
                case Position.B5:
                    return Position.B6;
                case Position.C5:
                    return Position.C6;
                case Position.D5:
                    return Position.D6;
                case Position.E5:
                    return Position.E6;
                case Position.F5:
                    return Position.F6;
                case Position.G5:
                    return Position.G6;
                case Position.H5:
                    return Position.H6;
                case Position.A4:
                    return Position.A5;
                case Position.B4:
                    return Position.B5;
                case Position.C4:
                    return Position.C5;
                case Position.D4:
                    return Position.D5;
                case Position.E4:
                    return Position.E5;
                case Position.F4:
                    return Position.F5;
                case Position.G4:
                    return Position.G5;
                case Position.H4:
                    return Position.H5;
                case Position.A3:
                    return Position.A4;
                case Position.B3:
                    return Position.B4;
                case Position.C3:
                    return Position.C4;
                case Position.D3:
                    return Position.D4;
                case Position.E3:
                    return Position.E4;
                case Position.F3:
                    return Position.F4;
                case Position.G3:
                    return Position.G4;
                case Position.H3:
                    return Position.H4;
                case Position.A2:
                    return Position.A3;
                case Position.B2:
                    return Position.B3;
                case Position.C2:
                    return Position.C3;
                case Position.D2:
                    return Position.D3;
                case Position.E2:
                    return Position.E3;
                case Position.F2:
                    return Position.F3;
                case Position.G2:
                    return Position.G3;
                case Position.H2:
                    return Position.H3;
                case Position.A1:
                    return Position.A2;
                case Position.B1:
                    return Position.B2;
                case Position.C1:
                    return Position.C2;
                case Position.D1:
                    return Position.D2;
                case Position.E1:
                    return Position.E2;
                case Position.F1:
                    return Position.F2;
                case Position.G1:
                    return Position.G2;
                case Position.H1:
                    return Position.H2;
                default:
                    throw new Exception("Maximum rank already achieved.");
            }
        }

        public static Position Down(this Position position)
        {
            switch (position)
            {
                case Position.A8:
                    return Position.A7;
                case Position.B8:
                    return Position.B7;
                case Position.C8:
                    return Position.C7;
                case Position.D8:
                    return Position.D7;
                case Position.E8:
                    return Position.E7;
                case Position.F8:
                    return Position.F7;
                case Position.G8:
                    return Position.G7;
                case Position.H8:
                    return Position.H7;
                case Position.A7:
                    return Position.A6;
                case Position.B7:
                    return Position.B6;
                case Position.C7:
                    return Position.C6;
                case Position.D7:
                    return Position.D6;
                case Position.E7:
                    return Position.E6;
                case Position.F7:
                    return Position.F6;
                case Position.G7:
                    return Position.G6;
                case Position.H7:
                    return Position.H6;
                case Position.A6:
                    return Position.A5;
                case Position.B6:
                    return Position.B5;
                case Position.C6:
                    return Position.C5;
                case Position.D6:
                    return Position.D5;
                case Position.E6:
                    return Position.E5;
                case Position.F6:
                    return Position.F5;
                case Position.G6:
                    return Position.G5;
                case Position.H6:
                    return Position.H5;
                case Position.A5:
                    return Position.A4;
                case Position.B5:
                    return Position.B4;
                case Position.C5:
                    return Position.C4;
                case Position.D5:
                    return Position.D4;
                case Position.E5:
                    return Position.E4;
                case Position.F5:
                    return Position.F4;
                case Position.G5:
                    return Position.G4;
                case Position.H5:
                    return Position.H4;
                case Position.A4:
                    return Position.A3;
                case Position.B4:
                    return Position.B3;
                case Position.C4:
                    return Position.C3;
                case Position.D4:
                    return Position.D3;
                case Position.E4:
                    return Position.E3;
                case Position.F4:
                    return Position.F3;
                case Position.G4:
                    return Position.G3;
                case Position.H4:
                    return Position.H3;
                case Position.A3:
                    return Position.A2;
                case Position.B3:
                    return Position.B2;
                case Position.C3:
                    return Position.C2;
                case Position.D3:
                    return Position.D2;
                case Position.E3:
                    return Position.E2;
                case Position.F3:
                    return Position.F2;
                case Position.G3:
                    return Position.G2;
                case Position.H3:
                    return Position.H2;
                case Position.A2:
                    return Position.A1;
                case Position.B2:
                    return Position.B1;
                case Position.C2:
                    return Position.C1;
                case Position.D2:
                    return Position.D1;
                case Position.E2:
                    return Position.E1;
                case Position.F2:
                    return Position.F1;
                case Position.G2:
                    return Position.G1;
                case Position.H2:
                    return Position.H1;
                default:
                    throw new Exception("Minumum rank already achieved.");
                    
            }
        }

        public static Position Right(this Position position)
        {
            switch (position)
            {
                case Position.A8:
                    return Position.B8;
                case Position.A7:
                    return Position.B7;
                case Position.A6:
                    return Position.B6;
                case Position.A5:
                    return Position.B5;
                case Position.A4:
                    return Position.B4;
                case Position.A3:
                    return Position.B3;
                case Position.A2:
                    return Position.B2;
                case Position.A1:
                    return Position.B1;
                case Position.B8:
                    return Position.C8;
                case Position.B7:
                    return Position.C7;
                case Position.B6:
                    return Position.C6;
                case Position.B5:
                    return Position.C5;
                case Position.B4:
                    return Position.C4;
                case Position.B3:
                    return Position.C3;
                case Position.B2:
                    return Position.C2;
                case Position.B1:
                    return Position.C1;
                case Position.C8:
                    return Position.D8;
                case Position.C7:
                    return Position.D7;
                case Position.C6:
                    return Position.D6;
                case Position.C5:
                    return Position.D5;
                case Position.C4:
                    return Position.D4;
                case Position.C3:
                    return Position.D3;
                case Position.C2:
                    return Position.D2;
                case Position.C1:
                    return Position.D1;
                case Position.D8:
                    return Position.E8;
                case Position.D7:
                    return Position.E7;
                case Position.D6:
                    return Position.E6;
                case Position.D5:
                    return Position.E5;
                case Position.D4:
                    return Position.E4;
                case Position.D3:
                    return Position.E3;
                case Position.D2:
                    return Position.E2;
                case Position.D1:
                    return Position.E1;
                case Position.E8:
                    return Position.F8;
                case Position.E7:
                    return Position.F7;
                case Position.E6:
                    return Position.F6;
                case Position.E5:
                    return Position.F5;
                case Position.E4:
                    return Position.F4;
                case Position.E3:
                    return Position.F3;
                case Position.E2:
                    return Position.F2;
                case Position.E1:
                    return Position.F1;
                case Position.F8:
                    return Position.G8;
                case Position.F7:
                    return Position.G7;
                case Position.F6:
                    return Position.G6;
                case Position.F5:
                    return Position.G5;
                case Position.F4:
                    return Position.G4;
                case Position.F3:
                    return Position.G3;
                case Position.F2:
                    return Position.G2;
                case Position.F1:
                    return Position.G1;
                case Position.G8:
                    return Position.H8;
                case Position.G7:
                    return Position.H7;
                case Position.G6:
                    return Position.H6;
                case Position.G5:
                    return Position.H5;
                case Position.G4:
                    return Position.H4;
                case Position.G3:
                    return Position.H3;
                case Position.G2:
                    return Position.H2;
                case Position.G1:
                    return Position.H1;
                default:
                    throw new Exception("Maximum rank achieved.");
            }
        }

        public static Position Left(this Position position)
        {
            char CurrentFile = position.File();
            if (CurrentFile == 'A')
            {
                throw new Exception("Minumum file already achieved.");
            }
            else
            {
                char NewFile = 'H';
                if (CurrentFile == 'H')
                {
                    NewFile = 'G';
                }
                else if (CurrentFile == 'G')
                {
                    NewFile = 'F';
                }
                else if (CurrentFile == 'F')
                {
                    NewFile = 'E';
                }
                else if (CurrentFile == 'E')
                {
                    NewFile = 'D';
                }
                else if (CurrentFile == 'D')
                {
                    NewFile = 'C';
                }
                else if (CurrentFile == 'C')
                {
                    NewFile = 'B';
                }
                else if (CurrentFile == 'B')
                {
                    NewFile = 'A';
                }
                
                Position ToReturn = PositionToolkit.Parse(NewFile.ToString() + position.Rank());
                return ToReturn;
            }
        }

        public static Position[] FenOrder()
        {
            List<Position> ToReturn = new List<Position>();
            ToReturn.Add(Position.A8);
            ToReturn.Add(Position.B8);
            ToReturn.Add(Position.C8);
            ToReturn.Add(Position.D8);
            ToReturn.Add(Position.E8);
            ToReturn.Add(Position.F8);
            ToReturn.Add(Position.G8);
            ToReturn.Add(Position.H8);
            ToReturn.Add(Position.A7);
            ToReturn.Add(Position.B7);
            ToReturn.Add(Position.C7);
            ToReturn.Add(Position.D7);
            ToReturn.Add(Position.E7);
            ToReturn.Add(Position.F7);
            ToReturn.Add(Position.G7);
            ToReturn.Add(Position.H7);
            ToReturn.Add(Position.A6);
            ToReturn.Add(Position.B6);
            ToReturn.Add(Position.C6);
            ToReturn.Add(Position.D6);
            ToReturn.Add(Position.E6);
            ToReturn.Add(Position.F6);
            ToReturn.Add(Position.G6);
            ToReturn.Add(Position.H6);
            ToReturn.Add(Position.A5);
            ToReturn.Add(Position.B5);
            ToReturn.Add(Position.C5);
            ToReturn.Add(Position.D5);
            ToReturn.Add(Position.E5);
            ToReturn.Add(Position.F5);
            ToReturn.Add(Position.G5);
            ToReturn.Add(Position.H5);
            ToReturn.Add(Position.A4);
            ToReturn.Add(Position.B4);
            ToReturn.Add(Position.C4);
            ToReturn.Add(Position.D4);
            ToReturn.Add(Position.E4);
            ToReturn.Add(Position.F4);
            ToReturn.Add(Position.G4);
            ToReturn.Add(Position.H4);
            ToReturn.Add(Position.A3);
            ToReturn.Add(Position.B3);
            ToReturn.Add(Position.C3);
            ToReturn.Add(Position.D3);
            ToReturn.Add(Position.E3);
            ToReturn.Add(Position.F3);
            ToReturn.Add(Position.G3);
            ToReturn.Add(Position.H3);
            ToReturn.Add(Position.A2);
            ToReturn.Add(Position.B2);
            ToReturn.Add(Position.C2);
            ToReturn.Add(Position.D2);
            ToReturn.Add(Position.E2);
            ToReturn.Add(Position.F2);
            ToReturn.Add(Position.G2);
            ToReturn.Add(Position.H2);
            ToReturn.Add(Position.A1);
            ToReturn.Add(Position.B1);
            ToReturn.Add(Position.C1);
            ToReturn.Add(Position.D1);
            ToReturn.Add(Position.E1);
            ToReturn.Add(Position.F1);
            ToReturn.Add(Position.G1);
            ToReturn.Add(Position.H1);
            return ToReturn.ToArray();
        }

    }
}