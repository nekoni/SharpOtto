namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 7xkk - ADD Vx, byte
    /// Set Vx = Vx + kk.
    /// Adds the value kk to the value of register Vx, then stores the result in Vx. 
    /// </summary>
    internal class AddVxByteOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x7000)
            {
                this.Interpreter.V[x] += k;
                return true;
            }

            return false;
        }
    }
}