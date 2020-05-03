namespace SharpOtto.Core
{
    using System;
    using System.Diagnostics;

    public partial class Interpreter : ITimer
    {
        private const int CpuClockHz = 1000;

        private const int TimersClockHz = 60;

        private sbyte delay;

        private sbyte sound;

        private Stopwatch clock = new Stopwatch();

        private CyclesTimer cpuTimer;

        private CyclesTimer delayAndSoundTimer;

        public sbyte Delay { get => this.delay; set => this.delay = value; }

        public sbyte Sound { get => this.sound; set => this.sound = value; }

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
            if (this.delay > 0)
            {
                this.delay -= (sbyte)delayAndSoundCycles;
            }
            else
            {
                this.delay = 0;
            }

            if (this.sound > 0)
            {
                this.sound -= (sbyte)delayAndSoundCycles;
                if (this.sound <= 0)
                {
                    Console.WriteLine("Beep");
                }
            }
            else
            {
                this.sound = 0;
            }
        }
    }
}