using System;
using TimHanewich.Chess;
using TimHanewich.Chess.PGN;
using Newtonsoft.Json;
using TimHanewich.Toolkit;
using System.Collections.Generic;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {

            BoardPosition bp = new BoardPosition("6k1/8/6b1/8/5b2/4P1P1/8/K7 w - - 2 2");



            Move[] Moves = bp.AvailableMoves();
            foreach (Move m in Moves)
            {
                Console.WriteLine(m.ToAlgebraicNotation(bp));
            }
            
            
        }

        public static void PrintStatus(string s)
        {
            Console.WriteLine(s);
        }

        public static void TimeEvaluationTest()
        {
            BoardPosition bp = new BoardPosition("1kb4r/3q4/8/4N3/3R2R1/8/1B2K3/8 b k - 0 1");
            HanewichTimer ht = new HanewichTimer();
            Console.Write("Evaluating... ");
            ht.StartTimer();
            float eval = bp.Evaluate(3);
            ht.StopTimer();
            Console.WriteLine(" eval " + eval.ToString() + " in " + ht.GetElapsedTime().TotalSeconds.ToString() + " seconds");
        }

        public static void PlayEngine()
        {
            Console.Write("Original position? FEN>");
            string BeginningFen = Console.ReadLine();
            BoardPosition PlayBoard = new BoardPosition(BeginningFen);
            Console.Write("Depth? >");
            int depth = Convert.ToInt32(Console.ReadLine());
            
            EvaluationEngine ee = new EvaluationEngine();
            ee.EvaluationStatusUpdated += PrintStatus;
            
            while (true)
            {
                //Make our move.
                Console.WriteLine("Finding moves...");
                MoveAssessment[] moves = ee.FindBestMoves(PlayBoard, depth);
                if (moves.Length == 0)
                {
                    Console.WriteLine("I have no moves to play! I resign.");
                }
                PlayBoard.ExecuteMove(moves[0].Move); //Execute move
                string AsNotation = moves[0].Move.ToAlgebraicNotation(PlayBoard);
                Console.WriteLine("I play " + AsNotation + " (" + moves[0].Move.FromPosition.ToString() + " --> " + moves[0].Move.ToPosition.ToString() + ")");

                //Make their move
                Console.WriteLine();
                Console.WriteLine("What move do you play?");
                Console.Write("From: ");
                Position From = PositionToolkit.Parse(Console.ReadLine());
                Console.Write("To: ");
                Position To = PositionToolkit.Parse(Console.ReadLine());
                Move m = new Move();
                m.FromPosition = From;
                m.ToPosition = To;
                Console.Write("Executing move... ");
                PlayBoard.ExecuteMove(m);
                Console.WriteLine("Executed.");
                Console.WriteLine();
            }
        }
    }
}
