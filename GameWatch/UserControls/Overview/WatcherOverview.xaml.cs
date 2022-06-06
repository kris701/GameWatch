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
            NameLabel.Content = watchedProcess.WatchModelGroup.UIName;
            StatusLabel.Content = watchedProcess.WatchModelGroup.Status;
            PassedTimeLabel.Content = watchedProcess.WatchModelGroup.PassedSeconds;
            AllowedTimeLabel.Content = watchedProcess.WatchModelGroup.AllowedIntervalSec;
            UsedPercent.Content = Math.Round(((double)watchedProcess.WatchModelGroup.PassedSeconds / (double)watchedProcess.WatchModelGroup.AllowedIntervalSec) * 100,0);
        }

        private void UpdateData()
        {
            PassedTimeLabel.Content = Watcher.WatchModelGroup.PassedSeconds;
            UsedPercent.Content = Math.Round(((double)Watcher.WatchModelGroup.PassedSeconds / (double)Watcher.WatchModelGroup.AllowedIntervalSec) * 100, 0);
            StatusLabel.Content = Watcher.WatchModelGroup.Status;
        }
    }
}
