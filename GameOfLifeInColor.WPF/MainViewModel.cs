using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GameOfLifeInColor.Core.Models;
using GameOfLifeInColor.Core.Interfaces;

namespace GameOfLifeInColor.WPF
{
    public class MainViewModel: ViewModelBase
    {
        private readonly IOptionsConstructor _optionsConstructor;
        private readonly IRandomizerFactory _randomizerFactory;
        private readonly IRuleSetFactory _ruleSetFactory;
        
        private IRandomizer _randomizer;
        private IRuleSet _ruleSet;
        
        private object _randomizerOptions;
        private object _ruleSetOptions;
        
        public CellCollection Cells { get; set; }
        public bool IsRunning { get { return _ruleSet.IsRunning; }}

        public ICollection<Type> RandomizerList { get { return _randomizerFactory.RandomizerList; } }
        public ICollection<Type> RuleSetList { get { return _ruleSetFactory.RuleSetList; } }
        
        public Type SelectedRandomizer
        {
            get { return _randomizer.GetType(); }
            set
            {
                _randomizer = _randomizerFactory.GetRandomizer(value);
                RandomizerOptions = _optionsConstructor.Construct(_randomizer);
            }
        }

        public Type SelectedRuleSet
        {
            get { return _ruleSet.GetType(); }
            set
            {
                _ruleSet = _ruleSetFactory.GetRuleSet(value);
                _ruleSet.PropertyChanged += NotifyRuleSetPropertyChanged;
                    
                RuleSetOptions = _optionsConstructor.Construct(_ruleSet);
            }
        }

        public object RandomizerOptions
        {
            get { return _randomizerOptions; }
            set
            {
                _randomizerOptions = value; 
                RaisePropertyChanged(() => RandomizerOptions);
            }
        }

        public object RuleSetOptions
        {
            get { return _ruleSetOptions; }
            set
            {
                _ruleSetOptions = value; 
                RaisePropertyChanged(() => RuleSetOptions);
            }
        }
        
        
        
        public RelayCommand StartSimulationCommand
        {
            get { return new RelayCommand(async 
                    () => {
                          _ruleSet.Cells = Cells;
                          await _ruleSet.Start(); }
                ); }
        }

        public RelayCommand StopSimulationCommand
        {
            get { return new RelayCommand(
                    () => _ruleSet.CancelationToken = true
                ); }
        }

        public RelayCommand RandomizeCommand
        {
            get { return new RelayCommand(async () =>
                {
                    _randomizer.Cells = Cells;
                    
                    await _randomizer.Randomize();
                    
                    Cells.OnCollectionUpdated();
                });
            }
        }

        
        
        public MainViewModel(IRuleSetFactory ruleSetFactory, IRandomizerFactory randomizerFactory, IOptionsConstructor optionsConstructor)
        {
            Cells = new CellCollection(0,0);

            _ruleSetFactory = ruleSetFactory;
            _ruleSet = _ruleSetFactory.GetRuleSet();
            _ruleSet.PropertyChanged += NotifyRuleSetPropertyChanged;

            _randomizerFactory = randomizerFactory;
            _randomizer = _randomizerFactory.GetRandomizer();

            _optionsConstructor = optionsConstructor;
            _randomizerOptions = _optionsConstructor.Construct(_randomizer);
            _ruleSetOptions = _optionsConstructor.Construct(_ruleSet);
        }
        
        private void NotifyRuleSetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == (GetPropertyName( () => IsRunning )))  
            {RaisePropertyChanged(() => IsRunning);}
        }
    }
}
