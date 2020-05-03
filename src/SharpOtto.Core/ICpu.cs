namespace SharpOtto.Core
{
    public interface ICpu
    {
        ushort I { get; set; }

        byte[] V { get; }

        ushort ProgramCounter { get; set; }
    }
}