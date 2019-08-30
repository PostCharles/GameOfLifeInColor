using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeInColor.Core.Models
{
    public struct Cell : IEquatable<Cell>
    {
        private readonly ColorHSV _color;
        private readonly bool _isAlive;

        public ColorHSV Color { get { return _color; } }
        public bool IsAlive { get { return _isAlive; } }

        public Cell(ColorHSV color)
        {
            _color = color;
            _isAlive = true;
        }

        public Cell(double hue, double saturation, double value)
        {
            _color = new ColorHSV(hue,saturation,value);
            _isAlive = true;
        }

        public Cell(bool isAlive = false)
        {
            _color = new ColorHSV();
            _isAlive = isAlive;
        }

        public override bool Equals(object obj)
        {
            if (! (obj is Cell) ) return false;
            var cell = (Cell) obj;

            return Equals(cell);
        }

        public bool Equals(Cell cell)
        {
            return IsAlive == cell.IsAlive && Color == cell.Color;
        }

        public static bool operator ==(Cell x, Cell y)
        {
            return x.IsAlive == y.IsAlive && x.Color == y.Color;
        }

        public static bool operator !=(Cell x, Cell y)
        {
            return x.IsAlive != y.IsAlive || x.Color != y.Color;
        }

        public override int GetHashCode()
        {
            return (IsAlive ? 5000 : 0) + Color.GetHashCode();
        }
    }
}
