using System;
using System.Timers;
using System.Windows.Interop;

namespace WeighBridgeApplication.Util
{
    public class AutoLogOffHelper
    {
        public delegate void MakeAutoLogOff();

        private static Timer _timer;
        public static int LogOffTime { get; set; }

        public static event MakeAutoLogOff MakeAutoLogOffEvent;

        public static void StartAutoLogoffOption()
        {
            LogOffTime = 1;
            ComponentDispatcher.ThreadIdle += DispatcherQueueEmptyHandler;
        }

        private static void _timer_Tick(object sender, EventArgs e)
        {
            if (_timer != null)
            {
                ComponentDispatcher.ThreadIdle -= DispatcherQueueEmptyHandler;
                _timer.Stop();
                _timer = null;
                if (MakeAutoLogOffEvent != null)
                {
                    MakeAutoLogOffEvent();
                }
            }
        }

        private static void DispatcherQueueEmptyHandler(object sender, EventArgs e)
        {
            if (_timer == null)
            {
                _timer = new Timer {Interval = LogOffTime*60*1000, Enabled = true};
                //                _timer.Tick += new EventHandler(_timer_Tick);
            }
            else if (_timer.Enabled == false)
            {
                _timer.Enabled = true;
            }
        }

        public static void ResetLogoffTimer()
        {
            if (_timer != null)
            {
                _timer.Enabled = false;
                _timer.Enabled = true;
            }
        }
    }
}