using System;
using TimHanewich.Chess;
using TimHanewich.Chess.MoveTree;
using TimHanewich.Chess.PGN;

namespace PlayEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        public static void FullGameEngine()
        {
            //////////// SETTINGS //////////
            int EvalDepth = 6;
            int FollowOpeningBookForMoves = 8;
            ////////////////////////////////

            


        }

        public static void FromPositionEngine()
        {
            Console.Write("Original position? FEN>");
            string BeginningFen = Console.ReadLine();
            BoardPosition PlayBoard = new BoardPosition(BeginningFen);
            Console.Write("Depth? >");
            int depth = Convert.ToInt32(Console.ReadLine());
            
            EvaluationEngine ee = new EvaluationEngine();
            ee.EvaluationStatusUpdated += PrintStatus;
            //ee.EvaluationProgressUpdated += PrintPercentComplete;
            ee.EvaluationEstimatedTimeRemainingUpdated += PrintTimeRemaining;

            //Create a history of moves in positions
            MoveDecisionHistory mdh = new MoveDecisionHistory();
            
            while (true)
            {
                //Make our move.
                Console.WriteLine("Finding moves...");
                MoveAssessment[] moves = ee.FindBestMoves(PlayBoard, depth);
                if (moves.Length == 0)
                {
                    Console.WriteLine("I have no moves to play! I resign.");
                }

                //Print all potential moves
                foreach (MoveAssessment ma in moves)
                {
                    Console.WriteLine(ma.Move.ToAlgebraicNotation(PlayBoard) + " = " + ma.ResultingEvaluation.ToString());
                }

                //Select the move to make
                Move ToMake = null;
                Move MoveMadePreviouslyInThisPosition = mdh.Find(PlayBoard.BoardRepresentation());
                if (MoveMadePreviouslyInThisPosition == null)
                {
                    ToMake = moves[0].Move;
                }
                else //We have been in this move previously in this exact position. Try to find a different move
                {
                    foreach (MoveAssessment ma in moves)
                    {
                        if (ma.Move.ToPosition != MoveMadePreviouslyInThisPosition.ToPosition) //This isn't the move we made last time
                        {
                            ToMake = ma.Move;
                            break;
                        }
                    }
                }
                if (ToMake == null)
                {
                    ToMake = moves[0].Move;
                }

                //Record the move we are going to make (have selected)
                mdh.Add(PlayBoard.BoardRepresentation(), ToMake);

                //Print and make the best move to make
                string AsNotation = ToMake.ToAlgebraicNotation(PlayBoard);
                PlayBoard.ExecuteMove(ToMake); //Execute move
                mdh.Add(PlayBoard.BoardRepresentation(), ToMake);
                Console.WriteLine("I play " + AsNotation + " (" + ToMake.FromPosition.ToString() + " --> " + ToMake.ToPosition.ToString() + ")");

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

    }
}
