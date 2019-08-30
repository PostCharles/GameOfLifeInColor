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
using GameOfLifeInColor.Core.Interfaces;
using GameOfLifeInColor.WPF.Properties;
using GameOfLifeInColor.Core.Enumerations;

namespace GameOfLifeInColor.WPF.Support
{
    public class ContainerConstructor : IContainerConstructor
    {
        public object Construct(BuildOrientation orientation, params object[] Controls)
        {
            var container = new StackPanel();
            container.Orientation = (orientation == BuildOrientation.Horizontal) ? Orientation.Horizontal : Orientation.Vertical;
            foreach (var control in Controls)
            {
                container.Children.Add( (control as UIElement) );
            }

            var border = new Border
            {
                Child = container,
                BorderThickness = new Thickness(1),
                BorderBrush = App.Current.Resources["BorderBrush"] as SolidColorBrush,
                Margin = new Thickness(0, 0, 0, 3)
            };

            return border;
        }
    }
}
