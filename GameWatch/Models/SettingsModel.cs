using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWatch.Models
{

    public class SettingsModel
    {
        public bool RunAtStartup { get; set; }
        public bool ResetWatchersWhenClosingSettings { get; set; }
        public TimeSpan RefreshRate { get; set; }
        public TimeSpan WindowFadeDelay { get; set; }
        public WatcherResetOptions ResetOption { get; set; }

        public SettingsModel()
        {
            RunAtStartup = false;
            ResetWatchersWhenClosingSettings = true;
            RefreshRate = TimeSpan.FromSeconds(1);
            WindowFadeDelay = TimeSpan.Zero;
            ResetOption = WatcherResetOptions.ResetOverMidnight;
        }

        public SettingsModel(bool runAtStartup, bool resetWatchersWhenClosingSettings, TimeSpan refreshRate, TimeSpan windowFadeDelay, WatcherResetOptions resetOption)
        {
            RunAtStartup = runAtStartup;
            ResetWatchersWhenClosingSettings = resetWatchersWhenClosingSettings;
            RefreshRate = refreshRate;
            WindowFadeDelay = windowFadeDelay;
            ResetOption = resetOption;
        }
    }
}
