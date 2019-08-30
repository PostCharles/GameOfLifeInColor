using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string text)
        {
            return Regex.Replace(text, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }
    }
}
