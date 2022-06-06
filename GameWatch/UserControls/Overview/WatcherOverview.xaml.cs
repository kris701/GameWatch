using GameWatch.Models;
using GameWatch.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameWatch.UserControls
{
    /// <summary>
    /// Interaction logic for WatcherOverview.xaml
    /// </summary>
    public partial class WatcherOverview : UserControl
    {
        public IWatcherService Watcher { get; set; }

        public WatcherOverview(IWatcherService watchedProcess)
        {
            Watcher = watchedProcess;
            Watcher.WatchModelGroup.Ticked += UpdateData;
            InitializeComponent();
        }

        private void UpdateData()
        {
            PassedTimeLabel.Content = Watcher.WatchModelGroup.PassedSeconds;
            UINameLabel.Content = $"Name [{Watcher.Status}]";
        }
    }
}
