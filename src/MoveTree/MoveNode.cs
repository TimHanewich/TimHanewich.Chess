using System;

namespace TimHanewich.Chess.MoveTree
{
    public class MoveNode
    {
        public string Move {get; set;}
        public uint Occurances {get; set;}
        public MoveNode[] ChildNodes {get; set;}
    }
}