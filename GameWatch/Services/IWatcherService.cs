using GameWatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWatch.Services
{
    public enum WatcherStatus { NotRunYet, Stopped, StoppedByAllowence, Running }
    public interface IWatcherService
    {
        public WatcherStatus Status { get; }
        public INotificationService Notifier { get; }
        public WatchedProcessGroup WatchModelGroup { get; }
        public void StartWatch();
        public void StopWatch();
    }
}
