using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeInColor.Core.Interfaces
{
    public interface IRuleSetFactory
    {
        ICollection<Type> RuleSetList { get; set; }
        IRuleSet GetRuleSet();
        IRuleSet GetRuleSet(Type type);
    }
}
