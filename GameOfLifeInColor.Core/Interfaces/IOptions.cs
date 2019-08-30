using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeInColor.Core.Models;
using GameOfLifeInColor.Core.Support;

namespace GameOfLifeInColor.Core.Interfaces
{
    public interface IOptions
    {
        ICollection<Option> Options { get; set; }
    }
}
