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
        public List<WatchedProcessGroup> Watched { get; }
        public List<IWatcherService> Watchers { get; }

        public WindowContext(List<WatchedProcessGroup> watched, List<IWatcherService> watchers)
        {
            Watched = watched;
            Watchers = watchers;
        }

        public void SaveContext(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (var file in Directory.GetFiles(path))
                File.Delete(file);
            foreach (var watchedProcess in Watched)
                IOHelper.SaveJsonObject($"{path}/{watchedProcess.ID}.json", watchedProcess);
        }

        public void LoadContext(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Watched.Clear();
            Watched.AddRange(IOHelper.LoadJsonObjects<WatchedProcessGroup>(path));
        }
    }
}
