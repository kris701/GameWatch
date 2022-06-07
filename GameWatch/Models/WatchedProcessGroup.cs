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
        public TimeSpan Allowed { get; set; }
        private TimeSpan _passed;
        public TimeSpan Passed { get { return _passed; } set {
                _passed = value;
                if (Ticked != null)
                    Ticked.Invoke();
            } }
        public DateTime LastTick { get; set; }

        public WatchedProcessGroup()
        {
            ID = Guid.NewGuid();
            ProcessNames = new List<string>();
            UIName = "";
            Allowed = TimeSpan.Zero;
            Passed = TimeSpan.Zero;
            LastTick = DateTime.UtcNow;
            Status = WatcherStatus.NotRunYet;
        }

        public WatchedProcessGroup(Guid id, List<string> processNames, string uIName, TimeSpan allowed, TimeSpan passed, DateTime lastTick)
        {
            ID = id;
            ProcessNames = processNames;
            UIName = uIName;
            Allowed = allowed;
            Passed = passed;
            LastTick = lastTick;
            Status = WatcherStatus.NotRunYet;
        }

        public WatchedProcessGroup(Guid id, List<string> processNames, string uIName, TimeSpan allowed)
        {
            ID = id;
            ProcessNames = processNames;
            UIName = uIName;
            Allowed = allowed;
            Passed = TimeSpan.Zero;
            LastTick = DateTime.UtcNow;
            Status = WatcherStatus.NotRunYet;
        }
    }
}
