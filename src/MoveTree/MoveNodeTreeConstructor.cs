using System;
using System.IO;
using TimHanewich.Chess.PGN;

namespace TimHanewich.Chess.MoveTree
{
    public class MoveNodeTreeConstructor
    {
        private Stream MassivePgnFileStream;
        private MassivePgnFileSplitter Splitter;

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

        public void ContinueConstruction(int number_of_games)
        {
            //If the stream has not been set, throw an error
            if (MassivePgnFileStream == null)
            {
                throw new Exception("Unable to construct move node tree: Massive PGN file input stream was not set.");
            }

            //Create the resulting move node tree if it doesnt exist
            if (ResultingMoveNodeTree == null)
            {
                ResultingMoveNodeTree = new MoveNodeTree();
            }

            //Make the splitter if it doesn't exist
            if (Splitter == null)
            {
                Splitter = new MassivePgnFileSplitter(MassivePgnFileStream);
            }

            //Do the # of games they asked for.
            for (int t = 0; t < number_of_games; t++)
            {
                string ThisGame = Splitter.NextGame();
                if (ThisGame != null)
                {
                    //Parse this PGN
                    PgnFile pgn = PgnFile.ParsePgn(ThisGame);
                    MoveNode OnNode = ResultingMoveNodeTree.GameStart;
                    foreach (string move in pgn.Moves)
                    {
                        MoveNode MoveThatWasMade = OnNode.FindChildNode(move);
                        if (MoveThatWasMade == null) //Our tree has not seen this first move yet
                        {
                            MoveNode ThisNewMove = new MoveNode();
                            ThisNewMove.Move = move;
                            ThisNewMove.Occurances = 1;
                            OnNode.AddChildNode(ThisNewMove);
                            OnNode = ThisNewMove;
                        }
                        else //We have seen this move before. So increment the # of times we have seen it, and then set this as the node we are on now
                        {
                            MoveThatWasMade.Occurances = MoveThatWasMade.Occurances + 1;
                            OnNode = MoveThatWasMade;
                        }
                    }
                }
            }
        }


    }
}