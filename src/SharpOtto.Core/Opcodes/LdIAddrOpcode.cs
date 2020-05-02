namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// Annn - LD I, addr
    /// Set I = nnn.
    /// The value of register I is set to nnn.
    /// </summary>
    internal class LdIAddrOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xA000)
            {
                this.Interpreter.I = n;
                return true;
            }

            return false;
        }
    }
}