using System;
using TimHanewich.Chess;
using TimHanewich.Chess.MoveTree;

namespace PlayEngine.BookMoveSelection
{
    public class BookMoveAssessment
    {
        public MoveNode SubjectNode {get; set;}

        //For establishing win ranking
        public float WinPercent {get; set;}


        public int PopularityRanking {get; set;}
        public int WinRanking {get; set;}
        public float WeightedRanking
        {
            get
            {
                float PopularityPart = Convert.ToSingle(PopularityRanking) * 0.8f;
                float WinPart = Convert.ToSingle(WinRanking) * 0.2f;
                return PopularityPart + WinPart;
            }
        }
    }
}