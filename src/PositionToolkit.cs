using System;
using System.Collections.Generic;

namespace TimHanewich.Chess
{
    public static class PositionToolkit
    {
        public static int Rank(this Position position)
        {
            string FullPos = position.ToString();
            int val = Convert.ToInt32(FullPos.Substring(1, 1));
            return val;
        }

        public static char File(this Position position)
        {
            string FullPos = position.ToString();
            return FullPos[0];
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
            if (position.Rank() == 8)
            {
                throw new Exception("Maximum rank already achieved.");
            }
            else
            {
                return PositionToolkit.Parse(position.File() + Convert.ToString(position.Rank() + 1));
            }
        }

        public static Position Down(this Position position)
        {
            if (position.Rank() == 1)
            {
                throw new Exception("Minumum rank already achieved.");
            }
            else
            {
                return PositionToolkit.Parse(position.File() + Convert.ToString(position.Rank() - 1));
            }
        }

        public static Position Right(this Position position)
        {
            char CurrentFile = position.File();
            if (CurrentFile == 'H')
            {
                throw new Exception("Maximum file already achieved.");
            }
            else
            {
                char NewFile = 'A';
                if (CurrentFile == 'A')
                {
                    NewFile = 'B';
                }
                else if (CurrentFile == 'B')
                {
                    NewFile = 'C';
                }
                else if (CurrentFile == 'C')
                {
                    NewFile = 'D';
                }
                else if (CurrentFile == 'D')
                {
                    NewFile = 'E';
                }
                else if (CurrentFile == 'E')
                {
                    NewFile = 'F';
                }
                else if (CurrentFile == 'F')
                {
                    NewFile = 'G';
                }
                else if (CurrentFile == 'G')
                {
                    NewFile = 'H';
                }
                
                Position ToReturn = PositionToolkit.Parse(NewFile.ToString() + position.Rank());
                return ToReturn;
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