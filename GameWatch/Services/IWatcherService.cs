using GameWatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWatch.Services
{
    public interface IWatcherService
    {
        public SettingsModel Settings { get; }
        public WatchedProcessGroup WatchModelGroup { get; }
        public void StartWatch();
        public void StopWatch();
        public void PauseWatch();
        public void ResetWatcher();
    }
}
