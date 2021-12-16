using System;
using Xunit;
using TimHanewich.Chess;

namespace test
{
    
    public class TranspositionTableTest
    {
        [Fact]
        public void TestStorageAndRetrieval()
        {
            TranspositionTable tt = new TranspositionTable();
            BoardPosition bp = new BoardPosition("3r1q2/pQ3pkp/5Npb/2nPNp2/7P/6P1/P4PK1/4R3 w - - 1 32");
            EvaluationPackage ep = new EvaluationPackage();
            ep.Depth = 5;
            ep.Evaluation = 1.4f;
            tt.Add(bp.BoardRepresentation(), ep);

            EvaluationPackage epr = tt.Find(bp.BoardRepresentation());
            if (epr.Depth == ep.Depth && epr.Evaluation == ep.Evaluation)
            {
                Assert.True(true);
            }
            else
            {
                Assert.True(false);
            }
        }
    }
}