using System;

namespace TimHanewich.Chess
{
    public class EvaluationPackage
    {
        public float Evaluation {get; set;}
        public int Depth {get; set;} //The depth the evaluation was valued at.
    }
}