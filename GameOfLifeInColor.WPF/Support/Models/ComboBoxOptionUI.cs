using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GameOfLifeInColor.Core.Support;
using GameOfLifeInColor.Core.Enumerations;

namespace GameOfLifeInColor.WPF.Support.Models
{
    public class ComboBoxOptionUI : ComboBoxOption
    {
        public ComboBoxOptionUI(ComboBoxOption option) :
                           base(option.PropertyName, option.PropertyType, option.Owner, option.SelectedItemPropertyName, option.DisplayPath) { }

        public override object Construct()
        {
            var comboBox = new ComboBox();
            
            comboBox.DataContext = Owner;
            comboBox.SetBinding(ComboBox.ItemsSourceProperty, new Binding(PropertyName));
            comboBox.SetBinding(ComboBox.SelectedItemProperty, new Binding(SelectedItemPropertyName));
            comboBox.DisplayMemberPath = DisplayPath;

            var label = new Label();
            label.Target = comboBox;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.Content = PropertyName.SplitCamelCase();

            return Constructor.Construct(BuildOrientation.Vertical, label, comboBox);
        }
    }
}
