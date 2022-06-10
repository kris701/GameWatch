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

namespace GameWatch.UserControls.Notification
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        private WatchedProcessGroup _watchModelGroup;
        public NotificationAction Action { get; internal set; }
        public NotificationWindow(WatchedProcessGroup watchModelGroup)
        {
            Opacity = 0;
            InitializeComponent();
            _watchModelGroup = watchModelGroup;
            AllowedTimeLabel.Content = $"Allowed time for {_watchModelGroup.UIName} exceeded";
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BlurHelper.EnableBlur(this);
            await FadeHelper.FadeIn(this, 0.05, 10);
        }

        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            Action = NotificationAction.StopTimer;
            await CloseWindow();
        }

        private async void AcceptDelayButton_Click(object sender, RoutedEventArgs e)
        {
            Action = NotificationAction.Add15Min;
            await CloseWindow();
        }

        private async Task CloseWindow()
        {
            await FadeHelper.FadeOut(this, 0.05, 10);
            Close();
        }
    }
}
