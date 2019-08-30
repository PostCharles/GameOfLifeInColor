using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeInColor.Core.Interfaces;
using System.Reflection;
using GameOfLifeInColor.Infrastructure.Support;

namespace GameOfLifeInColor.WPF.Support
{
    class RuleSetFactory : FactoryBase, IRuleSetFactory
    {
        private const string RuleSetAssemblyName = "GameOfLifeInColor.Infrastructure.RuleSets";
        private const string RuleSetAppSettingKey = "SelectedRuleSet";

        private readonly IRuleSet _defaultRuleSet;
        
        public ICollection<Type> RuleSetList{ get; set; }


        public RuleSetFactory(IRuleSet defaultRuleSet)
        {
            _defaultRuleSet = defaultRuleSet;

            RuleSetList = BuildImplementerList(RuleSetAssemblyName, typeof (IRuleSet));
        }


        public IRuleSet GetRuleSet()
        {
            var ruleSetName = ConfigurationManager.AppSettings[RuleSetAppSettingKey];
            var ruleType = RuleSetList.FirstOrDefault(r => r.Name == ruleSetName);

            if (ruleType != null) return GetRuleSet(ruleType);
            else return _defaultRuleSet;
        }

        public IRuleSet GetRuleSet(Type type)
        {
            if (!RuleSetList.Contains(type)) return _defaultRuleSet;

            if (RuleSetList.Contains(type)) return (IRuleSet) Activator.CreateInstance(type);
            else return _defaultRuleSet;
        }
    }
}