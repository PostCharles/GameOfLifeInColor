using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using GameOfLifeInColor.Core.Interfaces;
using GameOfLifeInColor.Core.Models;
using GameOfLifeInColor.Core.Support;
using GameOfLifeInColor.Infrastructure.RuleSets.Support;

namespace GameOfLifeInColor.Infrastructure.RuleSets
{
    public class AverageNeighborsRuleSet : ObservableObjectBase, IRuleSet 
    {

        private int _newIndex;
        private bool _isRunning;
        private Random Rand { get; set; }

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                RaisePropertyChanged(() => IsRunning);
            }
        }

        public CellCollection Cells { get; set; }
        public ICollection<Option> Options { get; set; }
        public bool CancelationToken { get; set; }
        public bool IsComplete { get; set; }       

       


        public AverageNeighborsRuleSet()
        {
            Rand = new Random();
        }


        public async Task<bool> Start()
        {
            if (IsRunning) return false;
            
            IsRunning = true;

            while(!IsComplete)
            {
                IsComplete = true;

                await Task.Run(() => UpdateCells());
                Cells.OnCollectionUpdated();
                

                if (CancelationToken) IsComplete = true;
            }

            IsRunning = false;
            IsComplete = false;
            CancelationToken = false;

            return false;
        }

        private void UpdateCells()
        {
            var temp = new Cell[Cells.Columns, Cells.Rows];

            for (int x = 0; x < Cells.Columns; x++)
            {
                for (int y = 0; y < Cells.Rows; y++)
                {
                    //var currentCell = Cells[x, y];
                    var neighbors = GetNeighborColors(x, y);
                    var newCell = CalculateAverage(neighbors, Cells[x, y]);

                    temp[x, y] = newCell;

                    if (newCell != Cells[x, y]) IsComplete = false;
                }
            }

            Cells.CollectionUpdate(temp);
        }

        private Cell CalculateAverage(IList<Cell> neighbors, Cell cell)
        {
            if (! cell.IsAlive) return cell;

            var closestPair = new Tuple<Cell, Cell,double>(new Cell(), new Cell(), double.MaxValue);

            for (int i = 0; i < neighbors.Count; i++)
            {
                for (int j = 0; j < neighbors.Count; j++)
                {
                    if (i == j) continue;

                    var difference = Math.Abs(neighbors[i].Color.Hue - neighbors[j].Color.Hue);

                    if (difference < closestPair.Item3) closestPair = new Tuple<Cell, Cell, double>(neighbors[i],neighbors[j],difference);
                }
            }

            var hue = (closestPair.Item1.Color.Hue + closestPair.Item2.Color.Hue)/2;

            return new Cell(hue, cell.Color.Saturation, cell.Color.Value);
        }

        private IList<Cell> GetNeighborColors(int x, int y)
        {
            var result = new List<Cell>();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == 0 && j == 0) continue;

                    var column = CalculateIndex(x, i, Cells.Columns);
                    var row = CalculateIndex(y, j, Cells.Rows);

                    var cell = Cells[column, row];

                    if (cell.IsAlive) result.Add(cell);
                }
            }

            return result;
        }

        private int CalculateIndex(int currentIndex, int move, int upperBound)
        {
            _newIndex = currentIndex + move;

            if (_newIndex < 0) return upperBound;
            if (_newIndex >= upperBound) return 0;
            else return _newIndex;
        }
    }
}
