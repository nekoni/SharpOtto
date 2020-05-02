namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 8xy6 - SHR Vx {, Vy}
    /// Set Vx = Vx SHR 1.
    /// If the least-significant bit of Vx is 1, then VF is set to 1, otherwise 0. Then Vx is divided by 2.
    /// </summary>
    internal class ShrVxVyOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x8000 && o == 0x06)
            {
                this.Interpreter.V[15] = (byte) (this.Interpreter.V[x] & 1);
                this.Interpreter.V[x] /= 2;
                return true;
            }

            return false;
        }
    }
}