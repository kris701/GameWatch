using GameWatch.Helpers;
using GameWatch.Models;
using GameWatch.Services;
using GameWatch.UserControls;
using GameWatch.UserControls.Overview;
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
        private string _savePath = "saves/";
        private WindowContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new WindowContext(new List<WatchedProcessGroup>(), new List<IWatcherService>(), new SettingsModel());
        }

        public void SwitchView(TrayWindowSwitchable toElement)
        {
            _context.SaveContext(_savePath);
            MainPanel.Children.Clear();
            MainPanel.Children.Add(toElement.Element);
            Width = toElement.TWidth;
            Height = toElement.THeight;
            this.Left = SystemParameters.PrimaryScreenWidth - this.Width - 10;
            this.Top = SystemParameters.PrimaryScreenHeight - this.Height - 45;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _context.SaveContext(_savePath);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NotifyIcon.Icon = new System.Drawing.Icon("gamewatchicon.ico");
            SetupContextMenu();
            _context.LoadContext(_savePath);
            Visibility = Visibility.Hidden;
            BlurHelper.EnableBlur(this);
            SwitchView(new MainOverview(_context, this));
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
            await FadeHelper.FadeOut(this, 0.02, 10);
            Visibility = Visibility.Hidden;
        }

        private void SetupContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Header = "Exit";
            item.Click += ExitButton_Click;
            contextMenu.Items.Add(item);
            NotifyIcon.ContextMenu = contextMenu;
            NotifyIcon.ContextMenu.IsOpen = false;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
