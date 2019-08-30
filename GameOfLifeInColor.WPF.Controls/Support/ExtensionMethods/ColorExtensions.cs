using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeInColor.Core.Models;

namespace System.Windows.Media
{
    public static class ColorExtensions
    {
        public static ColorHSV ToHSV(this Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            var hue = System.Drawing.Color.FromArgb(color.R, color.G, color.B).GetHue();
            var saturation = (max == 0) ? 0 : 1d - (1d*min/max);
            var value = max/255d;

            return new ColorHSV(hue, saturation, value);

        }

        public static SolidColorBrush ToBrush(this Color color)
        {
            return new SolidColorBrush(color);
        }
    }
}
