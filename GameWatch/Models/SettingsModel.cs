using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWatch.Models
{
    public class SettingsModel
    {
        public bool RunAtStartup { get; set; }

        public SettingsModel()
        {
            RunAtStartup = false;
        }

        public SettingsModel(bool runAtStartup)
        {
            RunAtStartup = runAtStartup;
        }
    }
}
