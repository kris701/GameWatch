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
        public static async Task FadeIn(UIElement element, double increments, int delay)
        {
            for (double i = element.Opacity; i < 1; i += increments)
            {
                element.Opacity = i;
                await Task.Delay(delay);
            }
            element.Opacity = 1;
        }

        public static async Task FadeOut(UIElement element, double increments, int delay)
        {
            for (double i = element.Opacity; i > 0; i -= increments)
            {
                element.Opacity = i;
                await Task.Delay(delay);
            }
            element.Opacity = 0;
        }
    }
}
