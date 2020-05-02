namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// Fx15 - LD DT, Vx
    /// Set delay timer = Vx.
    /// DT is set equal to the value of Vx.
    /// </summary>
    internal class LdDtVxOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xF000 && k == 0x15)
            {
                this.Interpreter.Delay = (sbyte)this.Interpreter.V[x];
                return true;
            }

            return false;
        }
    }
}