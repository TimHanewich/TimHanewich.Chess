using System;
using System.IO;
using TimHanewich.Chess.PGN;

namespace TimHanewich.Chess.MoveTree
{
    public class MoveNodeTreeConstructor
    {
        private Stream MassivePgnFileStream;

        //Progress indicators
        public uint GamesAdded;

        //Output
        public MoveNodeTree ResultingMoveNodeTree {get; set;}

        #region "Constructors"

        public MoveNodeTreeConstructor(Stream massive_pgn)
        {
            MassivePgnFileStream = massive_pgn;
        }

        #endregion

        public void Construct(int number_of_games)
        {
            //Create the resulting move node tree if it doesnt exist
            if (ResultingMoveNodeTree == null)
            {
                ResultingMoveNodeTree = new MoveNodeTree();
            }
        }


    }
}