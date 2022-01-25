using System;
using TimHanewich;
using TimHanewich.Chess;

namespace testing
{
    public class PerftTestResult
    {
        public uint Nodes {get; set;}

        public static PerftTestResult PerformTest(int depth)
        {
            BoardPosition bp = new BoardPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            PerftTestResult ToReturn = new PerftTestResult();
            ToReturn.PerftTestToDepth(bp, depth, ToReturn);
            return ToReturn;
        }

        private void PerftTestToDepth(BoardPosition root, int depth, PerftTestResult result)
        {
            if (depth == 0)
            {
                return;
            }

            BoardPosition[] PossibleNextPositions = root.AvailableMovePositions();
            foreach (BoardPosition pbp in PossibleNextPositions)
            {
                result.Nodes += 1; //Increment node by 1

                //Trigger
                PerftTestToDepth(pbp, depth - 1, result);
            }
        }

    }
}