using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using GameOfLifeInColor.Core.Support;
using System.Text.RegularExpressions;
using System.Windows.Media;
using GameOfLifeInColor.WPF.Converters;
using GameOfLifeInColor.WPF.Support.ExtensionMethods;
using GameOfLifeInColor.Core.Enumerations;

namespace GameOfLifeInColor.WPF.Support.Models
{
    public class TextBoxOptionUI : TextBoxOption
    {
        public TextBoxOptionUI(TextBoxOption option) : base(option.PropertyName,option.PropertyType, option.Owner) {}

        public override object Construct()
        {
            var textBox = new TextBox();
            textBox.DataContext = Owner;
            textBox.SetBinding(TextBox.TextProperty, new Binding(PropertyName));
            textBox.SetValue(Grid.ColumnProperty, 1);
            textBox.HorizontalAlignment = HorizontalAlignment.Stretch;

            var label = new Label();
            label.Target = textBox;
            label.HorizontalAlignment = HorizontalAlignment.Right;
            label.Content = PropertyName.SplitCamelCase() + ":";
            label.SetValue(Grid.ColumnProperty, 0);

            var textBoxContainer = new Grid().BuildTwoColumnGrid();
            textBoxContainer.Children.Add(textBox);
            textBoxContainer.Children.Add(label);


            return Constructor.Construct(BuildOrientation.Vertical, textBoxContainer);
        }
    }
}
