using GameWatch.Helpers;
using GameWatch.Models;
using GameWatch.Services;
using GameWatch.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;

namespace GameWatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _savePath = "saves/";
        private List<WatchedProcessGroup> _watched;
        private List<IWatcherService> _watchers;

        public MainWindow()
        {
            InitializeComponent();
            _watched = new List<WatchedProcessGroup>();
            _watchers = new List<IWatcherService>();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new WatcherSettings(_watched);
            ToggleWatchers(false);
            WatchersPanel.Children.Clear();
            window.ShowDialog();
            GenerateAllWatchers();
        }

        private void StartAllButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleWatchers(true);
        }

        private void StopAllButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleWatchers(false);
        }

        private void GenerateAllWatchers()
        {
            WatchersPanel.Children.Clear();
            _watchers.Clear();
            foreach (var item in _watched)
            {
                var newWatcher = new WatcherService(item);
                _watchers.Add(newWatcher);
                WatchersPanel.Children.Add(new WatcherOverview(newWatcher));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Directory.Exists(_savePath))
                Directory.CreateDirectory(_savePath);
            foreach (var watchedProcess in _watched)
                IOHelper.SaveJsonObject($"{_savePath}/{watchedProcess.ID}.json", watchedProcess);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(_savePath))
                Directory.CreateDirectory(_savePath);
            _watched = IOHelper.LoadJsonObjects<WatchedProcessGroup>(_savePath);
            GenerateAllWatchers();
            ToggleWatchers(true);
        }

        private void ToggleWatchers(bool doRun)
        {
            foreach (var watcher in _watchers)
                if (doRun)
                    watcher.StartWatch();
                else
                    watcher.StopWatch();
        }
    }
}
