using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWatch.UserControls.Settings
{
    public interface ValidatorControl
    {
        public bool IsValid();
        public void AcceptChanges();
    }
}
