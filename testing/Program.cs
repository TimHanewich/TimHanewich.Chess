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


            foreach (Position p in Enum.GetValues(typeof(Position)))
            {
                BoardPosition bp = new BoardPosition();
                Piece pie = new Piece();
                pie.Color = Color.White;
                pie.Position = p;
                pie.Type = PieceType.Knight;
                bp.AddPiece(pie);

                //dict.Add(Position.A1, new Position[]{Position.B3, Position.C2});

                string PRINT = "dict.Add(Position." + p.ToString() + ", new Position[]{";
                Move[] moves = pie.AvailableMoves(bp, false);
                foreach (Move m in moves)
                {
                    PRINT = PRINT + "Position." + m.ToPosition.ToString() + ",";
                }
                PRINT = PRINT.Substring(0, PRINT.Length - 1);
                PRINT = PRINT + "});";
                Console.WriteLine(PRINT);
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
