using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GameWatch.Models
{
    public enum WatcherStatus { NotRunYet, Stopped, StoppedByAllowence, Counting, Searching }
    public class WatchedProcessGroup
    {
        public delegate void TickedHandler();
        public event TickedHandler? Ticked;

        private WatcherStatus _status;
        public WatcherStatus Status { get { return _status; } set {
                _status = value;
                if (Ticked != null)
                    Ticked.Invoke();
            } }
        public Guid ID { get; set; }
        public List<string> ProcessNames { get; set; }
        public string UIName { get; set; }
        public int AllowedIntervalSec { get; set; }
        private int _passedSeconds;
        public int PassedSeconds { get { return _passedSeconds; } set {
                _passedSeconds = value;
                if (Ticked != null)
                    Ticked.Invoke();
            } }
        public DateTime LastTick { get; set; }

        public WatchedProcessGroup()
        {
            ID = Guid.NewGuid();
            ProcessNames = new List<string>();
            UIName = "";
            AllowedIntervalSec = -1;
            PassedSeconds = -1;
            LastTick = DateTime.UtcNow;
            Status = WatcherStatus.NotRunYet;
        }

        public WatchedProcessGroup(Guid id, List<string> processNames, string uIName, int allowedIntervalSec, int passedSeconds, DateTime lastTick)
        {
            ID = id;
            ProcessNames = processNames;
            UIName = uIName;
            AllowedIntervalSec = allowedIntervalSec;
            PassedSeconds = passedSeconds;
            LastTick = lastTick;
            Status = WatcherStatus.NotRunYet;
        }

        public WatchedProcessGroup(Guid id, List<string> processNames, string uIName, int allowedIntervalSec)
        {
            ID = id;
            ProcessNames = processNames;
            UIName = uIName;
            AllowedIntervalSec = allowedIntervalSec;
            PassedSeconds = 0;
            LastTick = DateTime.UtcNow;
            Status = WatcherStatus.NotRunYet;
        }
    }
}
