using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace RustEssentials.Util
{
    public class TimerPlus
    {
        private Timer timer;
        public TimerCallback timerCallback;
        private Stopwatch stopWatch = new Stopwatch();
        public bool AutoReset = false;
        public long Interval = 0;
        public bool isNull = false;

        public void Start()
        {
            stopWatch.Start();
            timer = new Timer(timerCallback, null, Interval, (AutoReset ? Interval : Timeout.Infinite));
        }

        public void Stop()
        {
            stopWatch.Stop();
            timer.Dispose();
        }
        
        public double TimeLeft
        {
            get
            {
                stopWatch.Stop();
                long elapsedTime = stopWatch.ElapsedMilliseconds;
                stopWatch.Start();
                return (Interval - elapsedTime);
            }
        }
    }
}
