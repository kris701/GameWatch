using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameWatch.Helpers
{
    internal static class FadeHelper
    {
        public static async Task FadeIn(UIElement element, double increments, int delay, double max = 1)
        {
            for (double i = element.Opacity; i < max; i += increments)
            {
                element.Opacity = i;
                await Task.Delay(delay);
            }
            element.Opacity = max;
        }

        public static async Task FadeOut(UIElement element, double increments, int delay, double min = 0)
        {
            for (double i = element.Opacity; i > min; i -= increments)
            {
                element.Opacity = i;
                await Task.Delay(delay);
            }
            element.Opacity = min;
        }
    }
}
