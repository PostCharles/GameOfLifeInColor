using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeInColor.Core.Enumerations;

namespace GameOfLifeInColor.Core.Interfaces
{
    public interface IContainerConstructor
    {
        object Construct(BuildOrientation orientation, params object[] Controls);
    }
}
