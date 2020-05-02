namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 3xkk - SE Vx, byte
    /// Skip next instruction if Vx = kk.
    /// The interpreter compares register Vx to kk, and if they are equal, increments the program counter by 2.
    /// </summary>
    internal class SeVxByteOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x3000)
            {
                if (this.Interpreter.V[x] == k)
                {
                    this.Interpreter.ProgramCounter += 2;
                }

                return true;
            }

            return false;
        }
    }
}