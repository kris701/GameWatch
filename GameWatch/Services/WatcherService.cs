using GameWatch.Helpers;
using GameWatch.Models;
using GameWatch.UserControls.Notification;
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
        private readonly TimeSpan _refreshRateSec = TimeSpan.FromSeconds(1);
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        public WatchedProcessGroup WatchModelGroup { get; }

        public WatcherService(WatchedProcessGroup watchModel)
        {
            WatchModelGroup = watchModel;

            _dispatcherTimer.Tick += Ticker;
            _dispatcherTimer.Interval = _refreshRateSec;
        }

        public void StartWatch()
        {
            _dispatcherTimer.Start();
            WatchModelGroup.Status = WatcherStatus.Searching;
        }

        public void StopWatch()
        {
            _dispatcherTimer.Stop();
            WatchModelGroup.Status = WatcherStatus.Stopped;
        }

        private void Ticker(object? sender, EventArgs e)
        {
            bool any = false;
            foreach (var process in WatchModelGroup.ProcessNames)
            {
                if (Process.GetProcessesByName(process).Length > 0)
                {
                    any = true;
                    break;
                }
            }
            if (any)
            {
                WatchModelGroup.Status = WatcherStatus.Counting;
                if (WatchModelGroup.LastTick.DayOfYear != DateTime.UtcNow.DayOfYear)
                    WatchModelGroup.Passed = TimeSpan.Zero;
                else
                    WatchModelGroup.Passed = WatchModelGroup.Passed.Add(_refreshRateSec);
                WatchModelGroup.LastTick = DateTime.UtcNow;

                if (WatchModelGroup.Passed > WatchModelGroup.Allowed)
                {
                    WatchModelGroup.Status = WatcherStatus.StoppedByAllowence;
                    StopWatch();
                    WatchModelGroup.Passed = TimeSpan.Zero;
                    Notify(WatchModelGroup.UIName);
                }
            }
            else
                WatchModelGroup.Status = WatcherStatus.Searching;
        }

        public void Notify(string text)
        {
            var window = new NotificationWindow(text);
            window.Show();
        }
    }
}
