using System;
using System.IO;
using System.Collections.Generic;

namespace TimHanewich.Chess.PGN
{
    public class MassivePgnFileSplitter
    {
        private StreamReader SR;
        private string Buff;

        public MassivePgnFileSplitter(Stream s)
        {
            SR = new StreamReader(s);
            Buff = SR.ReadLine();
        }

        public string NextGame()
        {
            if (Buff == null)
            {
                return null;
            }

            string NextGame = "";
            NextGame = Buff + Environment.NewLine;
            bool KillNow = false;
            do
            {
                string next_line = SR.ReadLine();

                if (next_line == null)
                {
                    Buff = null;
                    return NextGame;
                }

                if (next_line.Contains("[Event "))
                {
                    KillNow = true;
                    Buff = next_line;
                }
                else
                {
                    NextGame = NextGame + next_line + Environment.NewLine;
                }
            } while (KillNow == false);

            return NextGame.Trim();
        }
    
        public string[] AllGames()
        {
            List<string> ToReturn = new List<string>();
            bool Continue = true;
            while (Continue)
            {
                string ng = NextGame();
                if (ng == null)
                {
                    Continue = false;
                }
                else
                {
                    ToReturn.Add(ng);
                }
            }
            return ToReturn.ToArray();
        }
    }
}