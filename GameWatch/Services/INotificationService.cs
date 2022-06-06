using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWatch.Services
{
    public interface INotificationService
    {
        public void Notify(string text);
    }
}
