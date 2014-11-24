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
        public delegate void Callback();
        public delegate void CallbackArgs(params object[] args);
        internal System.Threading.Timer timer;
        internal TimerCallback timerCallback;
        internal Callback callback;
        internal CallbackArgs callbackArgs;
        internal Stopwatch stopWatch;
        internal bool isRunning = false;
        internal long intervalTime = 0;
        internal int resetsFinished = 0;
        internal bool autoReset = false;
        internal int resetAmount = 0;
        internal object[] args;
        public bool isNull = false;

        private TimerPlus(long interval, bool autoReset, int resetAmount, Callback callback)
        {
            this.intervalTime = interval;
            this.autoReset = autoReset;
            this.callback = callback;
            this.timerCallback = new TimerCallback(DoCallback);
            stopWatch = new Stopwatch();
            this.isRunning = true;
            timer = new System.Threading.Timer(timerCallback, null, interval, (autoReset || resetAmount > 0 ? interval : Timeout.Infinite));
            stopWatch.Start();
        }

        private TimerPlus(long interval, bool autoReset, int resetAmount, CallbackArgs callback, params object[] args)
        {
            this.intervalTime = interval;
            this.autoReset = autoReset;
            this.callbackArgs = callback;
            this.args = args;
            this.timerCallback = new TimerCallback(DoCallbackArgs);
            stopWatch = new Stopwatch();
            this.isRunning = true;
            timer = new System.Threading.Timer(timerCallback, null, interval, (autoReset || resetAmount > 0 ? interval : Timeout.Infinite));
            stopWatch.Start();
        }

        public TimerPlus()
        {

        }

        public int resetsDone
        {
            get
            {
                return resetsFinished;
            }
        }

        public long interval
        {
            get
            {
                return intervalTime;
            }
            set
            {
                timer.Change(value, value);
            }
        }

        internal void DoCallback(object obj)
        {
            if (isRunning)
            {
                if (!autoReset && resetAmount > 0)
                    resetsFinished++;

                callback();

                if ((resetsDone >= resetAmount && resetAmount > 0) || (!autoReset && resetAmount == 0))
                    this.stop();
            }
        }

        internal void DoCallbackArgs(object obj)
        {
            if (isRunning)
            {
                if (!autoReset && resetAmount > 0)
                    resetsFinished++;

                callbackArgs(args);

                if ((resetsDone >= resetAmount && resetAmount > 0) || (!autoReset && resetAmount == 0))
                    this.stop();
            }
        }

        public static TimerPlus Create(long intervalTime, Callback callback)
        {
            return new TimerPlus(intervalTime, false, 0, callback);
        }

        public static TimerPlus Create(long intervalTime, bool autoReset, Callback callback)
        {
            return new TimerPlus(intervalTime, autoReset, 0, callback);
        }

        public static TimerPlus Create(long intervalTime, int resetAmount, Callback callback)
        {
            return new TimerPlus(intervalTime, false, resetAmount, callback);
        }

        public static TimerPlus Create(long intervalTime, CallbackArgs callback, params object[] args)
        {
            return new TimerPlus(intervalTime, false, 0, callback, args);
        }

        public static TimerPlus Create(long intervalTime, bool autoReset, CallbackArgs callback, params object[] args)
        {
            return new TimerPlus(intervalTime, autoReset, 0, callback, args);
        }

        public static TimerPlus Create(long intervalTime, int resetAmount, CallbackArgs callback, params object[] args)
        {
            return new TimerPlus(intervalTime, false, resetAmount, callback, args);
        }

        public void start()
        {
            if (!isRunning)
            {
                isRunning = true;
                timer.Change(interval, interval);
            }
        }

        public void stop()
        {
            if (isRunning)
            {
                isRunning = false;
                timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        public void dispose()
        {
            isRunning = false;
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            stopWatch.Stop();
            timer.Dispose();
        }

        public double timeLeft
        {
            get
            {
                stopWatch.Stop();
                long elapsedTime = stopWatch.ElapsedMilliseconds;
                stopWatch.Start();
                return (interval - elapsedTime);
            }
        }
    }
}
