using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeInColor.Core.Interfaces;
using GameOfLifeInColor.Core.Models;
using GameOfLifeInColor.Core.Support;

namespace GameOfLifeInColor.WPF.Support
{
    class DefaultRandomizer : IRandomizer
    {
        public CellCollection Cells { get; set; }
        public ICollection<Option> Options { get; set; }

        public async Task Randomize()
        {
            await Task.Delay(1);
        }
    }
}
