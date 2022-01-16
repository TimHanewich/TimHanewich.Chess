using System;
using TimHanewich.Chess.MoveTree;
using TimHanewich.Chess.PGN;
using TimHanewich.Chess;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlayEngine.BookMoveSelection
{
    public class BookMoveSelector
    {
        public MoveNode SelectBookMove(MoveNode on_node, Color playing_as)
        {
            //Create an assessment for each.
            List<BookMoveAssessment> BookMoveAssessments = new List<BookMoveAssessment>();
            foreach (MoveNode ChildNode in on_node.ChildNodes)
            {
                BookMoveAssessment assessment = new BookMoveAssessment();
                assessment.SubjectNode = ChildNode;
                BookMoveAssessments.Add(assessment);
            }

            //Give each a win %
            foreach (BookMoveAssessment assessment in BookMoveAssessments)
            {
                if (playing_as == Color.White)
                {
                    assessment.WinPercent = Convert.ToSingle(assessment.SubjectNode.ResultedInWhiteVictory) / Convert.ToSingle(assessment.SubjectNode.Occurences);
                }
                else if (playing_as == Color.Black)
                {
                    assessment.WinPercent = Convert.ToSingle(assessment.SubjectNode.ResultedInBlackVictory) / Convert.ToSingle(assessment.SubjectNode.Occurences);
                }
            }

            //Arrange them by popularity
            List<BookMoveAssessment> ArrangedByPopularity = new List<BookMoveAssessment>();
            while (BookMoveAssessments.Count > 0)
            {
                BookMoveAssessment winner = BookMoveAssessments[0];
                foreach (BookMoveAssessment bma in BookMoveAssessments)
                {
                    if (bma.SubjectNode.Occurences > winner.SubjectNode.Occurences)
                    {
                        winner = bma;
                    }
                }
                ArrangedByPopularity.Add(winner);
                BookMoveAssessments.Remove(winner);
            }
            BookMoveAssessments = ArrangedByPopularity;

            //Rank each according to popularity
            for (int i = 0; i < BookMoveAssessments.Count; i++)
            {
                BookMoveAssessments[i].PopularityRanking = i + 1;
            }

            //Arrange by Win %
            List<BookMoveAssessment> ArrangedByWin = new List<BookMoveAssessment>();
            while (BookMoveAssessments.Count > 0)
            {
                BookMoveAssessment winner = BookMoveAssessments[0];
                foreach (BookMoveAssessment bma in BookMoveAssessments)
                {
                    if (bma.WinPercent > winner.WinPercent)
                    {
                        winner = bma;
                    }
                }
                ArrangedByWin.Add(winner);
                BookMoveAssessments.Remove(winner);
            }
            BookMoveAssessments = ArrangedByWin;

            //Rank each according to win %
            for (int i = 0; i < BookMoveAssessments.Count; i++)
            {
                BookMoveAssessments[i].WinRanking = i + 1;
            }

            //Arrange by weighted ranking
            List<BookMoveAssessment> ArrangedByWeighted = new List<BookMoveAssessment>();
            while (BookMoveAssessments.Count > 0)
            {
                BookMoveAssessment winner = BookMoveAssessments[0];
                foreach (BookMoveAssessment bma in BookMoveAssessments)
                {
                    if (bma.WeightedRanking < winner.WeightedRanking)
                    {
                        winner = bma;
                    }
                }
                ArrangedByWeighted.Add(winner);
                BookMoveAssessments.Remove(winner);
            }
            BookMoveAssessments = ArrangedByWeighted;


            //Return the #1
            return BookMoveAssessments[0].SubjectNode;
        }
    }
}