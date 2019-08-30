using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeInColor.Core.Interfaces;
using GameOfLifeInColor.Core.Models;
using GameOfLifeInColor.Core.Support;

namespace GameOfLifeInColor.WPF.Support
{
    class DefaultRuleSet : IRuleSet
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CellCollection Cells { get; set; }
        public ICollection<Option> Options { get; set; }
        public bool CancelationToken { get; set; }
        public bool IsRunning { get; set; }        

        public async Task<bool> Start()
        {
            await Task.Delay(1);
            return false;
        }
    }
}
