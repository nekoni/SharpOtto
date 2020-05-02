namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 9xy0 - SNE Vx, Vy
    /// Skip next instruction if Vx != Vy.
    /// The values of Vx and Vy are compared, and if they are not equal, the program counter is increased by 2.
    /// </summary>
    internal class SneVxVyOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x9000)
            {
                if (this.Interpreter.V[x] != this.Interpreter.V[y])
                {
                    this.Interpreter.ProgramCounter += 2;
                }

                return true;
            }

            return false;
        }
    }
}