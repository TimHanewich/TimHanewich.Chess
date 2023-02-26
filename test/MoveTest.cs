using TimHanewich.Chess;
using TimHanewich.Chess.PGN;
using Xunit;

namespace test
{
    public class MoveTest
    {
        private const string InitialPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        
        [Fact]
        public void CreateMove()
        {
            var board = new BoardPosition(InitialPosition);
            var move = new Move("e4", board);
            Assert.Equal(Position.E2, move.FromPosition);
            Assert.Equal(Position.E4, move.ToPosition);
        }

        [Fact]
        public void ExecuteGame()
        {
            var board = new BoardPosition(InitialPosition);
            string content = System.IO.File.ReadAllText("../../../game1.pgn");
            PgnFile pgn = PgnFile.ParsePgn(content);

            foreach (var moveString in pgn.Moves)
            {
                var move = new Move(moveString, board);
                board.ExecuteMove(move);
            }

            Assert.Equal("1r5r/5Qk1/1p2b1p1/pPp1P1q1/P2p4/3P2P1/1P4B1/4RRK1 b - -", board.ToFEN());
        }
    }
}
