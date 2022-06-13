using GameWatch.Helpers;
using GameWatch.Models;
using GameWatch.Services;
using GameWatch.UserControls;
using GameWatch.UserControls.Overview;
using GameWatch.UserControls.Welcome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace GameWatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ITrayWindow
    {
        private bool _isActive = false;
        private WindowContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new WindowContext(new List<WatchedProcessGroup>(), new List<IWatcherService>(), new SettingsModel());
        }

        public async Task SwitchView(TrayWindowSwitchable toElement)
        {
            SaveContext();
            await FadeHelper.FadeOut(this, 0.1, 10);
            MainPanel.Children.Clear();
            MainPanel.Children.Add(toElement.Element);
            Width = toElement.TWidth;
            Height = toElement.THeight;
            this.Left = SystemParameters.PrimaryScreenWidth - this.Width - 10;
            this.Top = SystemParameters.PrimaryScreenHeight - this.Height - 45;
            await FadeHelper.FadeIn(this, 0.1, 10, 0.8);
        }

        public void SaveContext()
        {
            _context.SaveContext();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveContext();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NotifyIcon.Icon = new System.Drawing.Icon("gamewatchicon.ico");
            if (!Directory.Exists(WindowContext.SavePath))
            {
                _context.Settings.WindowFadeDelay = TimeSpan.FromSeconds(60);
                await SwitchView(new WelcomeView(_context, this));
            }
            else
            {
                _context.LoadContext();
                Visibility = Visibility.Hidden;
                BlurHelper.EnableBlur(this);
                await SwitchView(new MainWatcherView(_context, this));
            }
        }

        private void NotifyIcon_TrayRightMouseDown(object sender, RoutedEventArgs e)
        {
            NotifyIcon.ContextMenu.IsOpen = true;
        }

        private async void NotifyIcon_PopupOpened(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Visible;
            await FadeHelper.FadeIn(this, 0.02, 10, 0.8);
            Activate();
        }

        private async void Window_Deactivated(object sender, EventArgs e)
        {
            _isActive = false;
            for(int i = 0; i < _context.Settings.WindowFadeDelay.TotalMilliseconds; i += 100)
            {
                await Task.Delay(100);
                if (_isActive)
                    break;
            }
            if (!_isActive)
            {
                await FadeHelper.FadeOut(this, 0.02, 10);
                Visibility = Visibility.Hidden;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            _isActive = true;
        }
    }
}
