using System;
using TimHanewich.Chess;
using System.Collections.Generic;
using System.Linq;

namespace PlayEngine.EvaluationMoveSelection
{
    public class EvaluationMoveSelector
    {
        public static MoveAssessment SelectMove(BoardPosition position, MoveAssessment[] moves, MoveDecisionHistory mdh, MoveHistory mh)
        {
            
            #region "Error checking"

            if (moves.Length == 0)
            {
                throw new Exception("Unable to select move. None are available!");
            }

            //If there is only one available, return that.
            if (moves.Length == 1)
            {
                return moves[0];
            }

            #endregion
            
            //Have we played a move in this EXACT position before? The MoveDecisionHistory will help with that. Get a list of moves we can choose from after taking this out.
            List<MoveAssessment> ToChooseFrom = new List<MoveAssessment>();
            Move MovePreviouslyPlayedInThisExactPosition = mdh.Find(position.BoardRepresentation());
            if (MovePreviouslyPlayedInThisExactPosition != null)
            {
                foreach (MoveAssessment ma in moves)
                {
                    if (ma.Move.FromPosition != MovePreviouslyPlayedInThisExactPosition.FromPosition && ma.Move.ToPosition != MovePreviouslyPlayedInThisExactPosition.ToPosition) //This is NOT the move I played previously.
                    {
                        ToChooseFrom.Add(ma);
                    }
                }
            }
            else
            {
                ToChooseFrom.AddRange(moves);
            }


            //If there are now 0 remaining, just return the first from before
            if (ToChooseFrom.Count == 0)
            {
                return moves[0];
            }

            //Get the # of moves with the BEST eval
            int NumberOfMovesWithBestEval = 0;
            foreach (MoveAssessment ma in ToChooseFrom)
            {
                if (ma.ResultingEvaluation == ToChooseFrom[0].ResultingEvaluation)
                {
                    NumberOfMovesWithBestEval = NumberOfMovesWithBestEval + 1;
                }
            }

            //If there is only one move with the best eval, just return it. That is clearly the best. So forget being redundant, just play it
            if (NumberOfMovesWithBestEval == 1)
            {
                return ToChooseFrom[0];
            }

            //Get the moves I have played before
            string[] MovesPlayedBefore = null;
            if (position.ToMove == Color.White)
            {
                MovesPlayedBefore = mh.WhiteMoves;
            }
            else if (position.ToMove == Color.Black)
            {
                MovesPlayedBefore = mh.BlackMoves;
            }
            

            //So we are here.. which means there were MULTIPLE moves with the best eval. So a tie. Go through them all and just play the first one that hasn't been played before.
            foreach (MoveAssessment ma in ToChooseFrom)
            {
                if (ma.ResultingEvaluation == ToChooseFrom[0].ResultingEvaluation) //It is one of the ones tied for best eval
                {
                    if (MovesPlayedBefore.Contains(ma.Move.ToAlgebraicNotation(position)) == false) //We have NOT played this move before
                    {
                        return ma;
                    }
                }
            }

            //We got here. Which means we went through all of the moves that were tied for best move, yet we have already played them all.
            //So just return the first one
            return ToChooseFrom[0];
        }
    }
}