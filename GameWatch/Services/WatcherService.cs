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
        private bool _haveReset = false;

        public SettingsModel Settings { get; }
        public WatchedProcessGroup WatchModelGroup { get; }

        public WatcherService(WatchedProcessGroup watchModel, SettingsModel settings)
        {
            WatchModelGroup = watchModel;
            Settings = settings;

            _dispatcherTimer.Tick += Ticker;
            _dispatcherTimer.Interval = Settings.RefreshRate;

            DetermineIfResetIsNeeded();
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
            DetermineIfResetIsNeeded();

            if (IsAnyWatchedProcessRunning())
            {
                WatchModelGroup.Status = WatcherStatus.Counting;
                WatchModelGroup.Passed = WatchModelGroup.Passed.Add(Settings.RefreshRate);

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

        private bool IsAnyWatchedProcessRunning()
        {
            foreach (var process in WatchModelGroup.ProcessNames)
                if (Process.GetProcessesByName(process).Length > 0)
                    return true;
            return false;
        }

        private void DetermineIfResetIsNeeded()
        {
            switch (Settings.ResetOption)
            {
                case WatcherResetOptions.ResetOverMidnight:
                    if (WatchModelGroup.LastTick.DayOfYear != DateTime.UtcNow.DayOfYear)
                        ResetWatcher();
                    break;
                case WatcherResetOptions.ResetAfter24h:
                    if (DateTime.UtcNow >= WatchModelGroup.LastTick.AddHours(12))
                        ResetWatcher();
                    break;
                case WatcherResetOptions.ResetAfter12h:
                    if (DateTime.UtcNow >= WatchModelGroup.LastTick.AddHours(24))
                        ResetWatcher();
                    break;
                case WatcherResetOptions.OnStartup:
                    if (!_haveReset)
                        ResetWatcher();
                    _haveReset = true;
                    break;
            }
        }

        public void ResetWatcher()
        {
            WatchModelGroup.Passed = TimeSpan.Zero;
            WatchModelGroup.LastTick = DateTime.UtcNow;
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
