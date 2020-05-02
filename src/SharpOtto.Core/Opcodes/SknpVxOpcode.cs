namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// ExA1 - SKNP Vx
    /// Skip next instruction if key with the value of Vx is not pressed.
    /// Checks the keyboard, and if the key corresponding to the value of Vx is currently in the up position, PC is increased by 2.
    /// </summary>
    internal class SknpVxOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xE000 && k == 0xA1)
            {
                if (!this.Interpreter.Keys[x])
                {
                    this.Interpreter.ProgramCounter += 2;
                }

                return true;
            }

            return false;
        }
    }
}