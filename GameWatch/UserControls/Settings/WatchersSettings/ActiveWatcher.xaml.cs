using GameWatch.Helpers;
using GameWatch.Models;
using GameWatch.UserControls.Settings;
using GameWatch.UserControls.Settings.WatchersSettings;
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
    public partial class ActiveWatcher : UserControl, ValidatorControl
    {
        public WatchedProcessGroup WatchedProcess { get; }

        private Brush _defaultTextboxBackground;
        private WatchersSettings _parent;

        public ActiveWatcher(WatchersSettings parent, WatchedProcessGroup watchedProcess)
        {
            InitializeComponent();
            WatchedProcess = watchedProcess;
            _parent = parent;
            ProcessNameTextbox.Text = String.Join(",", WatchedProcess.ProcessNames);
            UINameTextbox.Text = WatchedProcess.UIName;
            AllowedTimeTextbox.Text = WatchedProcess.Allowed.ToString();
            _defaultTextboxBackground = AllowedTimeTextbox.Background;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _parent.RemoveWatcher(this);
        }

        public bool IsValid()
        {
            TimeSpan res = TimeSpan.Zero;
            bool isValid = true;
            if (!InputHelper.IsTextboxValid(ProcessNameTextbox, ProcessNameTextbox.Text.Split(",").ToList().Count == 0, _defaultTextboxBackground))
                isValid = false;
            if (!InputHelper.IsTextboxValid(UINameTextbox, UINameTextbox.Text == "", _defaultTextboxBackground))
                isValid = false;
            if (!InputHelper.IsTextboxValid(AllowedTimeTextbox, !TimeSpan.TryParse(AllowedTimeTextbox.Text, out res), _defaultTextboxBackground))
                isValid = false;
            if (!InputHelper.IsTextboxValid(AllowedTimeTextbox, res == TimeSpan.Zero, _defaultTextboxBackground))
                isValid = false;
            return isValid;
        }

        public void AcceptChanges()
        {
            if (IsValid())
            {
                WatchedProcess.ProcessNames = ProcessNameTextbox.Text.Split(",").ToList();
                WatchedProcess.UIName = UINameTextbox.Text;
                WatchedProcess.Allowed = TimeSpan.Parse(AllowedTimeTextbox.Text);
                WatchedProcess.LastTick = DateTime.UtcNow;
            }
        }
    }
}
