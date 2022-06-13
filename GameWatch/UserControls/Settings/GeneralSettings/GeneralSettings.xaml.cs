using GameWatch.Helpers;
using GameWatch.Models;
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

namespace GameWatch.UserControls.Settings
{
    /// <summary>
    /// Interaction logic for GeneralSettings.xaml
    /// </summary>
    public partial class GeneralSettings : UserControl, ValidatorControl
    {
        private WindowContext _context;
        private Brush _defaultTextboxBackground;

        public GeneralSettings(WindowContext context)
        {
            InitializeComponent();
            _context = context;
            RunAtStartupCheckbox.IsChecked = _context.Settings.RunAtStartup;
            ResetWatchersCheckbox.IsChecked = _context.Settings.ResetWatchersWhenClosingSettings;
            _defaultTextboxBackground = RefreshRateTextbox.Background;
            RefreshRateTextbox.Text = _context.Settings.RefreshRate.ToString();
        }

        public bool IsValid()
        {
            TimeSpan res = TimeSpan.Zero;
            bool isValid = true;
            if (!InputHelper.IsTextboxValid(RefreshRateTextbox, !TimeSpan.TryParse(RefreshRateTextbox.Text, out res), _defaultTextboxBackground))
                isValid = false;
            if (!InputHelper.IsTextboxValid(RefreshRateTextbox, res == TimeSpan.Zero, _defaultTextboxBackground))
                isValid = false;
            return isValid;
        }

        public void AcceptChanges()
        {
            if (IsValid())
            {
                _context.Settings.RefreshRate = TimeSpan.Parse(RefreshRateTextbox.Text);
                if (ResetWatchersCheckbox.IsChecked != null)
                    _context.Settings.ResetWatchersWhenClosingSettings = (bool)ResetWatchersCheckbox.IsChecked;
                if (RunAtStartupCheckbox.IsChecked != null)
                    _context.Settings.RunAtStartup = (bool)RunAtStartupCheckbox.IsChecked;

                SetStartupSetting();
            }
        }

        private void SetStartupSetting()
        {
            var module = System.Diagnostics.Process.GetCurrentProcess().MainModule;
            if (module == null || module.FileName == null)
                throw new Exception("Could not find current running assembly!");

            if (_context.Settings.RunAtStartup)
                IOHelper.GenerateShortcut(
                    $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\",
                    "GameWatch",
                    module.FileName);
            else
                IOHelper.RemoveShortcut(
                    $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\",
                    "GameWatch",
                    module.FileName);
        }
    }
}
