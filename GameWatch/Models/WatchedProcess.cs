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
        public int AllowedIntervalSec { get; set; }
        public int PassedSeconds { get; set; }
        public DateTime LastTick { get; set; }

        public WatchedProcess(string processName, string uIName, int allowedIntervalSec, int passedSeconds, DateTime lastTick)
        {
            ProcessName = processName;
            UIName = uIName;
            AllowedIntervalSec = allowedIntervalSec;
            PassedSeconds = passedSeconds;
            LastTick = lastTick;
        }

        public WatchedProcess(string processName, string uIName, int allowedIntervalSec)
        {
            ProcessName = processName;
            UIName = uIName;
            AllowedIntervalSec = allowedIntervalSec;
            PassedSeconds = 0;
            LastTick = DateTime.UtcNow;
        }
    }
}
