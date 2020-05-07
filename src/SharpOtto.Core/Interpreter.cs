using System.Collections.Generic;

namespace SharpOtto.Core
{
    public partial class Interpreter : IInterpreter
    {
        public bool EnableRecording { get; set; }

        public List<ushort> ExecutedOpcodes { get; } = new List<ushort>();

        public ushort ExitOnOpcode { get; set; }

        public void Run(byte[] data)
        {
            this.Load(data);
            this.InitTimers();
            this.InitCpu();
            this.ClearScreen();

            while (true)
            {
                this.UpdateTimers();
                if (!this.RunCpuCycles())
                {
                    return;
                }

                this.UpdateScreen();
            }
        }
    }
}