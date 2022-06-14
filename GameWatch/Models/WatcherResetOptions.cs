using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWatch.Models
{
    public enum WatcherResetOptions { None, ResetOverMidnight, ResetAfter24h, ResetAfter12h }
    public static class WatcherResetOptionsConverter
    {
        public static string[] WatcherResetOptionsNames = new string[] { "Null", "Over Midnight", "After 24 hours", "After 12 hours" };
    }
}
