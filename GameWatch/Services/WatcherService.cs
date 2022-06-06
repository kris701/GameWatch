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
        private readonly int _refreshRateSec = 10;
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        public INotificationService Notifier { get; }
        public WatchedProcess WatchModel { get; }

        public WatcherService(WatchedProcess watchModel)
        {
            WatchModel = watchModel;
            Notifier = new NotificationService();

            _dispatcherTimer.Tick += Ticker;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, _refreshRateSec);
        }

        public WatcherService(string path) : this(IOHelper.LoadJsonObject<WatchedProcess>(path))
        {
        }

        public void StartWatch()
        {
            _dispatcherTimer.Start();
        }

        public void StopWatch()
        {
            _dispatcherTimer.Stop();
        }

        public void SaveProgress(string path)
        {
            IOHelper.SaveJsonObject(path, WatchModel);
        }

        private void Ticker(object? sender, EventArgs e)
        {
            var proc = Process.GetProcessesByName(WatchModel.ProcessName);
            if (proc.Length > 0)
            {
                if (WatchModel.LastTick.DayOfYear != DateTime.UtcNow.DayOfYear)
                    WatchModel.PassedSeconds = 0;
                else
                    WatchModel.PassedSeconds += _refreshRateSec;
                WatchModel.LastTick = DateTime.UtcNow;

                if (WatchModel.PassedSeconds > WatchModel.AllowedIntervalSec)
                    Notifier.Notify("aaa");
            }
        }
    }
}
