using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GameOfLifeInColor.Core.Enumerations;
using GameOfLifeInColor.Core.Support;

namespace GameOfLifeInColor.WPF.Support.Models
{
    public class CheckBoxOptionUI : CheckBoxOption
    {
        public CheckBoxOptionUI(CheckBoxOption option) : base(option.PropertyName, option.PropertyType, option.Owner) { }

        public override object Construct()
        {
            var binding = new Binding(PropertyName);

            var checkBox = new CheckBox();
            checkBox.HorizontalAlignment = HorizontalAlignment.Right;
            checkBox.VerticalAlignment = VerticalAlignment.Center;
            checkBox.Margin = new Thickness(0,-2,0,-3);
            checkBox.DataContext = Owner;
            checkBox.SetBinding(CheckBox.IsCheckedProperty, binding);

            var label = new Label();
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Margin = new Thickness(0, -2, 0, 0);
            label.Target = checkBox;
            label.Content = PropertyName.SplitCamelCase() + ":";

            return Constructor.Construct(BuildOrientation.Horizontal, label, checkBox);
        }
    }
}
