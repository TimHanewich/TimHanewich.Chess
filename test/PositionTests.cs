using System;
using Xunit;
using TimHanewich.Chess;

namespace test
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(Position.A1, Position.A2)]
        [InlineData(Position.C7, Position.C8)]
        [InlineData(Position.E4, Position.E5)]
        [InlineData(Position.F2, Position.F3)]
        [InlineData(Position.H6, Position.H7)]
        public void TestUp(Position start, Position expected_end)
        {
            Assert.True(start.Up() == expected_end);
        }

        [Theory]
        [InlineData(Position.A1, Position.B1)]
        [InlineData(Position.C7, Position.D7)]
        [InlineData(Position.E3, Position.F3)]
        [InlineData(Position.F2, Position.G2)]
        [InlineData(Position.B6, Position.C6)]
        public void TestRight(Position start, Position expected_end)
        {
            Assert.True(start.Right() == expected_end);
        }

        [Theory]
        [InlineData(Position.B1, Position.A1)]
        [InlineData(Position.C7, Position.B7)]
        [InlineData(Position.E3, Position.D3)]
        [InlineData(Position.F2, Position.E2)]
        [InlineData(Position.B6, Position.A6)]
        public void TestLeft(Position start, Position expected_end)
        {
            Assert.True(start.Left() == expected_end);
        }

        [Theory]
        [InlineData(Position.B3, Position.B2)]
        [InlineData(Position.C7, Position.C6)]
        [InlineData(Position.E3, Position.E2)]
        [InlineData(Position.F2, Position.F1)]
        [InlineData(Position.B6, Position.B5)]
        public void TestDown(Position start, Position expected_end)
        {
            Assert.True(start.Down() == expected_end);
        }
    }
}
