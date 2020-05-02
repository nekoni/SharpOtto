namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// Fx1E - ADD I, Vx
    /// Set I = I + Vx.
    /// The values of I and Vx are added, and the results are stored in I.
    /// </summary>
    internal class AddIVxOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xF000 && k == 0x1E)
            {
                this.Interpreter.I += this.Interpreter.V[x];
                return true;
            }

            return false;
        }
    }
}