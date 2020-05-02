namespace SharpOtto.Core
{
    internal interface IInterpreter : IMemory, ICpu, ITimer, IScreen, IKeypad
    {
    }
}