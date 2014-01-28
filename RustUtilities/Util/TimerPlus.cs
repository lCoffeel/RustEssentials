using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace RustEssentials.Util
{
    public class TimerPlus : Timer
    {
        private Stopwatch stopWatch = new Stopwatch();

        protected new void Dispose()
        {
            stopWatch.Stop();
            base.Dispose();
        }

        public double TimeLeft
        {
            get
            {
                stopWatch.Stop();
                long elapsedTime = stopWatch.ElapsedMilliseconds;
                stopWatch.Start();
                return (this.Interval - elapsedTime);
            }
        }

        public new void Start()
        {
            stopWatch.Start();
            base.Start();
        }
    }
}
