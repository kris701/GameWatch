using GameWatch.Helpers;
using GameWatch.Models;
using GameWatch.UserControls.Overview;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            UpdateWatcherList();
            SetGeneralSettings();
        }

        private void UpdateWatcherList()
        {
            ActiveWatchersPanel.Children.Clear();
            foreach (var item in _context.Watched)
                ActiveWatchersPanel.Children.Add(new ActiveWatcher(this, item));
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
                if (_context.Settings.ResetWatchersWhenClosingSettings)
                    foreach (var item in _context.Watched)
                        item.Passed = TimeSpan.Zero;
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
                TimeSpan.Zero));
            UpdateWatcherList();
        }

        public void RemoveWatcher(WatchedProcessGroup watcher)
        {
            _context.Watched.Remove(watcher);
            UpdateWatcherList();
        }

        private void RunAtStartupCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox check && check.IsChecked != null)
            {
                _context.Settings.RunAtStartup = (bool)check.IsChecked;

                var module = System.Diagnostics.Process.GetCurrentProcess().MainModule;
                if (module == null || module.FileName == null)
                    throw new Exception("Could not find current running assembly!");

                if (_context.Settings.RunAtStartup)
                    IOHelper.GenerateShortcut(
                        $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\",
                        "GameWatch",
                        module.FileName);
                else
                    IOHelper.RemoveShortcut(
                        $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\",
                        "GameWatch",
                        module.FileName);
            }
        }

        private void ResetWatchersCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox check && check.IsChecked != null)
                _context.Settings.ResetWatchersWhenClosingSettings = (bool)check.IsChecked;
        }

        private void SetGeneralSettings()
        {
            RunAtStartupCheckbox.IsChecked = _context.Settings.RunAtStartup;
            ResetWatchersCheckbox.IsChecked = _context.Settings.ResetWatchersWhenClosingSettings;
        }
    }
}
