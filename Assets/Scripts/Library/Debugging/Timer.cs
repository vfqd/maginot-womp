using System.Diagnostics;
using Library.Extensions;
using Debug = UnityEngine.Debug;

namespace Library.Debugging
{
    public static class Timer
    {
#if UNITY_EDITOR
        // Silence the warnings in the editor, but use faster const and compiler optimization in builds
        public static bool IsOn = false;
#else
        public const bool IsOn = false;
#endif
        private static Stopwatch _stopwatch;

        public static void StartTimer()
        {
            _stopwatch = Stopwatch.StartNew();
            Debug.Log("<TIMER> Starting timer".Bold().Magenta());
        }
        
        public static void Split(string message)
        {
            Debug.Log($"<TIMER> At {message} timer = {_stopwatch.Elapsed}".Bold().Magenta());
        }
        
        public static void Stop()
        {
            Debug.Log($"<TIMER> Stopping timer, total length = {_stopwatch.Elapsed}".Bold().Magenta());
        }
    }
}