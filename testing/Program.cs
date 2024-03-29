﻿using System;
using TimHanewich.Chess;
using TimHanewich.Chess.PGN;
using Newtonsoft.Json;
using TimHanewich.Toolkit;
using System.Collections.Generic;
using System.IO;
using TimHanewich.Chess.MoveTree;
using System.Collections;
using System.Threading.Tasks;

namespace testing
{
    class Program
    {

        private class SimpleMove
        {
            public Position from {get; set;}
            public Position to {get; set;}
        }

        static void Main(string[] args)
        {




            //BoardPosition bp = new BoardPosition("1r3r2/P1P5/1Pk2p2/4p3/6P1/8/4K2P/8 w - - 0 1");
            //Move m = new Move("cxb8=Q", bp);
            


            Stream s = System.IO.File.OpenRead(@"C:\Users\timh\Downloads\lichess_db_standard_rated_2023-03.pgn\lichess_db_standard_rated_2023-03.pgn");
            MassivePgnFileSplitter splitter = new MassivePgnFileSplitter(s);
            
            while (true)
            {
                string pgn_ = splitter.NextGame();
                Console.WriteLine(pgn_);
                PgnFile pgn = PgnFile.ParsePgn(pgn_);

                BoardPosition bp = new BoardPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
                foreach (string m in pgn.Moves)
                {
                    Console.Write(m + " ");
                    Move move = new Move(m, bp);
                    bp.ExecuteMove(move);
                    Console.WriteLine(" made! " + bp.ToFEN());
                }

                Console.Write(JsonConvert.SerializeObject(pgn.Moves, Formatting.None));
                Console.Clear();
            }

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

        public static void TimeEvaluationTest2()
        {
            BoardPosition bp = new BoardPosition("k1n5/3n1n2/1n5n/3Nn3/1N3N2/8/2N1N3/1N1N3K w - - 0 1");
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
