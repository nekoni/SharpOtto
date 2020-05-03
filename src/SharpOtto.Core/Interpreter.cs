namespace SharpOtto.Core
{
    public partial class Interpreter : IInterpreter
    {
        public void Run(byte[] data)
        {
            this.Load(data);
            this.InitTimers();
            this.InitCpu();
            this.ClearScreen();

            while (true)
            {
                this.UpdateTimers();
                this.RunCpuCycles();
                this.UpdateScreen();
            }
        }
    }
}