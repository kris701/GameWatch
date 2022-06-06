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
using System.Windows.Threading;

namespace GameWatch.UserControls
{
    /// <summary>
    /// Interaction logic for WatcherLabel.xaml
    /// </summary>
    public partial class WatcherLabel : UserControl
    {
        private WatchedProcess _wP;

        public WatcherLabel(WatchedProcess wP)
        {
            InitializeComponent();
            _wP = wP;
        }

        private void Ticker(object? sender, EventArgs e)
        {
            UpdateLabel.Content = $"Name: {_wP.UIName}, Seconds passed: {_wP.PassedSeconds}";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Ticker;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }
    }
}
