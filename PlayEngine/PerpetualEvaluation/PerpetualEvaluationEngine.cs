using System;
using TimHanewich.Chess;


namespace PlayEngine.PerpetualEvaluation
{

    //The whole point of this class is to just saturate the TranspositionTable continuously by "playing itself"
    public class PerpetualEvaluationEngine
    {
        public event StringHandler StatusUpdated;
        public int Depth {get; set;}

        private BoardPosition EvaluateFromPosition; //The position we should currently focus on evaluating from. Get the next best move in this posiiton, then the next best move for that position, then so on. This class will be used by a play engine. When the opponent finally DOES make their move, this will just be updated to the resulting position of the move they made. So that way it evaluate from there

        public void PerpetuallyEvaluate(TranspositionTable tt)
        {

            EvaluationEngine ee = new EvaluationEngine();

            //Continuously evaluate
            bool StopPerpetuallyEvaluating = false;
            while (StopPerpetuallyEvaluating == false)
            {
                //Evaluate, find best move
                UpdateStatus("Finding best move for " + EvaluateFromPosition.ToMove.ToString() + " in this position...");
                MoveAssessment[] assessments = ee.FindBestMoves(EvaluateFromPosition, Depth, tt);
                UpdateStatus(assessments.Length.ToString() + " assessments made.");

                //If there are no more moves forward, kill
                if (assessments.Length == 0)
                {
                    UpdateStatus("There were no more moves forward. Weird! Aborting perpetual evaluation...");
                    return;
                }

                //Execute the best move
                UpdateStatus("Executing best move: " + assessments[0].Move.ToAlgebraicNotation(EvaluateFromPosition));
                EvaluateFromPosition.ExecuteMove(assessments[0].Move);
                UpdateStatus("Move executed!");
            }
        }

        public void SetEvaluateFromPosition(BoardPosition root)
        {
            EvaluateFromPosition = root;
        }


        #region "Toolkit"

        private void UpdateStatus(string txt)
        {
            try
            {
                StatusUpdated.Invoke(txt);
            }
            catch
            {
                
            }
        }
    
        #endregion
    }
}