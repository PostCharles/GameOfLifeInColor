using System;
using System.Collections.Generic;
using GameOfLifeInColor.Core.Models;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GameOfLifeInColor.Core.Interfaces
{
    public interface IRuleSet : IOptions, INotifyPropertyChanged
    {
        CellCollection Cells { get; set; }
        bool CancelationToken { get; set; }
        bool IsRunning { get; set; }
        Task<bool> Start();
    }
}
