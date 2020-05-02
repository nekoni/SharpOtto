namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 8xy4 - ADD Vx, Vy
    /// Set Vx = Vx + Vy, set VF = carry.
    /// The values of Vx and Vy are added together. If the result is greater than 8 bits (i.e., > 255,) VF is set to 1, otherwise 0. Only the lowest 8 bits of the result are kept, and stored in Vx.
    /// </summary>
    internal class AddVxVyOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x8000 && o == 0x04)
            {
                var res = this.Interpreter.V[x] + this.Interpreter.V[y];
                this.Interpreter.V[15] = (byte) (res > 255 ? 1 : 0);
                this.Interpreter.V[x] = (byte) res;
                return true;
            }

            return false;
        }
    }
}