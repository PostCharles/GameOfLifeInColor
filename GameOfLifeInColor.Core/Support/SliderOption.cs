using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeInColor.Core.Support
{
    public class SliderOption : Option
    {
        public double Min { get; set; }
        public double Max { get; set; }


        public SliderOption(string propertyName, Type propertyType, object owner, double min, double max)
        {
            PropertyName = propertyName;
            Owner = owner;
            PropertyType = propertyType;
            Min = min;
            Max = max;
        }

        //public SliderOption(string propertyName, Type propertyType, object owner, ) : 
        //               this(propertyName, propertyType, owner)
        //{
        //    Min = min;
        //    Max = max;
        //}


    }
}
