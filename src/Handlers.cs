using System;

namespace TimHanewich.Chess
{
    public delegate void StringHandler(string str);
    public delegate void FloatHandler(float val);
    public delegate void TimeSpanHandler(TimeSpan ts);
    public delegate void IntHandler(int val);
}