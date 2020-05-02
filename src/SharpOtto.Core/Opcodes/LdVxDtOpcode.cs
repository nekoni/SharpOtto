namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// Fx07 - LD Vx, DT
    /// Set Vx = delay timer value.
    /// The value of DT is placed into Vx.
    /// </summary>
    internal class LdVxDtOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xF000 && k == 0x07)
            {
                this.Interpreter.V[x] = (byte)this.Interpreter.Delay;
                return true;
            }

            return false;
        }
    }
}