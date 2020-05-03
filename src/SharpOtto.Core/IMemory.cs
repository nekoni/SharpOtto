namespace SharpOtto.Core
{
    using System.Collections.Generic;

    public interface IMemory
    {
        byte[] Memory { get; }

        Stack<ushort> Stack { get; }
    }
}