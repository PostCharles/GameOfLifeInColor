using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeInColor.Core.Interfaces;
using GameOfLifeInColor.Infrastructure.Support;

namespace GameOfLifeInColor.WPF.Support
{
    class RandomizerFactory : FactoryBase, IRandomizerFactory
    {
        private const string RandomizerAppSettingKey = "SelectedRandomizer";
        private const string RuleSetAssemblyName = "GameOfLifeInColor.Infrastructure.Randomizers";

        private readonly IRandomizer _defaultRandomizer;

        public ICollection<Type> RandomizerList { get; set; }


        public RandomizerFactory(IRandomizer defaultRandomizer)
        {
            _defaultRandomizer = defaultRandomizer;

            RandomizerList = BuildImplementerList(RuleSetAssemblyName, typeof(IRandomizer));
        }


        public IRandomizer GetRandomizer()
        {
            var randomizerName = ConfigurationManager.AppSettings[RandomizerAppSettingKey];
            var randomizerType = RandomizerList.FirstOrDefault(r => r.Name == randomizerName);

            if (randomizerType != null) return GetRandomizer(randomizerType);
            else return _defaultRandomizer;
        }

        public IRandomizer GetRandomizer(Type type)
        {
            if (! RandomizerList.Contains(type)) return _defaultRandomizer;

            if (type.GetConstructor(Type.EmptyTypes) != null) return (IRandomizer)Activator.CreateInstance(type, Type.EmptyTypes);
            else return _defaultRandomizer;
        }
    }
}
