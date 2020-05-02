namespace SharpOtto.Core
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// This class is responsible for generating cycles
    /// based on the configured frequency.
    /// </summary>
    public class CyclesTimer
    {
        private Stopwatch systemWatch;

        private int frequency;

        private long lastMs;

        /// <summary>
        /// Initializes a new instance of the <see cref="CyclesTimer" /> class.
        /// </summary>
        /// <param name="systemWatch">The system watch.</param>
        /// <param name="frequency">The frequency expressed in hertz.</param>
        public CyclesTimer(Stopwatch systemWatch, int frequency)
        {
            if (systemWatch is null)
            {
                throw new ArgumentNullException(nameof(systemWatch));
            }

            this.systemWatch = systemWatch;
            this.frequency = frequency;
        }

        /// <summary>
        /// Gets the number of cycles that can be executed within the configured frequency.
        /// </summary>
        /// <returns>The number of cycles.</returns>
        public int GetCycles()
        {
            var currentMs = this.systemWatch.ElapsedMilliseconds;
            var cycles = (int)(currentMs - this.lastMs) * frequency / 1000;
            if (cycles > 0)
            {
                this.lastMs = currentMs;
            }
            
            return cycles;
        }
    }
}