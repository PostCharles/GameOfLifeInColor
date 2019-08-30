using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using GameOfLifeInColor.Core.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace GameOfLifeInColor.WPF.Support
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            RegisterTypes();
        }

        public MainViewModel Main
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MainViewModel>();
            }
        }

        private void RegisterTypes()
        {
            var ioc = SimpleIoc.Default;
            
            ioc.Register<DefaultRandomizer>();
            if (!ioc.IsRegistered<IRandomizer>()) ioc.Register<IRandomizer>(() => ioc.GetInstance<DefaultRandomizer>());
            
            ioc.Register<DefaultRuleSet>();
            if (!ioc.IsRegistered<IRuleSet>()) ioc.Register<IRuleSet>(() => ioc.GetInstance<DefaultRuleSet>());
            
            ioc.Register<RandomizerFactory>();
            if (!ioc.IsRegistered<IRandomizerFactory>()) ioc.Register<IRandomizerFactory>(() => ioc.GetInstance<RandomizerFactory>());
            
            ioc.Register<RuleSetFactory>();
            if (!ioc.IsRegistered<IRuleSetFactory>()) ioc.Register<IRuleSetFactory>(() => ioc.GetInstance<RuleSetFactory>());

            ioc.Register<ContainerConstructor>();
            if (!ioc.IsRegistered<IContainerConstructor>()) ioc.Register<IContainerConstructor>(() => ioc.GetInstance<ContainerConstructor>());

            ioc.Register<OptionsConstructor>();
            if (!ioc.IsRegistered<IOptionsConstructor>()) ioc.Register<IOptionsConstructor>(() => ioc.GetInstance<OptionsConstructor>());
            
            ioc.Register<MainViewModel>();;
            
            
        }
    }
}
