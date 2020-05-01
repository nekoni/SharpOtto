using System.Diagnostics;

namespace SharpOtto.Core
{
    public class CyclesTimer
    {
        private Stopwatch watch;

        private int frequency;

        private long lastMs;

        public CyclesTimer(Stopwatch watch, int frequency)
        {
            this.watch = watch;
            this.frequency = frequency;
        }

        public int GetCycles()
        {
            var currentMs = this.watch.ElapsedMilliseconds;
            var cycles = (int)(currentMs - lastMs) * frequency / 1000;
            if (cycles > 0)
            {
                lastMs = currentMs;
            }
            
            return cycles;
        }
    }
}
