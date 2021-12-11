using System;
using System.Collections.Generic;

namespace TimHanewich.Chess
{
    public class EvaluationEngine
    {
        public event StringHandler EvaluationStatusUpdated;

        public MoveAssessment[] FindBestMoves(BoardPosition position, int depth)
        {
            //Identify the next moves that can be made
            UpdateStatus("Identifying potential moves...");
            Move[] PotentialMoves = position.AvailableMoves();
            UpdateStatus(PotentialMoves.Length.ToString() + " potential moves identified.");

            //Evaluate each move
            List<MoveAssessment> ToReturnUnordered = new List<MoveAssessment>();
            for (int t = 0; t < PotentialMoves.Length; t++)
            {
                float PercentComplete = Convert.ToSingle(t) / Convert.ToSingle(PotentialMoves.Length);
                Console.WriteLine("Evaluating move " + t.ToString("#,##0") + " / " + PotentialMoves.Length.ToString("#,##0") + " (" + PercentComplete.ToString("#0%") + ")...");
                BoardPosition np = position.Copy();
                np.ExecuteMove(PotentialMoves[t]);
                float eval = np.Evaluate(depth - 1); //Minus one because this is already one step down

                MoveAssessment ma = new MoveAssessment();
                ma.Move = PotentialMoves[t];
                ma.ResultingEvaluation = eval;
                ToReturnUnordered.Add(ma);
            }

            //Sort in order from best to worse
            UpdateStatus("Prioritizing moves...");
            List<MoveAssessment> ToReturn = new List<MoveAssessment>();
            while (ToReturnUnordered.Count > 0)
            {
                MoveAssessment Winner = ToReturnUnordered[0];
                foreach (MoveAssessment ma in ToReturnUnordered)
                {
                    if (position.ToMove == Color.White)
                    {
                        if (ma.ResultingEvaluation > Winner.ResultingEvaluation)
                        {
                            Winner = ma;
                        }
                    }
                    else if (position.ToMove == Color.Black)
                    {
                        if (ma.ResultingEvaluation < Winner.ResultingEvaluation)
                        {
                            Winner = ma;
                        }
                    }
                }
                ToReturn.Add(Winner);
                ToReturnUnordered.Remove(Winner);
            }

            return ToReturn.ToArray();
        }


        private void UpdateStatus(string s)
        {
            try
            {
                EvaluationStatusUpdated.Invoke(s);
            }
            catch
            {

            }
        }

    }
}