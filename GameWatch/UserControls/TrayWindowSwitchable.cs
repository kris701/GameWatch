using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameWatch.UserControls
{
    public interface TrayWindowSwitchable
    {
        public UIElement Element { get; }
        public double TWidth { get; }
        public double THeight { get; }
    }
}
