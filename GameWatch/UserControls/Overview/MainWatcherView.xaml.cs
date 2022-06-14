using GameWatch.Models;
using GameWatch.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace GameWatch.UserControls.Overview
{
    /// <summary>
    /// Interaction logic for MainWatcherView.xaml
    /// </summary>
    public partial class MainWatcherView : UserControl, TrayWindowSwitchable
    {
        private WindowContext _context;
        private ITrayWindow _trayWindow;

        public UIElement Element { get; }
        public double TWidth { get; } = 800;
        public double THeight { get; } = 450;
        public MainWatcherView(WindowContext context, ITrayWindow trayWindow)
        {
            InitializeComponent();
            _context = context;
            _trayWindow = trayWindow;
            Element = this;
            InitializeComponent();
            _context.Watchers.Clear();
            foreach (var item in _context.Watched)
                _context.Watchers.Add(new WatcherService(item, context.Settings));
            foreach (var watcher in _context.Watchers)
                WatchersPanel.Children.Add(new WatcherOverview(watcher));
            if (WatchersPanel.Children.Count == 0)
                WatchersPanel.Children.Add(new AddNewWatchersLabelControl());
            StartAllWatchers();
        }

        private async void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            PauseAllWatchers();
            _trayWindow.SaveContext();
            await _trayWindow.SwitchView(new SettingsView(_context, _trayWindow));
        }

        private void StartAllButton_Click(object sender, RoutedEventArgs e)
        {
            _trayWindow.SaveContext();
            StartAllWatchers();
        }

        private void PauseAllButton_Click(object sender, RoutedEventArgs e)
        {
            _trayWindow.SaveContext();
            PauseAllWatchers();
        }

        private void StopAllButton_Click(object sender, RoutedEventArgs e)
        {
            _trayWindow.SaveContext();
            StopAllWatchers();
        }

        private void StartAllWatchers()
        {
            foreach (var watcher in _context.Watchers)
                watcher.StartWatch();
        }

        private void StopAllWatchers()
        {
            foreach (var watcher in _context.Watchers)
                watcher.StopWatch();
        }

        private void PauseAllWatchers()
        {
            foreach (var watcher in _context.Watchers)
                watcher.PauseWatch();
        }

        private void GithubLinkButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/kris701/GameWatch",
                UseShellExecute = true
            });
        }
    }
}
