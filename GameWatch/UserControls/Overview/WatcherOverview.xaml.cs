using GameWatch.Models;
using GameWatch.Services;
using GameWatch.UserControls.Overview;
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

            PieChart.UpdateChart(watchedProcess.WatchModelGroup.Passed.TotalSeconds, watchedProcess.WatchModelGroup.Allowed.TotalSeconds);
        }

        private void UpdateData()
        {
            PassedTimeLabel.Content = Watcher.WatchModelGroup.Passed;
            PieChart.UpdateChart(Watcher.WatchModelGroup.Passed.TotalSeconds, Watcher.WatchModelGroup.Allowed.TotalSeconds);
            StatusLabel.Content = Watcher.WatchModelGroup.Status;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Watcher.StartWatch();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Watcher.PauseWatch();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Watcher.StopWatch();
        }
    }
}
