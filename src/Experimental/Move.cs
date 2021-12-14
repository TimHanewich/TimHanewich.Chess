using System;

namespace TimHanewich.Chess.Experimental
{
    public struct Move
    {
        public Position Origin {get; set;}
        public Position Destination {get; set;}

        public Move(Position from, Position to)
        {
            Origin = from;
            Destination = to;
        }
    }
}