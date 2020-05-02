namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 6xkk - LD Vx, byte
    /// Set Vx = kk.
    /// The interpreter puts the value kk into register Vx.
    /// </summary>
    internal class LdVxByteOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x6000)
            {
                this.Interpreter.V[x] = k;
                return true;
            }

            return false;
        }
    }
}