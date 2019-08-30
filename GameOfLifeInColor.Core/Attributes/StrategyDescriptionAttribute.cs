using System;

namespace GameOfLifeInColor.Core.Attributes
{
    public class StrategyDescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public string Name { get; set; }
    }
}
