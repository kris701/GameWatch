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
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        public TimeSpan RefreshRate { get; }
        public WatchedProcessGroup WatchModelGroup { get; }

        public WatcherService(WatchedProcessGroup watchModel, TimeSpan refreshRate)
        {
            WatchModelGroup = watchModel;

            _dispatcherTimer.Tick += Ticker;
            RefreshRate = refreshRate;
            _dispatcherTimer.Interval = refreshRate;
        }

        public void StartWatch()
        {
            _dispatcherTimer.Start();
            WatchModelGroup.Status = WatcherStatus.Searching;
        }

        public void StopWatch()
        {
            _dispatcherTimer.Stop();
            WatchModelGroup.Passed = TimeSpan.Zero;
            WatchModelGroup.Status = WatcherStatus.Stopped;
        }

        public void PauseWatch()
        {
            _dispatcherTimer.Stop();
            WatchModelGroup.Status = WatcherStatus.Paused;
        }

        private void Ticker(object? sender, EventArgs e)
        {
            if (WatchModelGroup.LastTick.DayOfYear != DateTime.UtcNow.DayOfYear)
                WatchModelGroup.Passed = TimeSpan.Zero;
            WatchModelGroup.LastTick = DateTime.UtcNow;

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
                WatchModelGroup.Passed = WatchModelGroup.Passed.Add(RefreshRate);

                if (WatchModelGroup.Passed >= WatchModelGroup.Allowed)
                {
                    var window = new NotificationWindow(WatchModelGroup);
                    window.ShowDialog();
                    NotificationChoice(window.Action);
                }
            }
            else
                WatchModelGroup.Status = WatcherStatus.Searching;
        }

        private void NotificationChoice(NotificationAction action)
        {
            switch (action)
            {
                case NotificationAction.StopTimer:
                    WatchModelGroup.Status = WatcherStatus.StoppedByAllowence;
                    StopWatch();
                    WatchModelGroup.Passed = TimeSpan.Zero;
                    break;
                case NotificationAction.Add15Min:
                    if (WatchModelGroup.Allowed > TimeSpan.FromMinutes(15))
                        WatchModelGroup.Passed = WatchModelGroup.Passed.Subtract(TimeSpan.FromMinutes(15));
                    else
                        WatchModelGroup.Passed = TimeSpan.Zero;
                    break;
            }
        }
    }
}
