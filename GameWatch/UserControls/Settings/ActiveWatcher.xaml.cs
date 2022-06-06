using GameWatch.Helpers;
using GameWatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GameWatch.UserControls
{
    /// <summary>
    /// Interaction logic for ActiveWatcher.xaml
    /// </summary>
    public partial class ActiveWatcher : UserControl
    {
        private WatchedProcessGroup _watchedProcess;

        public ActiveWatcher(WatchedProcessGroup watchedProcess)
        {
            InitializeComponent();
            _watchedProcess = watchedProcess;
            ProcessNameTextbox.Text = String.Join(",", _watchedProcess.ProcessNames);
            UINameTextbox.Text = _watchedProcess.UIName;
            AllowedTimeTextbox.Text = _watchedProcess.AllowedIntervalSec.ToString();
        }

        private void AllowedTimeTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = InputHelper.IsOnlyNumbers(e.Text);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _watchedProcess.ProcessNames = ProcessNameTextbox.Text.Split(",").ToList();
            _watchedProcess.UIName = UINameTextbox.Text;
            _watchedProcess.AllowedIntervalSec = Int32.Parse(AllowedTimeTextbox.Text);
            _watchedProcess.PassedSeconds = 0;
            _watchedProcess.LastTick = DateTime.UtcNow;
        }
    }
}
