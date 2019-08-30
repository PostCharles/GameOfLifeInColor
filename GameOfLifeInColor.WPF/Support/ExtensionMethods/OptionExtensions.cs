using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GameOfLifeInColor.Core.Support;

namespace GameOfLifeInColor.WPF.Support.ExtensionMethods
{
    internal static class OptionExtensions
    {
    //    //This construct is complete overkill to avoid a switch statement, but I'm rather proud of it.

    //    public static FrameworkElement Construct(this Option option)
    //    {
    //        var baseType = typeof(Option);
    //        var derivedTypes = baseType.Assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));

    //        foreach (var derived in derivedTypes)
    //        {
    //            if (!option.GetType().IsAssignableFrom(derived)) continue;

    //            var method = typeof(Constructor).GetMethod("ConstructByDerivedType").MakeGenericMethod(derived);
    //            var castedOption = (Option)method.Invoke(null, new object[] { option });
                
    //        }
    //        return null;
    //    }

    //    private class Constructor
    //    {
    //        public static FrameworkElement ConstructByDerivedType<T>(Option option) where T : Option
    //        {
    //            var castedOption = ((T)option);

    //            var method = typeof(Constructor).GetMethod("Construct",  
    //                                                        BindingFlags.NonPublic | BindingFlags.Static,  
    //                                                        null,  new Type[] { typeof(T) },  null);
    //            return (FrameworkElement)method.Invoke(null, new object[] { castedOption });

    //        }
            
    //        private static FrameworkElement Construct(Option option)
    //        {
    //            return new FrameworkElement();
    //        }

    //        private static FrameworkElement Construct(TextBoxOption option)
    //        {
    //            return new FrameworkElement();
    //        }

    //        private  static FrameworkElement Construct(CheckBoxOption option)
    //        {
    //            return new FrameworkElement();
    //        }
    //    }
    }
}
