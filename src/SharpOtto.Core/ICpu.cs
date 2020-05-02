namespace SharpOtto.Core
{
    internal interface ICpu
    {
        ushort I { get; set; }

        byte[] V { get; }

        ushort ProgramCounter { get; set; }
    }
}