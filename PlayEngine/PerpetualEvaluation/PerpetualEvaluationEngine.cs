using System;
using TimHanewich.Chess;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace PlayEngine.PerpetualEvaluation
{

    //The whole point of this class is to just saturate the TranspositionTable continuously by "playing itself"
    public class PerpetualEvaluationEngine
    {
        public event StringHandler StatusUpdated;
        public event StringHandler ExceptionEncountered;

        private TranspositionTable LocalTranspositionTableBuffer;

        public PerpetualEvaluationEngine()
        {
            LocalTranspositionTableBuffer = new TranspositionTable();
        }
        
        //Starting
        private bool StopPerpetuallyEvaluating;
        public void Start(BoardPosition EvaluateFromPosition, int Depth)
        {

            try
            {
                EvaluationEngine ee = new EvaluationEngine();

                //Continuously evaluate
                StopPerpetuallyEvaluating = false;
                while (StopPerpetuallyEvaluating == false)
                {
                    //Evaluate, find best move
                    UpdateStatus("Finding best move for " + EvaluateFromPosition.ToMove.ToString() + " in this position...");
                    MoveAssessment[] assessments = ee.FindBestMoves(EvaluateFromPosition, Depth, LocalTranspositionTableBuffer);
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
                UpdateStatus("Stopped!");
                StopConfirmed = true; //Signal that the stop is indeed successful.


            }
            catch (Exception ex)
            {
                string msg = "Exception encountered in perpetual evaluation engine: " + ex.Message;
                RaiseExceptionEvent(msg);
            }

        }

        //Stopping
        private bool StopConfirmed;
        public async Task StopAsync()
        {
            StopConfirmed = false; //When this turns true, the stop is confirmed.
            StopPerpetuallyEvaluating = true; //Asking it to stop
            while (StopConfirmed == false)
            {
                await Task.Delay(100);
            }
        }

        #region "Dumping of TranspositionTable"

        public void DumpTranspositionTable(TranspositionTable dump_to)
        {
            //If the process is currently running, throw an error
            if (StopPerpetuallyEvaluating == false)
            {
                throw new Exception("Unable to dump local transposition table buffer while perpetual evaluation is running. First, stop the perpetual evaluation with the StopAsync method.");
            }
            
            //Dump
            foreach (KeyValuePair<int[], EvaluationPackage> kvp in LocalTranspositionTableBuffer.Values)
            {
                dump_to.Add(kvp.Key, kvp.Value);
            }
        }

        #endregion

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

        private void RaiseExceptionEvent(string msg)
        {
            try
            {
                ExceptionEncountered.Invoke(msg);
            }
            catch
            {

            }
        }
    
        #endregion
    }
}