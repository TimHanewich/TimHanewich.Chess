using System;

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


    }
}