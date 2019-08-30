using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeInColor.Infrastructure.Support
{
    public abstract class FactoryBase
    {
        protected ICollection<Type> BuildImplementerList(string AssemblyName, Type searchType)
        {
            var result = new List<Type>();

            foreach (Type type in Assembly.Load(AssemblyName).GetTypes())
            {
                if (searchType.IsAssignableFrom(type) && type.IsClass) result.Add(type);
            }

            return result;
        }
    }
}
