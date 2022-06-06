﻿using GameWatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWatch.Services
{
    internal interface IWatcherService
    {
        public INotificationService Notifier { get; }
        public WatchedProcess WatchModel { get; }
        public void StartWatch();
        public void StopWatch();
        public void SaveProgress(string path);
    }
}