﻿using System;
using TimHanewich.Chess;
using TimHanewich.Chess.MoveTree;
using TimHanewich.Chess.PGN;
using Newtonsoft.Json;
using System.Collections.Generic;
using PlayEngine.BookMoveSelection;
using ConsoleVisuals;

namespace PlayEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            FullGameEngine();
        }

        public static void FullGameEngine()
        {
            //////////// SETTINGS //////////
            int FollowOpeningBookForMoves = 12;
            int EvalDepth = 6;

            string MoveNodeTreePath = @"C:\Users\tahan\Downloads\MoveTree2.json"; //Path to the JSON-serialized MoveNodeTree object to use for the opening.
            ////////////////////////////////

            
            
            //Get the opening FEN
            Console.WriteLine("Please enter the FEN of the position you would like to start from.");
            Console.WriteLine("If you would like to start a new game from the opening position, leave blank.");
            Console.Write("> ");
            string OpeningPositionFen = Console.ReadLine();

            //Set a variable for the play mode (from position or opening)
            PlayMode MyPlayMode;
            if (OpeningPositionFen == null || OpeningPositionFen == "") //If it was blank, we are playing a full game
            {
                MyPlayMode = PlayMode.FullGame;
            }
            else
            {
                MyPlayMode = PlayMode.FromPosition;
            }

            //Verify eval depth is acceptable, or offer to change it
            Console.WriteLine("Eval depth is set to " + EvalDepth.ToString("#,##0"));
            Console.WriteLine("If you would like to change it, enter the depth you would like");
            Console.WriteLine("If this is acceptable, press enter.");
            Console.Write("Eval Depth: ");
            string NewEvalDepth = Console.ReadLine();
            if (NewEvalDepth != null && NewEvalDepth != "")
            {
                try
                {
                    EvalDepth = Convert.ToInt32(NewEvalDepth);
                    ConsoleVisualsToolkit.WriteLine("Eval depth set to " + EvalDepth.ToString("#,##0"), ConsoleColor.Blue);
                }
                catch
                {
                    ConsoleVisualsToolkit.WriteLine("Invalid eval depth provided! Aborting.", ConsoleColor.Red);
                    return;
                }
            }


            //If we are playing a full game, we will need an opening book. So verify the move node tree path is valid if we are playing a full game
            if (MyPlayMode == PlayMode.FullGame)
            {
                bool MoveNodeTreeExists = System.IO.File.Exists(MoveNodeTreePath);
                if (MoveNodeTreeExists == false)
                {
                    ConsoleVisualsToolkit.WriteLine("The supplied move node tree path was not valid.", ConsoleColor.Red);
                    Console.WriteLine("You can download an opening move tree from the readme at: https://github.com/TimHanewich/TimHanewich.Chess#supplying-an-opening-move-tree");
                    Console.WriteLine("Please provide a path to a valid move tree: ");
                    Console.Write("> ");
                    string newpath = Console.ReadLine();
                    if (System.IO.File.Exists(newpath) == false)
                    {
                        ConsoleVisualsToolkit.WriteLine("The path you provided is invalid.", ConsoleColor.Red);
                        Console.WriteLine("Aborting.");
                        return;
                    }
                    else
                    {
                        MoveNodeTreePath = newpath;
                    }
                }
            }

            //If we are playing a full game, verify the number of book moves to play is acceptable
            if (MyPlayMode == PlayMode.FullGame)
            {
                Console.WriteLine("Number of book moves that will be played is set to " + FollowOpeningBookForMoves.ToString("#,##0"));
                Console.WriteLine("If you would like to change it, enter the number of moves you would like to play.");
                Console.WriteLine("If this is acceptable, press enter.");
                Console.Write("Play book moves #:");
                string NewBookMovesNumber = Console.ReadLine();
                if (NewBookMovesNumber != null && NewBookMovesNumber != "")
                {
                    try
                    {
                        FollowOpeningBookForMoves = Convert.ToInt32(NewBookMovesNumber);
                        ConsoleVisualsToolkit.WriteLine("Will play " + FollowOpeningBookForMoves.ToString("#,##0") + " book moves during the opening", ConsoleColor.Blue);
                    }
                    catch
                    {
                        ConsoleVisualsToolkit.WriteLine("Invalid book move number provided! Aborting.", ConsoleColor.Red);
                        return;
                    }
                }
            }

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


            //Set up MoveNodeTree Variable (for later)
            //If we need the opening move tree (we are playing a FULL game), open it
            MoveNodeTree tree = null;
            if (MyPlayMode == PlayMode.FullGame)
            {
                //Open the move node
                Console.Write("Opening move node tree serialized file...");
                string mvtcontent = System.IO.File.ReadAllText(MoveNodeTreePath);
                Console.WriteLine("Opened");
                Console.Write("Deserializing move node tree... ");
                JsonSerializerSettings jsonsettings = new JsonSerializerSettings();
                jsonsettings.MaxDepth = 256;
                tree = JsonConvert.DeserializeObject<MoveNodeTree>(mvtcontent, jsonsettings);
                Console.WriteLine("Deserialized!");
            }

            //Create a transposition table to serve as a long list of positions and their evaluation (so we don't duplicate efforts later)
            TranspositionTable tt = new TranspositionTable();

            //Start the game
            BoardPosition GAME = null;
            MoveNode PositionInMoveTree = null; //For tracking of book move play
            int BookMovesPlayedSoFar = 0; //For if we play book moves, to not play a number beyond what we are supposed to.
            if (MyPlayMode == PlayMode.FullGame)
            {
                GAME = new BoardPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
                PositionInMoveTree = tree.GameStart; //This would have been fine because the tree variable (move tree) would have been loaded earlier, given we are playing a full game.
            }
            else if (MyPlayMode == PlayMode.FromPosition)
            {
                GAME = new BoardPosition(OpeningPositionFen);
            }
            MoveHistory HISTORY = new MoveHistory(); //Create a move history book to record all moves that are played (this is so we don't play moves that aren't repeated).

            //Create the evaluation engine
            EvaluationEngine ee = new EvaluationEngine();
            ee.EvaluationStatusUpdated += PrintStatus;
            ee.EvaluationEstimatedTimeRemainingUpdated += PrintTimeRemaining;

            //Create a history of move decisions (so there are no repeats)
            MoveDecisionHistory mdh = new MoveDecisionHistory();
            
            //Determine who's turn it is
            bool IsMyTurn;
            if (MyPlayMode == PlayMode.FullGame)
            {
                if (PlayingWhite)
                {
                    IsMyTurn = true;
                }
                else
                {
                    IsMyTurn = false;
                }
            }
            else //We are starting from a position. So it is the turn of the color that was parsed in the FEN position
            {
                if (GAME.ToMove == Color.White)
                {
                    if (PlayingWhite)
                    {
                        IsMyTurn = true;
                    }
                    else
                    {
                        IsMyTurn = false;
                    }
                }
                else //To move is black
                {
                    if (PlayingWhite == false) //I am playing black
                    {
                        IsMyTurn = true;
                    }
                    else
                    {
                        IsMyTurn = false;
                    }
                }
            }
            


            //Move
            while (true)
            {
                if (IsMyTurn == false)
                {

                    //Get their move
                    Move MoveTheyPlayed = null;
                    string algNotationPlayed = null;
                    while (MoveTheyPlayed == null)
                    {
                        //Ask what move they played (in algebraic notation)
                        Console.WriteLine("What move do you play?");
                        Console.Write("In algebraic notation >");
                        algNotationPlayed = Console.ReadLine();

                        //Find it be comparing what they wrote to our generation of the moves
                        Move[] MovesThatCanBePlayed = GAME.AvailableMoves();
                        foreach (Move m in MovesThatCanBePlayed)
                        {
                            if (m.ToAlgebraicNotation(GAME) == algNotationPlayed)
                            {
                                MoveTheyPlayed = m;
                            }
                        }

                        //If we still haven't found it, say it
                        if (MoveTheyPlayed == null)
                        {
                            Console.WriteLine("You enter moved '" + algNotationPlayed + "'");
                            Console.WriteLine("Are you sure this is correct? I don't have a move with notation that matches that.");
                            Console.WriteLine("Trying entering it in again please.");
                            Console.WriteLine("If this keeps up, it is probably an issue of the algebraic notation generation not being correct");
                            Console.WriteLine("OR you entered in moves incorrectly before and now the board I am looking at is different to the board you are looking at.");
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
                                Console.Write("I play ");
                                ConsoleVisualsToolkit.Write(SelectedNode.Move, ConsoleColor.Blue);
                                Console.Write(" (");
                                ConsoleVisualsToolkit.Write(ToPlayMove.FromPosition.ToString(), ConsoleColor.Blue);
                                Console.Write(" to ");
                                ConsoleVisualsToolkit.Write(ToPlayMove.ToPosition.ToString(), ConsoleColor.Blue);
                                Console.WriteLine(")");
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
                        MoveAssessment[] moves = ee.FindBestMoves(GAME, EvalDepth, tt);
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
