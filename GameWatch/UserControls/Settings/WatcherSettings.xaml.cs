using GameWatch.Helpers;
using GameWatch.Models;
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
using System.Windows.Shapes;

namespace GameWatch.UserControls
{
    /// <summary>
    /// Interaction logic for WatcherSettings.xaml
    /// </summary>
    public partial class WatcherSettings : UserControl, TrayWindowSwitchable
    {
        private WindowContext _context;
        private ITrayWindow _trayWindow;

        public UIElement Element { get; }
        public double TWidth { get; } = 400;
        public double THeight { get; } = 450;
        public WatcherSettings(WindowContext context, ITrayWindow trayWindow)
        {
            InitializeComponent();
            _context = context;
            _trayWindow = trayWindow;
            Element = this;
            UpdateList();
        }

        private void UpdateList()
        {
            ActiveWatchersPanel.Children.Clear();
            foreach (var item in _context.Watched)
                ActiveWatchersPanel.Children.Add(new ActiveWatcher(this, item));
        }

        private void AllowedTimeTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = InputHelper.IsOnlyNumbers(e.Text);
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if watchers are valid
            bool allFine = true;
            foreach (var child in ActiveWatchersPanel.Children)
            {
                if (child is ActiveWatcher watcher)
                {
                    allFine = watcher.IsValid;
                    if (!allFine)
                        break;
                }
            }
            if (allFine)
            {
                foreach (var item in _context.Watched)
                    item.PassedSeconds = 0;
                _trayWindow.SwitchView(new MainOverview(_context, _trayWindow));
            }
        }

        private void AddNewWatcherButton_Click(object sender, RoutedEventArgs e)
        {
            _context.Watched.Add(
            new WatchedProcessGroup(
                Guid.NewGuid(),
                new List<string>(),
                "",
                0));
            UpdateList();
        }

        public void RemoveWatcher(WatchedProcessGroup watcher)
        {
            _context.Watched.Remove(watcher);
            UpdateList();
        }

        public async Task BlinkElement(Panel element)
        {
            var normalBackground = element.Background;
            element.Background = Brushes.DarkRed;
            await Task.Delay(500);
            element.Background = normalBackground;
            await Task.Delay(500);
            element.Background = Brushes.DarkRed;
            await Task.Delay(500);
            element.Background = normalBackground;
            await Task.Delay(500);
        }
    }
}
