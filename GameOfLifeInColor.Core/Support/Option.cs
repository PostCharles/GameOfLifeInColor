using System;
using GameOfLifeInColor.Core.Interfaces;

namespace GameOfLifeInColor.Core.Support
{
    public abstract class Option
    {
        public string PropertyName { get; set; }
        public object Owner { get; set; }
        public Type PropertyType { get; set; }
        public IContainerConstructor Constructor { get; set; }

        public virtual object Construct()
        {
            return new object();
        }
    }
}
