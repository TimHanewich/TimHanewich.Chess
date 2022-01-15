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
        public int GamesAdded;
        public event IntHandler GamedAddedUpdated;

        //Output
        public MoveNodeTree ResultingMoveNodeTree {get; set;}

        #region "Constructors"

        public MoveNodeTreeConstructor(Stream massive_pgn)
        {
            MassivePgnFileStream = massive_pgn;
        }

        #endregion

        public void ContinueConstruction(int number_of_games, int to_depth)
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
                    int DepthExploredInThisGame = 0;
                    MoveNode OnNode = ResultingMoveNodeTree.GameStart;
                    foreach (string move in pgn.Moves)
                    {
                        MoveNode MoveThatWasMade = OnNode.FindChildNode(move);
                        if (MoveThatWasMade == null) //Our tree has not seen this first move yet
                        {
                            MoveNode ThisNewMove = new MoveNode();
                            ThisNewMove.Move = move;
                            if (pgn.IsWhiteVictory)
                            {
                                ThisNewMove.ResultedInWhiteVictory = 1;
                            }
                            else if (pgn.IsDraw)
                            {
                                ThisNewMove.ResultedInDraw = 1;
                            }
                            else if (pgn.IsBlackVictory)
                            {
                                ThisNewMove.ResultedInBlackVictory = 1;
                            }
                            OnNode.AddChildNode(ThisNewMove);
                            OnNode = ThisNewMove;
                        }
                        else //We have seen this move before. So increment the # of times we have seen it, and then set this as the node we are on now
                        {
                            if (pgn.IsWhiteVictory)
                            {
                                MoveThatWasMade.ResultedInWhiteVictory = MoveThatWasMade.ResultedInWhiteVictory + 1;
                            }
                            else if (pgn.IsDraw)
                            {
                                MoveThatWasMade.ResultedInDraw = MoveThatWasMade.ResultedInDraw + 1;
                            }
                            else if (pgn.IsBlackVictory)
                            {
                                MoveThatWasMade.ResultedInBlackVictory = MoveThatWasMade.ResultedInBlackVictory + 1;
                            }
                            OnNode = MoveThatWasMade;
                        }

                        //Increment the depth we have explored
                        DepthExploredInThisGame = DepthExploredInThisGame + 1;
                        if (DepthExploredInThisGame >= to_depth)
                        {
                            break; //break out of the current game. Go to the next game.
                        }
                    }

                    //Increment the # of games added progress indicator
                    GamesAdded = GamesAdded + 1;
                    try
                    {
                        GamedAddedUpdated.Invoke(GamesAdded);
                    }
                    catch
                    {

                    }
                }
                else //If the game WAS null, it means there are no more game lefts. So kill the process. It is over.
                {
                    return;
                }
            }
        }


    }
}