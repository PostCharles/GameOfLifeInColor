using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeInColor.Core.Support
{
    public class CheckBoxOption : Option
    {
        public CheckBoxOption(string propertyName, Type propertyType, object owner)
        {
            PropertyName = propertyName;
            Owner = owner;
            PropertyType = propertyType;
        }
    }
}
