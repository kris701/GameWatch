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
        public List<WatchedProcessGroup> Watched { get; internal set; }
        public List<IWatcherService> Watchers { get; internal set; }
        public SettingsModel Settings { get; internal set; }

        public WindowContext(List<WatchedProcessGroup> watched, List<IWatcherService> watchers, SettingsModel settings)
        {
            Watched = watched;
            Watchers = watchers;
            Settings = settings;
        }

        public void SaveContext(string path)
        {
            if (!Directory.Exists($"{path}/watchers/"))
                Directory.CreateDirectory($"{path}/watchers/");
            foreach (var file in Directory.GetFiles($"{path}/watchers/"))
                File.Delete(file);
            foreach (var watchedProcess in Watched)
                IOHelper.SaveJsonObject($"{path}/watchers/{watchedProcess.ID}.json", watchedProcess);
            IOHelper.SaveJsonObject($"{path}/settings.json", Settings);
        }

        public void LoadContext(string path)
        {
            if (!Directory.Exists($"{path}/watchers/"))
                Directory.CreateDirectory($"{path}/watchers/");
            Watched = IOHelper.LoadJsonObjects<WatchedProcessGroup>($"{path}/watchers/");
            if (File.Exists($"{path}/settings.json"))
            {
                var settings = IOHelper.LoadJsonObject<SettingsModel>($"{path}/settings.json");
                if (settings != null)
                    Settings = settings;
            }
        }
    }
}
