namespace SharpOtto.Core
{
    using System.Collections.Generic;

    internal interface IMemory
    {
        byte[] Memory { get; }

        Stack<ushort> Stack { get; }
    }
}