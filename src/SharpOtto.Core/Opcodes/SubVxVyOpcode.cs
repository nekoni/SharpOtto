namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 8xy5 - SUB Vx, Vy
    /// Set Vx = Vx - Vy, set VF = NOT borrow.
    /// If Vx > Vy, then VF is set to 1, otherwise 0. Then Vy is subtracted from Vx, and the results stored in Vx.
    /// </summary>
    internal class SubVxVyOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x8000 && o == 0x05)
            {
                this.Interpreter.V[15] = 
                    (byte) (this.Interpreter.V[x] > this.Interpreter.V[y] ? 1 : 0);
                this.Interpreter.V[x] -= this.Interpreter.V[y];
                return true;
            }

            return false;
        }
    }
}