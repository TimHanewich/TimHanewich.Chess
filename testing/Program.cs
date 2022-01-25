using System;
using TimHanewich.Chess;
using TimHanewich.Chess.PGN;
using Newtonsoft.Json;
using TimHanewich.Toolkit;
using System.Collections.Generic;
using System.IO;
using TimHanewich.Chess.MoveTree;
using PlayEngine;
using System.Collections;
using PlayEngine.PerpetualEvaluation;
using System.Threading.Tasks;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {

            BoardPosition bp = new BoardPosition("8/5pk1/Q5p1/P7/7p/5K1P/3prPP1/8 b - - 0 1");
            TranspositionTable tt = new TranspositionTable();
            HanewichTimer ht = new HanewichTimer();

            //Eval
            ht.StartTimer();
            Console.Write("Evaluating... ");
            float eval1 = bp.Evaluate(8, tt);
            ht.StopTimer();
            Console.WriteLine(ht.GetElapsedTime().TotalSeconds.ToString("#,##0.000"));

            //Eval 2
            ht.StartTimer();
            Console.Write("Evaluating again... ");
            float eval2 = bp.Evaluate(8, tt);
            ht.StopTimer();
            Console.WriteLine(ht.GetElapsedTime().TotalSeconds.ToString("#,##0.000"));


            Console.WriteLine("Eval 1: " + eval1.ToString("#,##0.000"));
            Console.WriteLine("Eval 2: " + eval2.ToString("#,##0.000"));
            Console.WriteLine("In transposition table: " + tt.Values.Length.ToString("#,##0"));
            
            
        }

        public static void TimeEvaluationTest()
        {
            BoardPosition bp = new BoardPosition("1kb4r/3q4/8/4N3/3R2R1/8/1B2K3/8 b k - 0 1");
            HanewichTimer ht = new HanewichTimer();
            Console.Write("Evaluating... ");
            ht.StartTimer();
            float eval = bp.Evaluate(6);
            ht.StopTimer();
            Console.WriteLine(" eval " + eval.ToString() + " in " + ht.GetElapsedTime().TotalSeconds.ToString() + " seconds");
        }

        public static void PrintStatus(string s)
        {
            Console.WriteLine(s);
        }
    
        public static void ConstructMoveTree()
        {
            Stream s = System.IO.File.OpenRead(@"C:\Users\tahan\Downloads\lichess_db_standard_rated_2016-07.pgn");
            MoveNodeTreeConstructor con = new MoveNodeTreeConstructor(s);
            con.GamesProcessedUpdated += GamesProcessedUpdateHandler;
            Console.Write("Constructing... ");
            con.ContinueConstruction(200000, 50);
            Console.WriteLine("Construction complete!");
            
            //Save
            Console.Write("Saving to file... ");
            System.IO.File.WriteAllText(@"C:\Users\tahan\Downloads\MoveTree2.json", JsonConvert.SerializeObject(con.ResultingMoveNodeTree));
            Console.WriteLine("Saved!");
        }

        public static void GamesProcessedUpdateHandler(int val)
        {
            Console.WriteLine("Games processed: " + val.ToString("#,##0"));
        }
    }
}
