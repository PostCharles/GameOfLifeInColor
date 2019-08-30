using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeInColor.Core.Support
{
    public class ComboBoxOption : Option
    {
        public string DisplayPath { get; set; }
        public string SelectedItemPropertyName { get; set; }

        public ComboBoxOption(string propertyName, Type propertyType, object owner, string selectedItemPropertyName, string displayPath)
        {
            PropertyName = propertyName;
            Owner = owner;
            PropertyType = propertyType;
            SelectedItemPropertyName = selectedItemPropertyName;
            DisplayPath = displayPath;
        }
    }
}
