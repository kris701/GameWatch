using GameWatch.Models;
using GameWatch.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class MainWindow : Window
    {
        private List<WatchedProcess> _watched;

        public MainWindow()
        {
            InitializeComponent();
            _watched = new List<WatchedProcess>();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new WatcherSettings(_watched);
            window.ShowDialog();
        }
    }
}
