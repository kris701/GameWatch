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
        private static readonly string _watchersPath = "watchers";
        private static readonly string _settingsFile = "settings";

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
            if (!Directory.Exists($"{path}/{_watchersPath}/"))
                Directory.CreateDirectory($"{path}/{_watchersPath}/");
            foreach (var file in Directory.GetFiles($"{path}/{_watchersPath}/"))
                File.Delete(file);
            foreach (var watchedProcess in Watched)
                IOHelper.SaveJsonObject($"{path}/{_watchersPath}/{watchedProcess.ID}.json", watchedProcess);
            IOHelper.SaveJsonObject($"{path}/{_settingsFile}.json", Settings);
        }

        public void LoadContext(string path)
        {
            if (!Directory.Exists($"{path}/{_watchersPath}/"))
                Directory.CreateDirectory($"{path}/{_watchersPath}/");
            Watched = IOHelper.LoadJsonObjects<WatchedProcessGroup>($"{path}/{_watchersPath}/");
            if (File.Exists($"{path}/{_settingsFile}.json"))
            {
                var settings = IOHelper.LoadJsonObject<SettingsModel>($"{path}/{_settingsFile}.json");
                if (settings != null)
                    Settings = settings;
            }
        }
    }
}
