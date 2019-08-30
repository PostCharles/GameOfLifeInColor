using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using GameOfLifeInColor.Core.Support;
using GameOfLifeInColor.Core.Enumerations;
using GameOfLifeInColor.WPF.Support.ExtensionMethods;

namespace GameOfLifeInColor.WPF.Support.Models
{
    public class TextBlockOptionUI : TextBlockOption
    {
        public TextBlockOptionUI(TextBlockOption option) : base(option.PropertyName, option.PropertyType, option.Owner) { }

        public override object Construct()
        {
            var textBlock = new Label().BuildStyledLabel(Owner, PropertyName);

            var label = new Label();
            label.Content = PropertyName.SplitCamelCase();
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.Target = textBlock;

            return Constructor.Construct(BuildOrientation.Vertical, label, textBlock);
        }
    }
}
