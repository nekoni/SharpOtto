namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// Fx18 - LD ST, Vx
    /// Set sound timer = Vx.
    /// ST is set equal to the value of Vx.
    /// </summary>
    internal class LdStVxOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xF000 && k == 0x18)
            {
                this.Interpreter.Sound = (sbyte)this.Interpreter.V[x];
                return true;
            }

            return false;
        }
    }
}