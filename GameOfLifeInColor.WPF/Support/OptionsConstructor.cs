using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GameOfLifeInColor.Core.Attributes;
using GameOfLifeInColor.Core.Interfaces;
using GameOfLifeInColor.Core.Models;
using GameOfLifeInColor.Core.Support;
using GameOfLifeInColor.WPF.Support.ExtensionMethods;
using GameOfLifeInColor.WPF.Support.Models;
using GameOfLifeInColor.Core.Enumerations;

namespace GameOfLifeInColor.WPF.Support
{
    class OptionsConstructor : IOptionsConstructor
    {
        private readonly IContainerConstructor _constructor;

        public OptionsConstructor(IContainerConstructor constructor)
        {
            _constructor = constructor;
        }

        public object Construct(IOptions implementer)
        {
            var container = new StackPanel();
            container.Children.Add( BuildDescriptionControl(implementer) );

            if (implementer.Options != null && implementer.Options.Count > 0)
            {
                foreach (var option in implementer.Options)
                {
                    var uiOption = DownCastOptionForUI(option);
                    uiOption.Constructor = _constructor;

                    container.Children.Add((UIElement) uiOption.Construct());
                }
            }

            return container;
        }

        private UIElement BuildDescriptionControl(IOptions implementer)
        {
            var info = implementer.GetType();
            var attribute = info.GetCustomAttributes(typeof (StrategyDescriptionAttribute))
                                .SingleOrDefault() as StrategyDescriptionAttribute;

            if (attribute != null) return BuildDescriptionControl(attribute.Name, attribute.Description);
            return GetDefaultDescriptionControl();
        }
        
        private UIElement BuildDescriptionControl(string name, string description)
        {
            var descriptionLabel = new Label().BuildStyledLabel(description);

            var nameLabel = new Label
                            {
                                Content = name,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                HorizontalContentAlignment = HorizontalAlignment.Center,
                                Target = descriptionLabel
                            };

            return _constructor.Construct(BuildOrientation.Vertical, nameLabel, descriptionLabel) as UIElement;
        }

        private UIElement GetDefaultDescriptionControl()
        {
            return BuildDescriptionControl("No Description Provided", "");
        }


        private Option DownCastOptionForUI(Option option)
        {
            var optionType = option.GetType();
            var baseType = typeof(Option);
            var derivedTypes = baseType.Assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
            var wpfDerivedTypes = Assembly.GetCallingAssembly().GetTypes().Where(t => t.IsSubclassOf(baseType));

            foreach (var derived in derivedTypes)
            {
                if (!optionType.IsAssignableFrom(derived)) continue;

                foreach (var wpf in wpfDerivedTypes)
                {
                    if (wpf.BaseType == derived) return (Option)Activator.CreateInstance(wpf, option);
                }
            }

            return null;
        }
    }
}
