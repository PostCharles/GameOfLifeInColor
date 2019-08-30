using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeInColor.Core.Interfaces
{
    public interface IOptionsConstructor
    {
        object Construct(IOptions implementer);
    }
}
