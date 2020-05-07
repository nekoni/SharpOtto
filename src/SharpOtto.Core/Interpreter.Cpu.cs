namespace SharpOtto.Core
{
    using System;

    public partial class Interpreter : ICpu
    {
        private OpcodeExecutor opcodeExecutor;

        public ushort ProgramCounter { get; set; } = 0;

        public ushort I { get; set; } = 0;

        public byte[] V { get; } = new byte[16];

        private void InitCpu()
        {
            Array.Clear(this.V, 0, this.V.Length);
            this.opcodeExecutor = new OpcodeExecutor(this);
            this.ProgramCounter = 512;
        }

        private bool RunCpuCycles()
        {
            var cycles = this.cpuTimer.GetCycles();
            for (var cycle = 0; cycle < cycles; cycle++)
            {
                var continueRun = this.opcodeExecutor.ExecuteNext();
                if (!continueRun)
                {
                    return false;
                }
            }

            return true;
        }
    }
}