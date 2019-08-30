using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeInColor.Core.Attributes;
using GameOfLifeInColor.Core.Interfaces;
using GameOfLifeInColor.Core.Models;
using GameOfLifeInColor.Core.Support;

namespace GameOfLifeInColor.Infrastructure.Randomizers
{
    [StrategyDescription(Name = "Circle Randomizer", Description = RandomizerDescription)]
    public class CircleRandomizer : IRandomizer, INotifyPropertyChanged
    {
        public CellCollection Cells { get; set; }

        public ICollection<Option> Options { get; set; }

        private int initialhue;

        public int InitialHue
        {
            get { return initialhue; }
            set
            {
                initialhue = value;
                if (SelectedDouble != value && Doubles.Contains(initialhue)) SelectedDouble = value;
                PropertyChanged(this, new PropertyChangedEventArgs("InitialHue"));
            }
        }


        private List<double> doubles;

        public List<double> Doubles
        {
            get { return doubles; }
            set { doubles = value; }
        }

        private double valee;
        public double SelectedDouble { get { return valee; } 
            set { valee = value;
                InitialHue = Convert.ToInt32(value);
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedDouble"));
        } }



        public CircleRandomizer()
        {
            Doubles = new List<double>() {1,3, 5, 7, 9, 6, 5, 43, 5};
            Options = new List<Option>();
            Options.Add(new SliderOption("InitialHue", InitialHue.GetType(), this, 0D, 360D));
            Options.Add(new CheckBoxOption("InitialHue", InitialHue.GetType(), this));
            Options.Add(new ComboBoxOption("Doubles", InitialHue.GetType(), this,"SelectedDouble",""));
            Options.Add(new TextBlockOption("InitialHue", InitialHue.GetType(), this));
            Options.Add(new TextBoxOption("InitialHue", InitialHue.GetType(), this));
        }


        public async Task Randomize()
        {
            if (Cells == null) return;

            await Task.Run(() => FillCollection());

        }

        public void FillCollection()
        {
            var cellCount = Cells.Rows * Cells.Columns;
            var increment = 360D / cellCount;
            var hueStep = (double)InitialHue;

            for (int x = 0; x < Cells.Columns; x++)
            {
                for (int y = 0; y < Cells.Rows; y++)
                {
                    Cells[x, y] = new Cell(hueStep, 1D, 1D);
                    hueStep += increment;
                }
            }
        }

        private const string RandomizerDescription = "Divides color spectrum between the number of cells";

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
