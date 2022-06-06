using GameWatch.Helpers;
using GameWatch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GameWatch.Services
{
    internal class WatcherService : IWatcherService
    {
        private readonly int _refreshRateSec = 1;
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        public INotificationService Notifier { get; }
        public WatchedProcessGroup WatchModelGroup { get; }

        public WatcherService(WatchedProcessGroup watchModel)
        {
            WatchModelGroup = watchModel;
            Notifier = new NotificationService();

            _dispatcherTimer.Tick += Ticker;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, _refreshRateSec);
        }

        public void StartWatch()
        {
            _dispatcherTimer.Start();
        }

        public void StopWatch()
        {
            _dispatcherTimer.Stop();
        }

        private void Ticker(object? sender, EventArgs e)
        {
            var list = new List<Process>();
            foreach(var process in WatchModelGroup.ProcessNames)
                list.AddRange(Process.GetProcessesByName(process).ToList());
            if (list.Count > 0)
            {
                if (WatchModelGroup.LastTick.DayOfYear != DateTime.UtcNow.DayOfYear)
                    WatchModelGroup.PassedSeconds = 0;
                else
                    WatchModelGroup.PassedSeconds += _refreshRateSec;
                WatchModelGroup.LastTick = DateTime.UtcNow;

                if (WatchModelGroup.PassedSeconds > WatchModelGroup.AllowedIntervalSec)
                    Notifier.Notify("aaa");
            }
        }
    }
}
