using GameWatch.Helpers;
using GameWatch.Models;
using GameWatch.UserControls.Confirmation;
using GameWatch.UserControls.Settings;
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

namespace GameWatch.UserControls.PresetSettings
{
    /// <summary>
    /// Interaction logic for PresetSettings.xaml
    /// </summary>
    public partial class PresetSettings : UserControl
    {
        private WindowContext _context;
        private SettingsView _parent;

        public PresetSettings(WindowContext context, SettingsView parent)
        {
            InitializeComponent();
            _parent = parent;
            _context = context;
            PresetNameTextbox.Text = _context.Name;
        }

        private void ExportSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = ".json";
            saveFileDialog.Filter = "Json file (*.json) | *.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                if (File.Exists(saveFileDialog.FileName))
                    File.Delete(saveFileDialog.FileName);
                PresetSaverHelper.SavePreset(_context);
            }
        }

        private async void ImportSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ConfirmationWindow("This action will remove all current settings. If a preset have the same name as the file you pick, the preset will be overwritten.");
            window.ShowDialog();
            if (window.Action == ConfirmationAction.Ok)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = ".zip";
                openFileDialog.Filter = "Zip file (*.zip) | *.zip";
                if (openFileDialog.ShowDialog() == true)
                {
                    PresetSaverHelper.LoadNewPreset(_context, openFileDialog.FileName);
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
                PresetSaverHelper.DeletePreset(_context.Name);
                PresetSaverHelper.LoadDefaultPreset(_context);
                await _parent.ResetView();
            }
        }

        private void ResetWatchersButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var watched in _context.Watched)
                watched.LastTick = DateTime.UtcNow;
        }

        private async void SettingsPresetCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox combobox && combobox.SelectedItem != null)
            {
                var newItem = combobox.SelectedItem.ToString();
                if (newItem != null) {
                    PresetSaverHelper.LoadPreset(_context, newItem);
                    Properties.Settings.Default.PresetName = newItem;
                    await _parent.ResetView();
                }
            }
        }

        private void SettingsPresetCombobox_DropDownOpened(object sender, EventArgs e)
        {
            SettingsPresetCombobox.Items.Clear();
            foreach (var item in PresetSaverHelper.GetPresetNames())
                SettingsPresetCombobox.Items.Add(item);
        }

        private async void AddNewPresetButton_Click(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();
            var newPresetName = $"New Preset {rnd.Next(0, 99999)}";

            PresetSaverHelper.SavePreset(
                new WindowContext(
                    new List<WatchedProcessGroup>(),
                    new List<Services.IWatcherService>(),
                    new SettingsModel(),
                    newPresetName));
            PresetSaverHelper.LoadPreset(_context, newPresetName);
            await _parent.ResetView();
        }

        private void PresetNameTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PresetNameTextbox.Text != "")
            {
                PresetSaverHelper.DeletePreset(_context.Name);
                _context.Name = PresetNameTextbox.Text;
                PresetSaverHelper.SavePreset(_context);
            }
        }
    }
}
