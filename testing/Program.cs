using System;
using TimHanewich.Chess.PGN;
using Newtonsoft.Json;
using TimHanewich.Toolkit;
using System.Collections.Generic;
using TimHanewich.Chess.Experimental;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeEvaluationTest();
            TimeEvaluationTestV2();
        }

        // public static void PrintStatus(string s)
        // {
        //     Console.WriteLine(s);
        // }

        // public static void PrintPercentComplete(float f)
        // {
        //     Console.WriteLine("New percent complete: " + f.ToString());
        // }

        // public static void PrintTimeRemaining(TimeSpan ts)
        // {
        //     Console.WriteLine("Time Remaining: " + ts.Seconds.ToString("#,##0.0") + " seconds");
        // }

        public static void TimeEvaluationTest()
        {
            TimHanewich.Chess.BoardPosition bp = new TimHanewich.Chess.BoardPosition("1kb4r/3q4/8/4N3/3R2R1/8/1B2K3/8 b k - 0 1");
            HanewichTimer ht = new HanewichTimer();
            Console.Write("Evaluating... ");
            ht.StartTimer();
            float eval = bp.Evaluate(4);
            ht.StopTimer();
            Console.WriteLine(" eval " + eval.ToString() + " in " + ht.GetElapsedTime().TotalSeconds.ToString() + " seconds");
        }

        public static void TimeEvaluationTestV2()
        {
            GameState gs = new GameState("1kb4r/3q4/8/4N3/3R2R1/8/1B2K3/8 b k - 0 1");
            HanewichTimer ht = new HanewichTimer();
            Console.Write("Evaluating... ");
            ht.StartTimer();
            float eval = gs.Evaluate(4);
            ht.StopTimer();
            Console.WriteLine(" eval " + eval.ToString() + " in " + ht.GetElapsedTime().TotalSeconds.ToString() + " seconds");
        }

        // public static void PlayEngine()
        // {
        //     Console.Write("Original position? FEN>");
        //     string BeginningFen = Console.ReadLine();
        //     BoardPosition PlayBoard = new BoardPosition(BeginningFen);
        //     Console.Write("Depth? >");
        //     int depth = Convert.ToInt32(Console.ReadLine());
            
        //     EvaluationEngine ee = new EvaluationEngine();
        //     ee.EvaluationStatusUpdated += PrintStatus;
        //     //ee.EvaluationProgressUpdated += PrintPercentComplete;
        //     ee.EvaluationEstimatedTimeRemainingUpdated += PrintTimeRemaining;
            
        //     while (true)
        //     {
        //         //Make our move.
        //         Console.WriteLine("Finding moves...");
        //         MoveAssessment[] moves = ee.FindBestMoves(PlayBoard, depth);
        //         if (moves.Length == 0)
        //         {
        //             Console.WriteLine("I have no moves to play! I resign.");
        //         }
        //         string AsNotation = moves[0].Move.ToAlgebraicNotation(PlayBoard);
        //         PlayBoard.ExecuteMove(moves[0].Move); //Execute move
        //         Console.WriteLine("I play " + AsNotation + " (" + moves[0].Move.FromPosition.ToString() + " --> " + moves[0].Move.ToPosition.ToString() + ")");

        //         //Make their move
        //         Console.WriteLine();
        //         Console.WriteLine("What move do you play?");
        //         Console.Write("From: ");
        //         Position From = PositionToolkit.Parse(Console.ReadLine());
        //         Console.Write("To: ");
        //         Position To = PositionToolkit.Parse(Console.ReadLine());
        //         Move m = new Move();
        //         m.FromPosition = From;
        //         m.ToPosition = To;
        //         Console.Write("Executing move... ");
        //         PlayBoard.ExecuteMove(m);
        //         Console.WriteLine("Executed.");
        //         Console.WriteLine();
        //     }
        // }


    }
}
