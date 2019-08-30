using System;

namespace GameOfLifeInColor.Core.Models
{
    public struct ColorHSV : IEquatable<ColorHSV>
    {
        private readonly double _hue;
        private readonly double _saturation;
        private readonly double _value;

        public double Hue { get { return _hue; }}
        public double Saturation { get { return _saturation; } }
        public double Value { get { return _value; } }

        public ColorHSV(double h, double s, double v)
        {
            _hue = h.Loop(360);
            _saturation = s.Clamp(0.0,1.0);
            _value = v.Clamp(0.0,1.0);
        }

        public override bool Equals(object obj)
        {
            if (! (obj is ColorHSV) ) return false;
            var color = (ColorHSV) obj;

            return Equals(color);
        }

        public bool Equals(ColorHSV color)
        {
            return Math.Abs(Hue - color.Hue) < 0.0001 && 
                   Math.Abs(Saturation - color.Saturation) < 0.0001 && 
                   Math.Abs(Value - color.Value) < 0.0001;
        }

        public static bool operator ==(ColorHSV x, ColorHSV y)
        {
            return Math.Abs(x.Hue - y.Hue) < 0.0001 && 
                   Math.Abs(x.Saturation - y.Saturation) < 0.0001 && 
                   Math.Abs(x.Value - y.Value) < 0.0001;
        }

        public static bool operator !=(ColorHSV x, ColorHSV y)
        {
            return Math.Abs(x.Hue - y.Hue) > 0.0001 ||
                   Math.Abs(x.Saturation - y.Saturation) > 0.0001 || 
                   Math.Abs(x.Value - y.Value) > 0.0001;
        }

        public override int GetHashCode()
        {
            var hash = Hue + 
                       (379 + 19 * Saturation) + 
                       (941 + 199 * Value);
            
            return Convert.ToInt32(hash);
        }
    }
}
