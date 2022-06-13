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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameWatch.UserControls.Welcome
{
    /// <summary>
    /// Interaction logic for WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView : UserControl, TrayWindowSwitchable
    {
        private WindowContext _context;
        private ITrayWindow _trayWindow;


        public UIElement Element { get; }
        public double TWidth { get; } = 800;
        public double THeight { get; } = 450;

        public WelcomeView(WindowContext context, ITrayWindow trayWindow)
        {
            InitializeComponent();
            _context = context;
            _trayWindow = trayWindow;
            Element = this;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _context.Settings.WindowFadeDelay = TimeSpan.Zero;
            await _trayWindow.SwitchView(new MainWatcherView(_context, _trayWindow));
        }
    }
}
