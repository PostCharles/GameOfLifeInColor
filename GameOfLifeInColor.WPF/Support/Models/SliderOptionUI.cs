using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using GameOfLifeInColor.Core.Support;
using GameOfLifeInColor.WPF.Converters;
using GameOfLifeInColor.WPF.Support.ExtensionMethods;
using GameOfLifeInColor.Core.Enumerations;

namespace GameOfLifeInColor.WPF.Support.Models
{
    public class SliderOptionUI : SliderOption
    {
        public SliderOptionUI(SliderOption option) : 
                          base(option.PropertyName, option.PropertyType, option.Owner,option.Min, option.Max) { }

        public override object Construct()
        {
            var slider = new Slider();
            slider.Minimum = Min;
            slider.Maximum = Max;
            slider.Background = Colors.Transparent.ToBrush();
            slider.DataContext = Owner;
            slider.SetBinding(Slider.ValueProperty, new Binding(PropertyName));
            slider.Margin = new Thickness(0,0,0,2);
            
            var label = new Label();
            label.Target = slider;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Content = PropertyName.SplitCamelCase();
            label.SetValue(Grid.ColumnProperty, 0);
            
            var labelBinding = new Binding("Value");
            labelBinding.Converter = new DoubleToIntConverter();
            
            var valueLabel = new Label();
            valueLabel.DataContext = slider;
            valueLabel.HorizontalAlignment = HorizontalAlignment.Right;
            valueLabel.SetBinding(Label.ContentProperty, labelBinding);
            valueLabel.SetValue(Grid.ColumnProperty, 1);

            var grid = new Grid().BuildTwoColumnGrid();
            grid.Children.Add(label);
            grid.Children.Add(valueLabel);

            return Constructor.Construct(BuildOrientation.Vertical, grid, slider);
        }
    }
}
