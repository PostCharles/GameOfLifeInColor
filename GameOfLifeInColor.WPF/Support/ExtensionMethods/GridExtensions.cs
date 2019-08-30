using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GameOfLifeInColor.WPF.Support.ExtensionMethods
{
    public static class GridExtensions
    {
        public static Grid BuildTwoColumnGrid(this Grid grid)
        {
            var columnDefintionLeft = new ColumnDefinition();
            columnDefintionLeft.Width = new GridLength(1, GridUnitType.Star);
            var columnDefintionRight = new ColumnDefinition();
            columnDefintionRight.Width = new GridLength(2, GridUnitType.Star);
            grid.ColumnDefinitions.Add(columnDefintionLeft);
            grid.ColumnDefinitions.Add(columnDefintionRight);

            return grid;
        }
    }
}
