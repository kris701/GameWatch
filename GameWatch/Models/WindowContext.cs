using GameWatch.Helpers;
using GameWatch.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameWatch.Models
{
    public class WindowContext
    {
        public string Name { get; set; }
        public List<WatchedProcessGroup> Watched { get; set; }
        [JsonIgnore]
        public List<IWatcherService> Watchers { get; set; } = new List<IWatcherService>();
        public SettingsModel Settings { get; set; }

        public WindowContext(string name)
        {
            Watched = new List<WatchedProcessGroup>();
            Watchers = new List<IWatcherService>();
            Settings = new SettingsModel();
            Name = name;
        }
    }
}
