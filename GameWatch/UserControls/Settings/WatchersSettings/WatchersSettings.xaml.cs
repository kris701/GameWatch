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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameWatch.UserControls.Settings.WatchersSettings
{
    /// <summary>
    /// Interaction logic for WatchersSettings.xaml
    /// </summary>
    public partial class WatchersSettings : UserControl, ValidatorControl
    {
        private WindowContext _context;
        public WatchersSettings(WindowContext context)
        {
            InitializeComponent();
            _context = context;
            ActiveWatchersPanel.Children.Clear();
            foreach (var item in _context.Watched)
                ActiveWatchersPanel.Children.Add(new ActiveWatcher(this, item));
        }

        private void AddNewWatcherButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveWatchersPanel.Children.Add(
            new ActiveWatcher(
                this, new WatchedProcessGroup(
                    Guid.NewGuid(),
                    new List<string>(),
                    "",
                    TimeSpan.Zero)));
        }

        public void RemoveWatcher(UIElement watcher)
        {
            ActiveWatchersPanel.Children.Remove(watcher);
        }

        public bool IsValid()
        {
            bool any = true;
            foreach (var child in ActiveWatchersPanel.Children)
            {
                if (child is ActiveWatcher watcher)
                {
                    if (any)
                        any = watcher.IsValid();
                    else
                        watcher.IsValid();
                }
            }
            return any;
        }

        public void AcceptChanges()
        {
            if (IsValid())
            {
                _context.Watched.Clear();
                foreach (var child in ActiveWatchersPanel.Children)
                {
                    if (child is ActiveWatcher watcher)
                    {
                        watcher.AcceptChanges();
                        _context.Watched.Add(watcher.WatchedProcess);
                    }
                }
            }
        }
    }
}
