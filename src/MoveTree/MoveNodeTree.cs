using System;

namespace TimHanewich.Chess.MoveTree
{
    public class MoveNodeTree
    {
        //The move nodes
        public MoveNode GameStart {get; set;}

        public MoveNodeTree()
        {
            GameStart = new MoveNode();
        }
    }

}