namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 00EE - RET
    /// Return from a subroutine.
    /// </summary>
    internal class RetOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (opcode == 0x00EE)
            {
                this.Interpreter.ProgramCounter = this.Interpreter.Stack.Pop();
                return true;
            }

            return false;
        }
    }
}