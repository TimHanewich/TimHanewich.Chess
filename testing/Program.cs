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


            Dictionary<Position, Position[]> dict = new Dictionary<Position, Position[]>();
            dict.Add(Position.A1, new Position[]{Position.B3,Position.C2});dict.Add(Position.A2, new Position[]{Position.B4,Position.C3,Position.C1});
            dict.Add(Position.A3, new Position[]{Position.B5,Position.C4,Position.C2,Position.B1});
            dict.Add(Position.A4, new Position[]{Position.B6,Position.C5,Position.C3,Position.B2});
            dict.Add(Position.A5, new Position[]{Position.B7,Position.C6,Position.C4,Position.B3});
            dict.Add(Position.A6, new Position[]{Position.B8,Position.C7,Position.C5,Position.B4});
            dict.Add(Position.A7, new Position[]{Position.C8,Position.C6,Position.B5});
            dict.Add(Position.A8, new Position[]{Position.C7,Position.B6});
            dict.Add(Position.B1, new Position[]{Position.A3,Position.C3,Position.D2});
            dict.Add(Position.B2, new Position[]{Position.A4,Position.C4,Position.D3,Position.D1});
            dict.Add(Position.B3, new Position[]{Position.A5,Position.C5,Position.D4,Position.D2,Position.A1,Position.C1});
            dict.Add(Position.B4, new Position[]{Position.A6,Position.C6,Position.D5,Position.D3,Position.A2,Position.C2});
            dict.Add(Position.B5, new Position[]{Position.A7,Position.C7,Position.D6,Position.D4,Position.A3,Position.C3});
            dict.Add(Position.B6, new Position[]{Position.A8,Position.C8,Position.D7,Position.D5,Position.A4,Position.C4});
            dict.Add(Position.B7, new Position[]{Position.D8,Position.D6,Position.A5,Position.C5});
            dict.Add(Position.B8, new Position[]{Position.D7,Position.A6,Position.C6});
            dict.Add(Position.C1, new Position[]{Position.B3,Position.D3,Position.A2,Position.E2});
            dict.Add(Position.C2, new Position[]{Position.B4,Position.D4,Position.A3,Position.E3,Position.A1,Position.E1});
            dict.Add(Position.C3, new Position[]{Position.B5,Position.D5,Position.A4,Position.E4,Position.A2,Position.E2,Position.B1,Position.D1});
            dict.Add(Position.C4, new Position[]{Position.B6,Position.D6,Position.A5,Position.E5,Position.A3,Position.E3,Position.B2,Position.D2});
            dict.Add(Position.C5, new Position[]{Position.B7,Position.D7,Position.A6,Position.E6,Position.A4,Position.E4,Position.B3,Position.D3});
            dict.Add(Position.C6, new Position[]{Position.B8,Position.D8,Position.A7,Position.E7,Position.A5,Position.E5,Position.B4,Position.D4});
            dict.Add(Position.C7, new Position[]{Position.A8,Position.E8,Position.A6,Position.E6,Position.B5,Position.D5});
            dict.Add(Position.C8, new Position[]{Position.A7,Position.E7,Position.B6,Position.D6});
            dict.Add(Position.D1, new Position[]{Position.C3,Position.E3,Position.B2,Position.F2});
            dict.Add(Position.D2, new Position[]{Position.C4,Position.E4,Position.B3,Position.F3,Position.B1,Position.F1});
            dict.Add(Position.D3, new Position[]{Position.C5,Position.E5,Position.B4,Position.F4,Position.B2,Position.F2,Position.C1,Position.E1});
            dict.Add(Position.D4, new Position[]{Position.C6,Position.E6,Position.B5,Position.F5,Position.B3,Position.F3,Position.C2,Position.E2});
            dict.Add(Position.D5, new Position[]{Position.C7,Position.E7,Position.B6,Position.F6,Position.B4,Position.F4,Position.C3,Position.E3});
            dict.Add(Position.D6, new Position[]{Position.C8,Position.E8,Position.B7,Position.F7,Position.B5,Position.F5,Position.C4,Position.E4});
            dict.Add(Position.D7, new Position[]{Position.B8,Position.F8,Position.B6,Position.F6,Position.C5,Position.E5});
            dict.Add(Position.D8, new Position[]{Position.B7,Position.F7,Position.C6,Position.E6});
            dict.Add(Position.E1, new Position[]{Position.D3,Position.F3,Position.C2,Position.G2});
            dict.Add(Position.E2, new Position[]{Position.D4,Position.F4,Position.C3,Position.G3,Position.C1,Position.G1});
            dict.Add(Position.E3, new Position[]{Position.D5,Position.F5,Position.C4,Position.G4,Position.C2,Position.G2,Position.D1,Position.F1});
            dict.Add(Position.E4, new Position[]{Position.D6,Position.F6,Position.C5,Position.G5,Position.C3,Position.G3,Position.D2,Position.F2});
            dict.Add(Position.E5, new Position[]{Position.D7,Position.F7,Position.C6,Position.G6,Position.C4,Position.G4,Position.D3,Position.F3});
            dict.Add(Position.E6, new Position[]{Position.D8,Position.F8,Position.C7,Position.G7,Position.C5,Position.G5,Position.D4,Position.F4});
            dict.Add(Position.E7, new Position[]{Position.C8,Position.G8,Position.C6,Position.G6,Position.D5,Position.F5});
            dict.Add(Position.E8, new Position[]{Position.C7,Position.G7,Position.D6,Position.F6});
            dict.Add(Position.F1, new Position[]{Position.E3,Position.G3,Position.D2,Position.H2});
            dict.Add(Position.F2, new Position[]{Position.E4,Position.G4,Position.D3,Position.H3,Position.D1,Position.H1});
            dict.Add(Position.F3, new Position[]{Position.E5,Position.G5,Position.D4,Position.H4,Position.D2,Position.H2,Position.E1,Position.G1});
            dict.Add(Position.F4, new Position[]{Position.E6,Position.G6,Position.D5,Position.H5,Position.D3,Position.H3,Position.E2,Position.G2});
            dict.Add(Position.F5, new Position[]{Position.E7,Position.G7,Position.D6,Position.H6,Position.D4,Position.H4,Position.E3,Position.G3});
            dict.Add(Position.F6, new Position[]{Position.E8,Position.G8,Position.D7,Position.H7,Position.D5,Position.H5,Position.E4,Position.G4});
            dict.Add(Position.F7, new Position[]{Position.D8,Position.H8,Position.D6,Position.H6,Position.E5,Position.G5});
            dict.Add(Position.F8, new Position[]{Position.D7,Position.H7,Position.E6,Position.G6});
            dict.Add(Position.G1, new Position[]{Position.F3,Position.H3,Position.E2});
            dict.Add(Position.G2, new Position[]{Position.F4,Position.H4,Position.E3,Position.E1});
            dict.Add(Position.G3, new Position[]{Position.F5,Position.H5,Position.E4,Position.E2,Position.F1,Position.H1});
            dict.Add(Position.G4, new Position[]{Position.F6,Position.H6,Position.E5,Position.E3,Position.F2,Position.H2});
            dict.Add(Position.G5, new Position[]{Position.F7,Position.H7,Position.E6,Position.E4,Position.F3,Position.H3});
            dict.Add(Position.G6, new Position[]{Position.F8,Position.H8,Position.E7,Position.E5,Position.F4,Position.H4});
            dict.Add(Position.G7, new Position[]{Position.E8,Position.E6,Position.F5,Position.H5});
            dict.Add(Position.G8, new Position[]{Position.E7,Position.F6,Position.H6});
            dict.Add(Position.H1, new Position[]{Position.G3,Position.F2});
            dict.Add(Position.H2, new Position[]{Position.G4,Position.F3,Position.F1});
            dict.Add(Position.H3, new Position[]{Position.G5,Position.F4,Position.F2,Position.G1});
            dict.Add(Position.H4, new Position[]{Position.G6,Position.F5,Position.F3,Position.G2});
            dict.Add(Position.H5, new Position[]{Position.G7,Position.F6,Position.F4,Position.G3});
            dict.Add(Position.H6, new Position[]{Position.G8,Position.F7,Position.F5,Position.G4});
            dict.Add(Position.H7, new Position[]{Position.F8,Position.F6,Position.G5});
            dict.Add(Position.H8, new Position[]{Position.F7,Position.G6});


            foreach (Position p in dict.Keys)
            {
                Position[] destinations = dict[p];

                Console.WriteLine("else if (p == Position." + p.ToString() + ")");
                Console.WriteLine("{");
                

                //Positions to write
                string ToWritePos = "";
                foreach (Position dest in destinations)
                {
                    ToWritePos = ToWritePos + "Position." + dest.ToString() + ",";
                }
                ToWritePos = ToWritePos.Substring(0, ToWritePos.Length - 1);

                Console.WriteLine("\t" + "return new Position[]{" + ToWritePos + "};");
                Console.WriteLine("}");

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
