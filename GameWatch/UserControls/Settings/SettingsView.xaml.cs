using GameWatch.Helpers;
using GameWatch.Models;
using GameWatch.UserControls.Overview;
using GameWatch.UserControls.Settings;
using GameWatch.UserControls.Settings.WatchersSettings;
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
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl, TrayWindowSwitchable
    {
        private GeneralSettings _generalSettings;
        private WatchersSettings _watchersSettings;

        private WindowContext _context;
        private ITrayWindow _trayWindow;

        public UIElement Element { get; }
        public double TWidth { get; } = 400;
        public double THeight { get; } = 450;
        public SettingsView(WindowContext context, ITrayWindow trayWindow)
        {
            InitializeComponent();
            _context = context;
            _trayWindow = trayWindow;
            Element = this;

            _generalSettings = new GeneralSettings(context, this);
            _watchersSettings = new WatchersSettings(context);

            GeneralSettingsExpander.Content = _generalSettings;
            WatchersSettingsExpander.Content = _watchersSettings;
        }

        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            if (isValid)
                isValid = _generalSettings.IsValid();
            else
                _generalSettings.IsValid();
            if (isValid)
                isValid = _watchersSettings.IsValid();
            else
                _watchersSettings.IsValid();

            if (isValid)
            {
                _generalSettings.AcceptChanges();
                _watchersSettings.AcceptChanges();

                if (_context.Settings.ResetWatchersWhenClosingSettings)
                    foreach (var item in _context.Watched)
                        item.Passed = TimeSpan.Zero;

                await SwitchView();
            }
            else
            {
                GeneralSettingsExpander.IsExpanded = true;
                WatchersSettingsExpander.IsExpanded = true;
            }
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            await SwitchView();
        }

        public async Task SwitchView()
        {
            await _trayWindow.SwitchView(new MainWatcherView(_context, _trayWindow));
        }

        public async Task ResetView()
        {
            await _trayWindow.SwitchView(new SettingsView(_context, _trayWindow));
        }
    }
}
