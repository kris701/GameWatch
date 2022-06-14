using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameWatch.Helpers
{
    public static class InputHelper
    {
        public static bool IsTextboxValid(TextBox element, bool check, Brush defaultBackground)
        {
            if (check)
            {
                element.Background = Brushes.DarkRed;
                return false;
            }
            else
                element.Background = defaultBackground;
            return true;
        }

        public static bool IsComboboxValid(ComboBox element, bool check, Brush defaultForeground)
        {
            if (check)
            {
                element.Foreground = Brushes.DarkRed;
                return false;
            }
            else
                element.Foreground = defaultForeground;
            return true;
        }
    }
}
