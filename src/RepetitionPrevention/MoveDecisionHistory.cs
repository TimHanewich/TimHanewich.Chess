using System;
using System.Collections;
using System.Collections.Generic;
using TimHanewich.Chess;

namespace TimHanewich.Chess
{
    public class MoveDecisionHistory
    {
        private Dictionary<int[], Move> dict;

        public MoveDecisionHistory()
        {
            dict = new Dictionary<int[], Move>(new IntArrayComparer());
        }

        public void Add(int[] board_rep, Move m)
        {
            dict[board_rep] = m;
        }

        public Move Find(int[] board_rep)
        {
            try
            {
                return dict[board_rep];
            }
            catch
            {
                return null;
            }
        }
    }
}