using System;
using System.Collections.Generic;

namespace TimHanewich.Chess.PGN
{
    public class PgnParserLite
        {
            public string Event { get; set; }
            public string Site { get; set; }
            public DateTime Date { get; set; }
            public string Round { get; set; }
            public string White { get; set; }
            public string Black { get; set; }
            public string Result { get; set; }
            public DateTime UtcDateTime { get; set; }
            public int WhiteElo { get; set; }
            public int BlackElo { get; set; }
            public int WhiteRatingDifference { get; set; }
            public int BlackRatingDifference { get; set; }
            public string ECO { get; set; }
            public string OpeningName { get; set; }
            public TimeSpan TimeControl { get; set; }
            public TimeSpan TimeIncrement { get; set; }
            public string Termination { get; set; }
            public string[] Moves { get; set; }

            public static PgnParserLite ParsePgn(string pgn)
            {

                PgnParserLite ppl = new PgnParserLite();
                try
                {
                    ppl.Event = ppl.GetProperty(pgn, "Event");
                }
                catch
                {

                }
                try
                {
                    ppl.Site = ppl.GetProperty(pgn, "Site");
                }
                catch
                {

                }
                try
                {
                    ppl.Date = DateTime.Parse(ppl.GetProperty(pgn, "Date"));
                }
                catch
                {

                }
                try
                {
                    ppl.Round = ppl.GetProperty(pgn, "Round");
                }
                catch
                {

                }
                try
                {
                    ppl.White = ppl.GetProperty(pgn, "White");
                }
                catch
                {

                }
                try
                {
                    ppl.Black = ppl.GetProperty(pgn, "Black");
                }
                catch
                {

                }
                try
                {
                    ppl.Result = ppl.GetProperty(pgn, "Result");
                }
                catch
                {

                }
                try
                {
                    DateTime utcd = DateTime.Parse(ppl.GetProperty(pgn, "UTCDate"));
                    DateTime utct = DateTime.Parse(ppl.GetProperty(pgn, "UTCTime"));
                    DateTime utcdt = DateTime.Parse(utcd.Month.ToString() + "/" + utcd.Day.ToString() + "/" + utcd.Year.ToString() + " " + utct.Hour.ToString() + ":" + utct.Minute.ToString() + ":" + utct.Second.ToString());
                    ppl.UtcDateTime = utcdt;
                }
                catch
                {

                }
                try
                {
                    ppl.WhiteElo = Convert.ToInt32(ppl.GetProperty(pgn, "WhiteElo"));
                }
                catch
                {

                }
                try
                {
                    ppl.BlackElo = Convert.ToInt32(ppl.GetProperty(pgn, "BlackElo"));
                }
                catch
                {

                }
                try
                {
                    ppl.WhiteRatingDifference = Convert.ToInt32(ppl.GetProperty(pgn, "WhiteRatingDiff"));
                }
                catch
                {

                }
                try
                {
                    ppl.BlackRatingDifference = Convert.ToInt32(ppl.GetProperty(pgn, "BlackRatingDiff"));
                }
                catch
                {

                }
                try
                {
                    ppl.ECO = ppl.GetProperty(pgn, "ECO");
                }
                catch
                {

                }
                try
                {
                    ppl.OpeningName = ppl.GetProperty(pgn, "Opening");
                }
                catch
                {

                }
                try
                {
                    string time_control = ppl.GetProperty(pgn, "TimeControl");
                    List<string> Splitter = new List<string>();
                    Splitter.Add("+");
                    string[] parts = time_control.Split(Splitter.ToArray(), StringSplitOptions.None);
                    ppl.TimeControl = new TimeSpan(0, 0, Convert.ToInt32(parts[0]));
                    ppl.TimeIncrement = new TimeSpan(0, 0, Convert.ToInt32(parts[1]));
                }
                catch
                {

                }
                try
                {
                    ppl.Termination = ppl.GetProperty(pgn, "Termination");
                }
                catch
                {

                }


                //Get moves
                int loc1;
                loc1 = pgn.IndexOf("1. ");
                if (loc1 != -1)
                {
                    string Moves = pgn.Substring(loc1);
                    Moves = Moves.Trim();

                    //Strip out the comments
                    string MovesStrippedOfComments = "";
                    bool InComment = false;
                    foreach (char c in Moves)
                    {
                        if (InComment == false)
                        {
                            if (c.ToString() == "{")
                            {
                                InComment = true;
                            }
                            else
                            {
                                MovesStrippedOfComments = MovesStrippedOfComments + c.ToString();
                            }
                        }
                        else
                        {
                            if (c.ToString() == "}")
                            {
                                InComment = false;
                            }
                        }
                    }
                    Moves = MovesStrippedOfComments;


                    //Pull out the exclamation points and question marks
                    Moves = Moves.Replace("!", "");
                    Moves = Moves.Replace("?", "");

                    //Pull out all triple periods
                    Moves = Moves.Replace("...", ".");

                    //Remove the result
                    Moves = Moves.Replace("1-0", "");
                    Moves = Moves.Replace("0-1", "");
                    Moves = Moves.Replace("1/2-1/2", "");
                    Moves = Moves.Replace("*", "");


                    //Remove the move numbers
                    int t = 1000;
                    for (t = 1000; t >= 1; t--)
                    {
                        Moves = Moves.Replace(t.ToString() + ".", "");
                    }


                    //Take out triple or double spaces
                    Moves = Moves.Replace("   ", " ");
                    Moves = Moves.Replace("  ", " ");

                    //Trim
                    Moves = Moves.Trim();

                    List<string> Splitter = new List<string>();
                    ppl.Moves = Moves.Split(Splitter.ToArray(), StringSplitOptions.None);

                }
                else
                {
                    List<string> blanks = new List<string>();
                    ppl.Moves = blanks.ToArray();
                }

                return ppl;
            }

            private string GetProperty(string raw_pgn, string property_name)
            {
                int loc1;
                int loc2;

                loc1 = raw_pgn.IndexOf("[" + property_name + " ");
                if (loc1 == -1)
                {
                    throw new Exception("Unable to find property '" + property_name + "' in PGN data.");
                }
                loc1 = raw_pgn.IndexOf("\"", loc1 + 1);
                loc2 = raw_pgn.IndexOf("\"", loc1 + 1);
                string val = raw_pgn.Substring(loc1 + 1, loc2 - loc1 - 1);
                return val;
            }
        }
}