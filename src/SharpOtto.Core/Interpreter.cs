namespace SharpOtto.Core
{
    public partial class Interpreter : IInterpreter
    {
        private OpcodeExecutor opcodeExecutor;

        public Interpreter()
        {
            this.opcodeExecutor = new OpcodeExecutor(this);
        }

        public void RunGame(byte[] data)
        {
            this.LoadGame(data);
            this.InitTimers();
            this.InitCpu();
            this.ClearScreen();

            while (true)
            {
                this.UpdateTimers();

                var cycles = this.GetCpuCycles();
                for (var cycle = 0; cycle < cycles; cycle++)
                {
                    this.opcodeExecutor.ExecuteNext();
                }

                this.UpdateScreen();
            }
        }
    }
}