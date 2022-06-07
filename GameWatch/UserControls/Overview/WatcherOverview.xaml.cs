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
            PassedTimeLabel.Content = watchedProcess.WatchModelGroup.Passed;
            AllowedTimeLabel.Content = watchedProcess.WatchModelGroup.Allowed;
            UsedPercent.Content = Math.Round(((double)watchedProcess.WatchModelGroup.Passed.TotalSeconds / (double)watchedProcess.WatchModelGroup.Allowed.TotalSeconds) * 100,0);
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = watchedProcess.WatchModelGroup.Allowed.TotalSeconds;
            ProgressBar.Value = watchedProcess.WatchModelGroup.Passed.TotalSeconds;
        }

        private void UpdateData()
        {
            PassedTimeLabel.Content = Watcher.WatchModelGroup.Passed;
            ProgressBar.Value = Watcher.WatchModelGroup.Passed.TotalSeconds;
            UsedPercent.Content = Math.Round(((double)Watcher.WatchModelGroup.Passed.TotalSeconds / (double)Watcher.WatchModelGroup.Allowed.TotalSeconds) * 100, 0);
            StatusLabel.Content = Watcher.WatchModelGroup.Status;
        }
    }
}
