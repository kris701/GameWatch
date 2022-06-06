using GameWatch.Helpers;
using GameWatch.Models;
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
    public partial class WatcherSettings : Window
    {
        private List<WatchedProcessGroup> _watched;
        public WatcherSettings(List<WatchedProcessGroup> watched)
        {
            InitializeComponent();
            _watched = watched;
            UpdateList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _watched.Add(
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
            foreach (var item in _watched)
                ActiveWatchersPanel.Children.Add(new ActiveWatcher(item));
        }

        private void AllowedTimeTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = InputHelper.IsOnlyNumbers(e.Text);
        }
    }
}
