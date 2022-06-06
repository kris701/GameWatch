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

namespace GameWatch.UserControls.Overview
{
    /// <summary>
    /// Interaction logic for MainOverview.xaml
    /// </summary>
    public partial class MainOverview : UserControl, TrayWindowSwitchable
    {
        private WindowContext _context;
        private ITrayWindow _trayWindow;

        public UIElement Element { get; }
        public double TWidth { get; } = 800;
        public double THeight { get; } = 450;
        public MainOverview(WindowContext context, ITrayWindow trayWindow)
        {
            InitializeComponent();
            _context = context;
            _trayWindow = trayWindow;
            Element = this;
            InitializeComponent();
            _context.Watchers.Clear();
            foreach (var item in _context.Watched)
                _context.Watchers.Add(new WatcherService(item));
            foreach (var watcher in _context.Watchers)
                WatchersPanel.Children.Add(new WatcherOverview(watcher));
            if (WatchersPanel.Children.Count == 0)
                WatchersPanel.Children.Add(new AddNewWatchersLabelControl());
            ToggleWatchers(true);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleWatchers(false);
            _trayWindow.SwitchView(new WatcherSettings(_context, _trayWindow));
        }

        private void StartAllButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleWatchers(true);
        }

        private void StopAllButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleWatchers(false);
        }

        private void ToggleWatchers(bool doRun)
        {
            foreach (var watcher in _context.Watchers)
                if (doRun)
                    watcher.StartWatch();
                else
                    watcher.StopWatch();
        }
    }
}
