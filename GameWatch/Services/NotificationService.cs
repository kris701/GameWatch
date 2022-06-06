using GameWatch.UserControls.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameWatch.Services
{
    internal class NotificationService : INotificationService
    {
        public void Notify(string text)
        {
            var window = new NotificationWindow(text);
            window.Show();
        }
    }
}
