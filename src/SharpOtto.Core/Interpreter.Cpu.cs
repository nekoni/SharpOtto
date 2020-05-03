namespace SharpOtto.Core
{
    using System;

    public partial class Interpreter : ICpu
    {
        private ushort programCounter = 0;

        private ushort index = 0;

        private byte[] v = new byte[16];

        private OpcodeExecutor opcodeExecutor;

        public ushort ProgramCounter { get => this.programCounter; set => this.programCounter = value; }

        public ushort I { get => this.index; set => this.index = value; }

        public byte[] V { get { return this.v; } }

        private void InitCpu()
        {
            Array.Clear(this.v, 0, this.v.Length);
            this.opcodeExecutor = new OpcodeExecutor(this);
            this.programCounter = 512;
        }

        private void RunCpuCycles()
        {
            var cycles = this.cpuTimer.GetCycles();
            for (var cycle = 0; cycle < cycles; cycle++)
            {
                this.opcodeExecutor.ExecuteNext();
            }
        }
    }
}