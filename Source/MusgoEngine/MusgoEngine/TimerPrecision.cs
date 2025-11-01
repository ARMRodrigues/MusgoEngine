using System.Runtime.InteropServices;

namespace MusgoEngine
{
    public static class TimerPrecision
    {
        private static bool _enabled;

#if WINDOWS
        [DllImport("winmm.dll")]
        private static extern int timeBeginPeriod(int uPeriod);

        [DllImport("winmm.dll")]
        private static extern int timeEndPeriod(int uPeriod);
#endif

        /// <summary>
        /// Enables high timer precision on Windows. Does nothing on other platforms.
        /// </summary>
        public static void EnableHighPrecision()
        {
#if WINDOWS
            if (_enabled || !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

            timeBeginPeriod(1);
            _enabled = true;
#endif
        }

        /// <summary>
        /// Restores default timer precision on Windows. Does nothing on other platforms.
        /// </summary>
        public static void DisableHighPrecision()
        {
#if WINDOWS
            if (!_enabled || !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

            timeEndPeriod(1);
            _enabled = false;
#endif
        }
    }
}
