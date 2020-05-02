namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    // 1nnn - JP addr
    // Jump to location nnn.
    // The interpreter sets the program counter to nnn.
    /// </summary>
    internal class JpOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x1000)
            {
                this.Interpreter.ProgramCounter = n;
                this.SkipIncrementProgramCounter = true;
                return true;
            }

            return false;
        }
    }
}