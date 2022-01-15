using System;
using TimHanewich.Chess;
using TimHanewich.Chess.PGN;
using Newtonsoft.Json;
using TimHanewich.Toolkit;
using System.Collections.Generic;
using System.IO;
using TimHanewich.Chess.MoveTree;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Stream s = System.IO.File.OpenRead(@"C:\Users\tahan\Downloads\lichess_db_standard_rated_2016-07.pgn");
            
            MoveNodeTreeConstructor con = new MoveNodeTreeConstructor(s);
            con.ContinueConstruction(5000, 15);
            MoveNode pop = con.ResultingMoveNodeTree.GameStart.MostPopularChildNode();
            while (true)
            {
                Console.WriteLine(pop.Move + " " + pop.ResultedInWhiteVictory.ToString() + " " + pop.ResultedInDraw.ToString() + " " + pop.ResultedInBlackVictory.ToString());
                pop = pop.MostPopularChildNode();
                if (pop == null)
                {
                    Console.WriteLine("Thats it!");
                    return;
                }
            }

            
        }

        public static void PrintStatus(string s)
        {
            Console.WriteLine(s);
        }

        public static void PrintPercentComplete(float f)
        {
            Console.WriteLine("New percent complete: " + f.ToString());
        }

        public static void PrintTimeRemaining(TimeSpan ts)
        {
            Console.WriteLine("Time Remaining: " + ts.Seconds.ToString("#,##0.0") + " seconds");
        }

        public static void TimeEvaluationTest()
        {
            BoardPosition bp = new BoardPosition("1kb4r/3q4/8/4N3/3R2R1/8/1B2K3/8 b k - 0 1");
            HanewichTimer ht = new HanewichTimer();
            Console.Write("Evaluating... ");
            ht.StartTimer();
            float eval = bp.Evaluate(7);
            ht.StopTimer();
            Console.WriteLine(" eval " + eval.ToString() + " in " + ht.GetElapsedTime().TotalSeconds.ToString() + " seconds");
        }
    }
}
