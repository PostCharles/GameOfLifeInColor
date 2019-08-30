using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GameOfLifeInColor.Core.Models
{
    public static class ColorHSVExtensions
    {
        public static SolidColorBrush ToBrush(this ColorHSV color)
        {
            return new SolidColorBrush(color.ToColor());
        }

        public static Color ToColor(this ColorHSV color)
        {
            int hi = Convert.ToInt32(Math.Floor(color.Hue / 60)) % 6;
            double f = color.Hue / 60 - Math.Floor(color.Hue / 60);

            var value = color.Value * 255;
            byte v = Convert.ToByte(value);
            byte p = Convert.ToByte(value * (1 - color.Saturation));
            byte q = Convert.ToByte(value * (1 - f * color.Saturation));
            byte t = Convert.ToByte(value * (1 - (1 - f) * color.Saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}
