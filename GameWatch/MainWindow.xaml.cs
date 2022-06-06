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
            _context = new WindowContext(new List<WatchedProcessGroup>(), new List<IWatcherService>());
            SwitchView(new MainOverview(_context, this));
        }

        public void SwitchView(TrayWindowSwitchable toElement)
        {
            MainPanel.Children.Clear();
            MainPanel.Children.Add(toElement.Element);
            Width = toElement.TWidth;
            Height = toElement.THeight;
            this.Left = SystemParameters.PrimaryScreenWidth - this.Width - 10;
            this.Top = SystemParameters.PrimaryScreenHeight - this.Height - 45;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Directory.Exists(_savePath))
                Directory.CreateDirectory(_savePath);
            foreach (var watchedProcess in _context.Watched)
                IOHelper.SaveJsonObject($"{_savePath}/{watchedProcess.ID}.json", watchedProcess);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myNotifyIcon.Icon = new System.Drawing.Icon("powericon.ico");
            SetupContextMenu();
            Visibility = Visibility.Hidden;
            BlurHelper.EnableBlur(this);
            if (!Directory.Exists(_savePath))
                Directory.CreateDirectory(_savePath);
            _context.Watched.Clear();
            _context.Watched.AddRange(IOHelper.LoadJsonObjects<WatchedProcessGroup>(_savePath));
        }

        private void myNotifyIcon_TrayRightMouseDown(object sender, RoutedEventArgs e)
        {
            myNotifyIcon.ContextMenu.IsOpen = true;
        }

        private void myNotifyIcon_PopupOpened(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Visible;
            Activate();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Visibility = Visibility.Hidden;
        }

        private void SetupContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Header = "Exit";
            item.Click += ExitButton_Click;
            contextMenu.Items.Add(item);
            myNotifyIcon.ContextMenu = contextMenu;
            myNotifyIcon.ContextMenu.IsOpen = false;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
