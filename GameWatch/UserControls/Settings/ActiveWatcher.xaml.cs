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
        private Brush _defaultTextboxBackground;
        private WatchedProcessGroup _watchedProcess;
        private WatcherSettings _parent;
        public bool IsValid = true;

        public ActiveWatcher(WatcherSettings parent, WatchedProcessGroup watchedProcess)
        {
            InitializeComponent();
            _watchedProcess = watchedProcess;
            _parent = parent;
            ProcessNameTextbox.Text = String.Join(",", _watchedProcess.ProcessNames);
            UINameTextbox.Text = _watchedProcess.UIName;
            AllowedTimeTextbox.Text = _watchedProcess.AllowedIntervalSec.ToString();
            _defaultTextboxBackground = AllowedTimeTextbox.Background;
        }

        private void AllowedTimeTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = InputHelper.IsOnlyNumbers(e.Text);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _parent.RemoveWatcher(_watchedProcess);
        }

        private void UIInputChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = true;
            if (ProcessNameTextbox.Text.Split(",").ToList().Count == 0)
            {
                ProcessNameTextbox.Background = Brushes.DarkRed;
                IsValid = false;
            }
            else
                ProcessNameTextbox.Background = _defaultTextboxBackground;
            if (UINameTextbox.Text == "")
            {
                UINameTextbox.Background = Brushes.DarkRed;
                IsValid = false;
            }
            else
                UINameTextbox.Background = _defaultTextboxBackground;
            if (AllowedTimeTextbox.Text == "")
            {
                AllowedTimeTextbox.Background = Brushes.DarkRed;
                IsValid = false;
            }
            else
                AllowedTimeTextbox.Background = _defaultTextboxBackground;
            if (IsValid)
            {
                _watchedProcess.ProcessNames = ProcessNameTextbox.Text.Split(",").ToList();
                _watchedProcess.UIName = UINameTextbox.Text;
                _watchedProcess.AllowedIntervalSec = Int32.Parse(AllowedTimeTextbox.Text);
                _watchedProcess.PassedSeconds = 0;
                _watchedProcess.LastTick = DateTime.UtcNow;
            }
        }
    }
}
