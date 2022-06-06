using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameWatch.Helpers
{
    internal static class InputHelper
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        public static bool IsOnlyNumbers(string text)
        {
            return _regex.IsMatch(text);
        }
    }
}
