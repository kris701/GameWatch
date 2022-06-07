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

        public SettingsModel()
        {
            RunAtStartup = false;
            ResetWatchersWhenClosingSettings = true;
        }

        public SettingsModel(bool runAtStartup, bool resetWatchersWhenClosingSettings)
        {
            RunAtStartup = runAtStartup;
            ResetWatchersWhenClosingSettings = resetWatchersWhenClosingSettings;
        }
    }
}
