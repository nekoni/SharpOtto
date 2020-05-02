namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// Bnnn - JP V0, addr
    /// Jump to location nnn + V0.
    /// The program counter is set to nnn plus the value of V0.
    /// </summary>
    internal class JpV0AddrOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xB000)
            {
                this.Interpreter.ProgramCounter = 
                    (ushort)(n + this.Interpreter.V[0]);
                this.SkipIncrementProgramCounter = true;
                return true;
            }

            return false;
        }
    }
}