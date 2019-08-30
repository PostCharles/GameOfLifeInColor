using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeInColor.Core.Attributes;
using GameOfLifeInColor.Core.Interfaces;
using GameOfLifeInColor.Core.Models;
using GameOfLifeInColor.Core.Support;

namespace GameOfLifeInColor.Infrastructure.Randomizers
{
    [StrategyDescription(Name="True Randomizer", Description = RandomizerDescription)]
    class TrueRandomizer : IRandomizer
    {

        public CellCollection Cells { get; set; }
        public ICollection<Option> Options { get; set; }

        public TrueRandomizer()
        {
            Options = new List<Option>();
        }

        public async Task Randomize()
        {
            if (Cells == null) return;

            await Task.Run(() =>
            {
                var random = new Random();

                for (int x = 0; x < Cells.Columns; x++)
                {
                    for (int y = 0; y < Cells.Rows; y++)
                    {
                        Cells[x, y] = new Cell(random.Next(0, 360), 1D, 1D);
                    }
                }
            });
        }

        private const string RandomizerDescription = "Each cell is filled with a unique random color";
    }
}
