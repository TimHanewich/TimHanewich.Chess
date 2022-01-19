using System;
using TimHanewich.Chess;
using TimHanewich.Chess.MoveTree;
using TimHanewich.Chess.PGN;
using Newtonsoft.Json;
using System.Collections.Generic;
using PlayEngine.BookMoveSelection;

namespace PlayEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            FromPositionEngine();
        }

        public static void FullGameEngine()
        {
            //////////// SETTINGS //////////
            int FollowOpeningBookForMoves = 12;
            int EvalDepth = 6;

            string MoveNodeTreePath = @"C:\Users\tahan\Downloads\MoveTree2.json"; //Path to the JSON-serialized MoveNodeTree object to use for the opening.
            ////////////////////////////////

            //Open the move node
            Console.Write("Opening move node tree serialized file...");
            string mvtcontent = System.IO.File.ReadAllText(MoveNodeTreePath);
            Console.WriteLine("Opened");
            Console.Write("Deserializing move node tree... ");
            JsonSerializerSettings jsonsettings = new JsonSerializerSettings();
            jsonsettings.MaxDepth = 256;
            MoveNodeTree tree = JsonConvert.DeserializeObject<MoveNodeTree>(mvtcontent, jsonsettings);
            Console.WriteLine("Deserialized!");

            //What color should the bot play?
            bool PlayingWhite = false;
            Console.WriteLine("What color am I playing?");
            Console.WriteLine("0 = White");
            Console.WriteLine("1 = Black");
            Console.Write("> ");
            string playingstr = Console.ReadLine();
            if (playingstr == "0")
            {
                PlayingWhite = true;
            }
            else if (playingstr == "1")
            {
                PlayingWhite = false;
            }
            else
            {
                Console.WriteLine("I did not understand that.");
                return;
            }
        
            //Turn that boolean field into a color
            Color PlayingAs;
            if (PlayingWhite)
            {
                PlayingAs = Color.White;
            }
            else
            {
                PlayingAs = Color.Black;
            }


            //Start the game
            BoardPosition GAME = new BoardPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            MoveHistory HISTORY = new MoveHistory(); //Create a move history book to record all moves that are played (this is so we don't play moves that aren't repeated).

            //Create the evaluation engine
            EvaluationEngine ee = new EvaluationEngine();
            ee.EvaluationStatusUpdated += PrintStatus;
            ee.EvaluationEstimatedTimeRemainingUpdated += PrintTimeRemaining;

            //Create a history of move decisions (so there are no repeats)
            MoveDecisionHistory mdh = new MoveDecisionHistory();
            
            //For tracking of book move play
            MoveNode PositionInMoveTree = tree.GameStart;
            int BookMovesPlayedSoFar = 0;

            //If it is my turn?
            bool IsMyTurn;
            if (PlayingWhite)
            {
                IsMyTurn = true;
            }
            else
            {
                IsMyTurn = false;
            }


            //Move
            while (true)
            {
                if (IsMyTurn == false)
                {

                    Console.WriteLine("What move do you play?");
                    Console.Write("In algebraic notation >");
                    string algNotationPlayed = Console.ReadLine();

                    //Look for the move that was played
                    Move MoveTheyPlayed = null;
                    Move[] MovesThatCanBePlayed = GAME.AvailableMoves();
                    foreach (Move m in MovesThatCanBePlayed)
                    {
                        if (m.ToAlgebraicNotation(GAME) == algNotationPlayed)
                        {
                            MoveTheyPlayed = m;
                        }
                    }

                    //If we did not find the move, cancel
                    if (MoveTheyPlayed == null)
                    {
                        Console.WriteLine("I could not a find a move that matched that notation!");
                        Console.WriteLine("This is likely because either 1) you entered the move in incorrectly or 2) The PGN of my available moves isn't up to par. Or 3) I did not properly generate all potential next moves.");
                        return;
                    }

                    //If we are still following this in the move tree, attempt to follow
                    if (PositionInMoveTree != null)
                    {

                        //Get the algebraic notation for this move
                        string ThisMoveAlgebraicNotation = MoveTheyPlayed.ToAlgebraicNotation(GAME);

                        //Look to advance the move node
                        MoveNode NextNodeToAdvanceTo = PositionInMoveTree.FindChildNode(ThisMoveAlgebraicNotation);
                        if (NextNodeToAdvanceTo == null)
                        {
                            Console.WriteLine("The move the opponent played was not in the opening book. We have reached the end of the move tree.");
                            PositionInMoveTree = null;
                        }   
                        else
                        {
                            PositionInMoveTree = NextNodeToAdvanceTo;
                        }
                    }

                    

                    //Execute
                    Console.Write("Executing move on my local board... ");
                    GAME.ExecuteMove(MoveTheyPlayed);
                    Console.WriteLine("Move executed.");

                    //Add the move that they played to the move history log
                    HISTORY.AddNextMove(algNotationPlayed);
                }
                else //It is MY TURN!
                {

                    //Go to calculation?
                    bool GoToCalculation = false;

                    //Should a book move be played? If we should play a book move and CAN play a book move, play it. And then set go to calculation as NO.
                    if (BookMovesPlayedSoFar < FollowOpeningBookForMoves && PositionInMoveTree != null) //Play a book move
                    {

                        //Try to find the most popular
                        Console.WriteLine("Going to try to play a book move!");
                        Console.Write("Looking for best move to play...");
                        BookMoveSelector selector = new BookMoveSelector();
                        MoveNode SelectedNode = selector.SelectBookMove(PositionInMoveTree, PlayingAs); 
                        if (SelectedNode != null)
                        {
                            Console.WriteLine("I found the move I want to play: " + SelectedNode.Move);
                            
                            //Find the move
                            Move ToPlayMove = null;
                            Move[] PotentialMoves = GAME.AvailableMoves();
                            foreach (Move m in PotentialMoves)
                            {
                                string ThisMoveAlgNot = m.ToAlgebraicNotation(GAME);
                                if (ThisMoveAlgNot == SelectedNode.Move)
                                {
                                    ToPlayMove = m;
                                }
                            }

                            //If we found the move, play it
                            if (ToPlayMove != null)
                            {
                                Console.WriteLine("I play " + SelectedNode.Move + " (" + ToPlayMove.FromPosition.ToString() + " to " + ToPlayMove.ToPosition.ToString() + ")");
                                Console.Write("Executing move... ");
                                GAME.ExecuteMove(ToPlayMove); //Play the move on the board
                                HISTORY.AddNextMove(SelectedNode.Move); //Add it to the move history log.
                                PositionInMoveTree = SelectedNode; //Advance the current position in the move tree.
                                Console.WriteLine("Move executed!");

                                //Increment the # of book moves played
                                BookMovesPlayedSoFar = BookMovesPlayedSoFar + 1;
                            }
                            else
                            {
                                Console.WriteLine("Unable to find a move to execute that matches that move's Algebraic Notation (from the move tree).");
                                Console.WriteLine("This is probably because the algebraric notation engine is not up to par.");
                                Console.WriteLine("Will evaluate instead.");
                                GoToCalculation = true;
                                PositionInMoveTree = null;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Unable to find most popular move in this position. Maybe no more child nodes exist."); 
                            Console.WriteLine("Going to calculation!");
                            GoToCalculation = true;
                            PositionInMoveTree = null;
                        }
                    }
                    else
                    {
                        GoToCalculation = true;
                    }
                    


                    //Should we calculte? if so, do it
                    if (GoToCalculation)
                    {
                        Console.WriteLine("Going to calculate the best move forward.");
                        Console.WriteLine("Current Position: " + GAME.ToFEN());
                        Console.WriteLine("Finding moves...");
                        MoveAssessment[] moves = ee.FindBestMoves(GAME, EvalDepth);
                        if (moves.Length == 0)
                        {
                            Console.WriteLine("I have no moves to play! I resign.");
                            return;
                        }

                        //Print all potential moves
                        foreach (MoveAssessment ma in moves)
                        {
                            Console.WriteLine(ma.Move.ToAlgebraicNotation(GAME) + " = " + ma.ResultingEvaluation.ToString());
                        }

                        //Select the move to make
                        MoveAssessment ToMakeAssessment = EvaluationMoveSelection.EvaluationMoveSelector.SelectMove(GAME, moves, mdh, HISTORY);
                        Move ToMake = ToMakeAssessment.Move;


                        //Record the move we are going to make (have selected)
                        mdh.Add(GAME.BoardRepresentation(), ToMake);


                        //Print and make the best move to make
                        string AsNotation = ToMake.ToAlgebraicNotation(GAME);
                        GAME.ExecuteMove(ToMake); //Execute move
                        HISTORY.AddNextMove(AsNotation); //Record it in the game move history log
                        mdh.Add(GAME.BoardRepresentation(), ToMake);
                        Console.WriteLine("I play " + AsNotation + " (" + ToMake.FromPosition.ToString() + " --> " + ToMake.ToPosition.ToString() + ")");  
                    }


                }


                //Swap the IsMyTurn
                IsMyTurn = !IsMyTurn;
            }
        }

        public static void FromPositionEngine()
        {
            Console.Write("Original position? FEN>");
            string BeginningFen = Console.ReadLine();
            BoardPosition PlayBoard = new BoardPosition(BeginningFen);
            Console.Write("Depth? >");
            int depth = Convert.ToInt32(Console.ReadLine());
            
            EvaluationEngine ee = new EvaluationEngine();
            ee.EvaluationStatusUpdated += PrintStatus;
            //ee.EvaluationProgressUpdated += PrintPercentComplete;
            ee.EvaluationEstimatedTimeRemainingUpdated += PrintTimeRemaining;

            //Create a history of moves in positions
            MoveDecisionHistory mdh = new MoveDecisionHistory();
            
            while (true)
            {
                //Make our move.
                Console.WriteLine("Finding moves...");
                MoveAssessment[] moves = ee.FindBestMoves(PlayBoard, depth);
                if (moves.Length == 0)
                {
                    Console.WriteLine("I have no moves to play! I resign.");
                }

                //Print all potential moves
                foreach (MoveAssessment ma in moves)
                {
                    Console.WriteLine(ma.Move.ToAlgebraicNotation(PlayBoard) + " = " + ma.ResultingEvaluation.ToString());
                }

                //Select the move to make
                Move ToMake = null;
                Move MoveMadePreviouslyInThisPosition = mdh.Find(PlayBoard.BoardRepresentation());
                if (MoveMadePreviouslyInThisPosition == null)
                {
                    ToMake = moves[0].Move;
                }
                else //We have been in this move previously in this exact position. Try to find a different move
                {
                    foreach (MoveAssessment ma in moves)
                    {
                        if (ma.Move.ToPosition != MoveMadePreviouslyInThisPosition.ToPosition) //This isn't the move we made last time
                        {
                            ToMake = ma.Move;
                            break;
                        }
                    }
                }
                if (ToMake == null)
                {
                    ToMake = moves[0].Move;
                }

                //Record the move we are going to make (have selected)
                mdh.Add(PlayBoard.BoardRepresentation(), ToMake);

                //Print and make the best move to make
                string AsNotation = ToMake.ToAlgebraicNotation(PlayBoard);
                PlayBoard.ExecuteMove(ToMake); //Execute move
                mdh.Add(PlayBoard.BoardRepresentation(), ToMake);
                Console.WriteLine("I play " + AsNotation + " (" + ToMake.FromPosition.ToString() + " --> " + ToMake.ToPosition.ToString() + ")");

                //Make their move
                Console.WriteLine();
                Console.WriteLine("What move do you play?");
                Console.Write("From: ");
                Position From = PositionToolkit.Parse(Console.ReadLine());
                Console.Write("To: ");
                Position To = PositionToolkit.Parse(Console.ReadLine());
                Move m = new Move();
                m.FromPosition = From;
                m.ToPosition = To;
                Console.Write("Executing move... ");
                PlayBoard.ExecuteMove(m);
                Console.WriteLine("Executed.");
                Console.WriteLine();
            }
        }

        public static void PrintStatus(string s)
        {
            Console.WriteLine(s);
        }

        public static void PrintPercentComplete(float f)
        {
            Console.WriteLine("New percent complete: " + f.ToString());
        }

        public static void PrintTimeRemaining(TimeSpan ts)
        {
            Console.WriteLine("Time Remaining: " + ts.Seconds.ToString("#,##0.0") + " seconds");
        }

    }
}
