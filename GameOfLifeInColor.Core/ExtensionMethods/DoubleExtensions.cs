namespace System
{
    public static class DoubleExtensions
    {
        public static double Clamp(this double value, double Min, double Max, double Offset = 0)
        {
            if (value > Max) return (Max - Offset);
            if (value < Min) return (Min - Offset);

            return value;
        }

        public static double Loop(this double value, double EndLoop)
        {
            var newValue = Math.Abs(value);
            
            if (newValue > EndLoop) return (newValue % EndLoop);
            return value;
        }       
    }
}
