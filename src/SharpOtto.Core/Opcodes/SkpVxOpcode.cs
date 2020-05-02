namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// Ex9E - SKP Vx
    /// Skip next instruction if key with the value of Vx is pressed.
    /// Checks the keyboard, and if the key corresponding to the value of Vx is currently in the down position, PC is increased by 2.
    /// </summary>
    internal class SkpVxOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xE000 && k == 0x9E)
            {
                if (this.Interpreter.Keys[x])
                {
                    this.Interpreter.ProgramCounter += 2;
                }

                return true;
            }

            return false;
        }
    }
}