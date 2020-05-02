namespace SharpOtto.Core
{
    using System;

    public partial class Interpreter : ICpu
    {
        private ushort programCounter = 0;

        private ushort index = 0;

        private byte[] v = new byte[16];

        public ushort ProgramCounter { get => this.programCounter; set => this.programCounter = value; }

        public ushort I { get => this.index; set => this.index = value; }

        public byte[] V { get { return this.v; } }

        private void InitCpu()
        {
            Array.Clear(this.v, 0, this.v.Length);
            this.programCounter = 512;
        }
    }
}