using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLifeInColor.Core.Support
{
    public class TextBoxOption : Option
    {
        public TextBoxOption(string propertyName, Type propertyType, object owner)
        {
            PropertyName = propertyName;
            Owner = owner;
            PropertyType = propertyType;
        }
    }
}
