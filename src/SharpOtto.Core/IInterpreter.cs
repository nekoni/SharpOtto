namespace SharpOtto.Core
{
    using System.Collections.Generic;

    public interface IInterpreter : IMemory, ICpu, ITimer, IScreen, IKeypad
    {
        bool EnableRecording { get; set; }

        List<ushort> ExecutedOpcodes { get; }

        ushort ExitOnOpcode { get; set; }

        void Run(byte[] data);
    }
}