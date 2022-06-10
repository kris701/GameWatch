using GameWatch.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameWatch
{
    public interface ITrayWindow
    {
        public void SwitchView(TrayWindowSwitchable toElement);
        public void SaveContext();
    }
}
