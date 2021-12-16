using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TimHanewich.Chess
{
    public class TranspositionTable
    {
        private Dictionary<int[], EvaluationPackage> dict;

        public TranspositionTable()
        {
            dict = new Dictionary<int[], EvaluationPackage>(new IntArrayComparer());
        }

        public KeyValuePair<int[], EvaluationPackage>[] Values
        {
            get
            {
                
                List<KeyValuePair<int[], EvaluationPackage>> ToReturn = new List<KeyValuePair<int[], EvaluationPackage>>();
                foreach (KeyValuePair<int[], EvaluationPackage> kvp in dict)
                {
                    ToReturn.Add(kvp);
                }
                return ToReturn.ToArray();
            }
        }

        public void Add(int[] position, EvaluationPackage ep)
        {
            dict[position] = ep;
        }

        public void Add(BoardPosition bp, EvaluationPackage ep)
        {
            dict[bp.BoardRepresentation()] = ep;
        }

        public EvaluationPackage Find(int[] position)
        {
            try
            {
                return dict[position];
            }
            catch
            {
                return null;
            }
        }

        public EvaluationPackage Find(BoardPosition bp)
        {
            try
            {
                return dict[bp.BoardRepresentation()];
            }
            catch
            {
                return null;
            }
        }
    }
}