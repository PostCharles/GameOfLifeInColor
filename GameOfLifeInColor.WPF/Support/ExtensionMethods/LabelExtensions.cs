using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GameOfLifeInColor.WPF.Support.ExtensionMethods
{
    public static class LabelExtensions
    {
        public static Label BuildStyledLabel(this Label label, string text)
        {
            var textBlock = BuildTextBlock();
            textBlock.Text = text;

            return GetStyledLabel(label, textBlock); 
        }

        public static Label BuildStyledLabel(this Label label, object dataContext, string propertyName)
        {
            var textBlock = BuildTextBlock();
            textBlock.DataContext = dataContext;
            textBlock.SetBinding(TextBlock.TextProperty, new Binding(propertyName));

            return GetStyledLabel(label, textBlock);
        }

        private static Label GetStyledLabel(Label label, TextBlock textBlock)
        {
            label.Content = textBlock;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.Margin = new Thickness(10, 0, 10, 2);

            return label;
        }

        private static TextBlock BuildTextBlock()
        {
            var text = new TextBlock();            
            text.TextAlignment = TextAlignment.Center;
            text.TextWrapping = TextWrapping.Wrap;
            text.FontWeight = FontWeights.Normal;

            return text;
        }
    }
}
