using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeInColor.Core.Interfaces
{
    public interface IRandomizerFactory
    {
        ICollection<Type> RandomizerList { get; set; }
        IRandomizer GetRandomizer();
        IRandomizer GetRandomizer(Type type);
    }
}
