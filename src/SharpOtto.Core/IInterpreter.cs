namespace SharpOtto.Core
{
    public interface IInterpreter : IMemory, ICpu, ITimer, IScreen, IKeypad
    {
        void Run(byte[] data);
    }
}