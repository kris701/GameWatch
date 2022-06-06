using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GameWatch.Models
{
    public class WatchedProcess
    {
        public string ProcessName { get; set; }
        public string UIName { get; set; }
        public int RefreshIntervalSec { get; set; }
        public int AllowedIntervalSec { get; set; }

        private int _passed = 0;
        public int PassedSeconds
        {
            get
            {
                return _passed * RefreshIntervalSec;
            }
        }

        public WatchedProcess(string processName, string uIName, int refreshIntervalSec, int allowedIntervalSec)
        {
            ProcessName = processName;
            UIName = uIName;
            RefreshIntervalSec = refreshIntervalSec;
            AllowedIntervalSec = allowedIntervalSec;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Ticker;
            dispatcherTimer.Interval = new TimeSpan(0, 0, RefreshIntervalSec);
            dispatcherTimer.Start();
        }

        private void Ticker(object? sender, EventArgs e)
        {
            var proc = Process.GetProcessesByName(ProcessName);
            if (proc.Length > 0)
                _passed++;
        }
    }
}
