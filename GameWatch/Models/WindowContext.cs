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
        public static readonly string SavePath = "saves/";
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

        public void SaveContext()
        {
            if (!Directory.Exists($"{SavePath}/{_watchersPath}/"))
                Directory.CreateDirectory($"{SavePath}/{_watchersPath}/");
            foreach (var file in Directory.GetFiles($"{SavePath}/{_watchersPath}/"))
                File.Delete(file);
            foreach (var watchedProcess in Watched)
                IOHelper.SaveJsonObject($"{SavePath}/{_watchersPath}/{watchedProcess.ID}.json", watchedProcess);
            IOHelper.SaveJsonObject($"{SavePath}/{_settingsFile}.json", Settings);
        }

        public void LoadContext()
        {
            Watched.Clear();
            Watchers.Clear();
            Settings = new SettingsModel();
            if (!Directory.Exists($"{SavePath}/{_watchersPath}/"))
                Directory.CreateDirectory($"{SavePath}/{_watchersPath}/");
            Watched = IOHelper.LoadJsonObjects<WatchedProcessGroup>($"{SavePath}/{_watchersPath}/");
            if (File.Exists($"{SavePath}/{_settingsFile}.json"))
            {
                var settings = IOHelper.LoadJsonObject<SettingsModel>($"{SavePath}/{_settingsFile}.json");
                if (settings != null)
                    Settings = settings;
            }
        }
    }
}
