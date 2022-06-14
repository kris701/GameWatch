using GameWatch.Helpers;
using GameWatch.Models;
using GameWatch.UserControls.Confirmation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
        private SettingsView _parent;
        private Brush _defaultTextboxBackground;
        private Brush _defaultComboboxForeground;

        public GeneralSettings(WindowContext context, SettingsView parent)
        {
            InitializeComponent();
            _parent = parent;
            _context = context;
            RunAtStartupCheckbox.IsChecked = _context.Settings.RunAtStartup;
            ResetWatchersCheckbox.IsChecked = _context.Settings.ResetWatchersWhenClosingSettings;
            _defaultTextboxBackground = RefreshRateTextbox.Background;
            _defaultComboboxForeground = WatcherResetOptionsCombobox.Foreground;
            RefreshRateTextbox.Text = _context.Settings.RefreshRate.ToString();
            WindowFadeDelayTextbox.Text = _context.Settings.WindowFadeDelay.ToString();

            for (int i = 1; i < WatcherResetOptionsConverter.WatcherResetOptionsNames.Length; i++)
                WatcherResetOptionsCombobox.Items.Add(WatcherResetOptionsConverter.WatcherResetOptionsNames[i]);
            WatcherResetOptionsCombobox.SelectedIndex = (int)_context.Settings.ResetOption - 1;
        }

        public bool IsValid()
        {
            TimeSpan res = TimeSpan.Zero;
            bool isValid = true;
            if (!InputHelper.IsTextboxValid(RefreshRateTextbox, !TimeSpan.TryParse(RefreshRateTextbox.Text, out res), _defaultTextboxBackground))
                isValid = false;
            if (!InputHelper.IsTextboxValid(RefreshRateTextbox, res == TimeSpan.Zero, _defaultTextboxBackground))
                isValid = false;
            if (!InputHelper.IsTextboxValid(WindowFadeDelayTextbox, !TimeSpan.TryParse(RefreshRateTextbox.Text, out res), _defaultTextboxBackground))
                isValid = false;
            if (!InputHelper.IsComboboxValid(WatcherResetOptionsCombobox, !(WatcherResetOptionsCombobox.SelectedIndex >= 0 && WatcherResetOptionsCombobox.SelectedIndex < WatcherResetOptionsConverter.WatcherResetOptionsNames.Length), _defaultComboboxForeground))
                isValid = false;
            return isValid;
        }

        public void AcceptChanges()
        {
            if (IsValid())
            {
                _context.Settings.RefreshRate = TimeSpan.Parse(RefreshRateTextbox.Text);
                _context.Settings.WindowFadeDelay = TimeSpan.Parse(WindowFadeDelayTextbox.Text);
                if (ResetWatchersCheckbox.IsChecked != null)
                    _context.Settings.ResetWatchersWhenClosingSettings = (bool)ResetWatchersCheckbox.IsChecked;
                if (RunAtStartupCheckbox.IsChecked != null)
                    _context.Settings.RunAtStartup = (bool)RunAtStartupCheckbox.IsChecked;
                _context.Settings.ResetOption = (WatcherResetOptions)(WatcherResetOptionsCombobox.SelectedIndex + 1);

                SetStartupSetting();
            }
        }

        private void SetStartupSetting()
        {
            var module = Process.GetCurrentProcess().MainModule;
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

        private void ExportSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = ".zip";
            saveFileDialog.Filter = "Zip file (*.zip) | *.zip";
            if (saveFileDialog.ShowDialog() == true)
            {
                var path = GetSavePath();
                if (File.Exists(saveFileDialog.FileName))
                    File.Delete(saveFileDialog.FileName);
                ZipFile.CreateFromDirectory(path, saveFileDialog.FileName);
            }
        }

        private async void ImportSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ConfirmationWindow("This action will remove all current settings.");
            window.ShowDialog();
            if (window.Action == ConfirmationAction.Ok)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = ".zip";
                openFileDialog.Filter = "Zip file (*.zip) | *.zip";
                if (openFileDialog.ShowDialog() == true)
                {
                    var path = GetSavePath();
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                    ZipFile.ExtractToDirectory(openFileDialog.FileName, path);
                    _context.LoadContext();
                    await _parent.SwitchView();
                }
            }
        }

        private async void DeleteSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ConfirmationWindow("This action will remove all current settings.");
            window.ShowDialog();
            if (window.Action == ConfirmationAction.Ok)
            {
                var path = GetSavePath();
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                Directory.CreateDirectory(path);
                _context.LoadContext();
                await _parent.SwitchView();
            }
        }

        private string GetSavePath()
        {
            var assembly = System.Reflection.Assembly.GetEntryAssembly();
            if (assembly == null)
                throw new Exception("");
            return System.IO.Path.GetDirectoryName(assembly.Location) + "\\" + WindowContext.SavePath;
        }

        private void ResetWatchersButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var watched in _context.Watched)
                watched.LastTick = DateTime.UtcNow;
        }
    }
}
