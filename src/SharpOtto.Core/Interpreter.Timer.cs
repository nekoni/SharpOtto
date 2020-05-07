namespace SharpOtto.Core
{
    using System;
    using System.Diagnostics;

    public partial class Interpreter : ITimer
    {
        private const int CpuClockHz = 1000;

        private const int TimersClockHz = 60;

        private Stopwatch clock = new Stopwatch();

        private CyclesTimer cpuTimer;

        private CyclesTimer delayAndSoundTimer;

        public sbyte Delay { get; set; }

        public sbyte Sound { get; set; }

        private void InitTimers()
        {
            this.clock.Reset();
            this.clock.Start();
            this.cpuTimer = new CyclesTimer(this.clock, CpuClockHz);
            this.delayAndSoundTimer = new CyclesTimer(this.clock, TimersClockHz);
        }

        private void UpdateTimers()
        {
            var delayAndSoundCycles = this.delayAndSoundTimer.GetCycles();
            if (this.Delay > 0)
            {
                this.Delay -= (sbyte)delayAndSoundCycles;
            }
            else
            {
                this.Delay = 0;
            }

            if (this.Sound > 0)
            {
                this.Sound -= (sbyte)delayAndSoundCycles;
                if (this.Sound <= 0)
                {
                    Console.WriteLine("Beep");
                }
            }
            else
            {
                this.Sound = 0;
            }
        }
    }
}