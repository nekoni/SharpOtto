namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    // 5xy0 - SE Vx, Vy
    // Skip next instruction if Vx = Vy.
    // The interpreter compares register Vx to register Vy, and if they are equal, increments the program counter by 2.
    /// </summary>
    internal class SeVxVyOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x5000)
            {
                if (this.Interpreter.V[x] == this.Interpreter.V[y])
                {
                    this.Interpreter.ProgramCounter += 2;
                }

                return true;
            }

            return false;
        }
    }
}