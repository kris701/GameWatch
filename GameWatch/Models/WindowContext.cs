using GameWatch.Services;
using System;
using System.Collections.Generic;
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
    }
}
