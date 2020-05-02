namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 2nnn - CALL addr
    /// Call subroutine at nnn.
    /// The interpreter increments the stack pointer, then puts the current PC on the top of the stack. The PC is then set to nnn.
    /// </summary>
    internal class CallOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x2000)
            {
                this.Interpreter.Stack.Push(this.Interpreter.ProgramCounter);
                this.Interpreter.ProgramCounter = n;
                this.SkipIncrementProgramCounter = true;
                return true;
            }

            return false;
        }
    }
}