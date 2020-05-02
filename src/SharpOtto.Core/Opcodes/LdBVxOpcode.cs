namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// Fx33 - LD B, Vx
    /// Store BCD representation of Vx in memory locations I, I+1, and I+2.
    /// The interpreter takes the decimal value of Vx, and places the hundreds digit in memory at location in I, the tens digit at location I+1, and the ones digit at location I+2.
    /// </summary>
    internal class LdBVxOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xF000 && k == 0x33)
            {
                var val = this.Interpreter.V[x].ToString("000");
                this.Interpreter.Memory[this.Interpreter.I] = byte.Parse(val[0].ToString());
                this.Interpreter.Memory[this.Interpreter.I+1] = byte.Parse(val[1].ToString());
                this.Interpreter.Memory[this.Interpreter.I+2] = byte.Parse(val[2].ToString());
                return true;
            }

            return false;
        }
    }
}