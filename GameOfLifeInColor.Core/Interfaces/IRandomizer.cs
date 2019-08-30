using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeInColor.Core.Models;

namespace GameOfLifeInColor.Core.Interfaces
{
    public interface IRandomizer : IOptions
    {
        CellCollection Cells { get; set; }
        Task Randomize();
    }
}
