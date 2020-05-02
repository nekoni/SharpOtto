namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    // 8xy0 - LD Vx, Vy
    // Set Vx = Vy.
    // Stores the value of register Vy in register Vx.
    /// </summary>
    internal class LdVxVyOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x8000 && o == 0x00)
            {
                this.Interpreter.V[x] = this.Interpreter.V[y];
                return true;
            }

            return false;
        }
    }
}