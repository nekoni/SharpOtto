namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// Fx29 - LD F, Vx
    /// Set I = location of sprite for digit Vx.
    /// The value of I is set to the location for the hexadecimal sprite corresponding to the value of Vx. See section 2.4, Display, for more information on the Chip-8 hexadecimal font.
    /// </summary>
    internal class LdFVxOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xF000 && k == 0x29)
            {
                this.Interpreter.I = (ushort)(this.Interpreter.V[x] * 5);
                return true;
            }

            return false;
        }
    }
}