using GameWatch.Helpers;
using GameWatch.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWatch.Models
{
    public class WindowContext
    {
        public string Name { get; set; }
        public List<WatchedProcessGroup> Watched { get; set; }
        public List<IWatcherService> Watchers { get; set; }
        public SettingsModel Settings { get; set; }

        public WindowContext(List<WatchedProcessGroup> watched, List<IWatcherService> watchers, SettingsModel settings, string name)
        {
            Watched = watched;
            Watchers = watchers;
            Settings = settings;
            Name = name;
        }
    }
}
