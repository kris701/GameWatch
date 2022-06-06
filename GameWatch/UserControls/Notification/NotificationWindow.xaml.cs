using GameWatch.Helpers;
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
        public NotificationWindow(string name)
        {
            Opacity = 0;
            InitializeComponent();
            AllowedTimeLabel.Content = $"Allowed time for {name} exceeded";
            Top = 0;
            Left = 0;
            Height = SystemParameters.PrimaryScreenHeight; 
            Width = SystemParameters.PrimaryScreenWidth;
        }

        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            await FadeHelper.FadeOut(this, 0.05, 10);
            Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BlurHelper.EnableBlur(this);
            await FadeHelper.FadeIn(this, 0.05, 10);
        }
    }
}
