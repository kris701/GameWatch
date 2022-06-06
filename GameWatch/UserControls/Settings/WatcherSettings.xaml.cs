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
        public double TWidth { get; } = 250;
        public double THeight { get; } = 450;
        public WatcherSettings(WindowContext context, ITrayWindow trayWindow)
        {
            InitializeComponent();
            _context = context;
            _trayWindow = trayWindow;
            Element = this;
            UpdateList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _context.Watched.Add(
                new WatchedProcessGroup(
                    Guid.NewGuid(),
                    ProcessNameTextbox.Text.Split(",").ToList(),
                    UINameTextbox.Text,
                    Int32.Parse(AllowedTimeTextbox.Text)));
            UpdateList();
        }

        private void UpdateList()
        {
            ActiveWatchersPanel.Children.Clear();
            foreach (var item in _context.Watched)
                ActiveWatchersPanel.Children.Add(new ActiveWatcher(item));
        }

        private void AllowedTimeTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = InputHelper.IsOnlyNumbers(e.Text);
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            _trayWindow.SwitchView(new MainOverview(_context, _trayWindow));
        }
    }
}
