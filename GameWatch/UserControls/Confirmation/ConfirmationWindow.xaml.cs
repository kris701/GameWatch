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

namespace GameWatch.UserControls.Confirmation
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        public ConfirmationAction Action { get; internal set; }
        public ConfirmationWindow(string text)
        {
            Opacity = 0;
            InitializeComponent();
            TitleLabel.Content = text;
        }

        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            Action = ConfirmationAction.Ok;
            await CloseWindow();
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Action = ConfirmationAction.Cancel;
            await CloseWindow();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BlurHelper.EnableBlur(this);
            await FadeHelper.FadeIn(this, 0.05, 10);
        }

        private async Task CloseWindow()
        {
            await FadeHelper.FadeOut(this, 0.05, 10);
            Close();
        }
    }
}
