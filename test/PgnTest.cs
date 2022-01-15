using System;
using TimHanewich.Chess;
using TimHanewich.Chess.PGN;
using Xunit;

namespace test
{
    public class PgnTest
    {
        [Fact]
        public void PgnParsingTest()
        {
            string content = System.IO.File.ReadAllText("../../../game1.pgn");
            PgnFile pgn = PgnFile.ParsePgn(content);
            Assert.True(true);
        }
    }
}