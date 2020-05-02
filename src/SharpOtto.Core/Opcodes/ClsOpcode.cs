namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 00E0 - CLS
    /// Clear the display.
    /// </summary>
    internal class ClsOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (opcode == 0x00E0)
            {
                this.Interpreter.ClearScreen();
                return true;
            }

            return false;
        }
    }
}