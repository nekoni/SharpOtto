namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 8xy7 - SUBN Vx, Vy
    /// Set Vx = Vy - Vx, set VF = NOT borrow.
    /// If Vy > Vx, then VF is set to 1, otherwise 0. Then Vx is subtracted from Vy, and the results stored in Vx.
    /// </summary>
    internal class SubnVxVyOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x8000 && o == 0x07)
            {
                this.Interpreter.V[15] =
                    (byte) (this.Interpreter.V[y] > this.Interpreter.V[x] ? 1 : 0);
                this.Interpreter.V[x] =
                    (byte) (this.Interpreter.V[y] - this.Interpreter.V[x]);
                return true;
            }

            return false;
        }
    }
}