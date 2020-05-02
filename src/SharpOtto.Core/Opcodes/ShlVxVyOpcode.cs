namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 8xyE - SHL Vx {, Vy}
    /// Set Vx = Vx SHL 1.
    /// If the most-significant bit of Vx is 1, then VF is set to 1, otherwise to 0. Then Vx is multiplied by 2.
    /// </summary>
    internal class ShlVxVyOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x8000 && o == 0x0E)
            {
                this.Interpreter.V[15] =
                    (byte) ((this.Interpreter.V[x] >> 7) & 1);
                this.Interpreter.V[x] *= 2;
                return true;
            }

            return false;
        }
    }
}